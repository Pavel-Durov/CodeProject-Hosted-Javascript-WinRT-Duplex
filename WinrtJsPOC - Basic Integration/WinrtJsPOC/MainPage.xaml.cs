using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinrtJsPOC.Handlers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WinrtJsPOC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to wi thin a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            webView.ScriptNotify += webView_ScriptNotify;
        }

        void webView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e != null && !String.IsNullOrEmpty(e.Value))
            {
                Debug.WriteLine("JS Message  : {0}", e.Value);
            }
        }

        async  void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Uri path = webView.BuildLocalStreamUri("someIdentifier", "index.html");
            var uriResolver = new UrlResolver();

            try
            {
                webView.NavigateToLocalStreamUri(path, uriResolver);
            }
            catch (Exception ex)
            {
                if (ex != null)
                    Debug.WriteLine(ex.ToString());
            }

            await Task.Delay(2000);
            await NotifyScript();
        }

        public async Task NotifyScript()
        {
            try
            {
                await webView.InvokeScriptAsync("Test", new string[] { "hello from C#!" });
            }
            catch (Exception e)
            {
                //throw;
            }
        }

    }
}
