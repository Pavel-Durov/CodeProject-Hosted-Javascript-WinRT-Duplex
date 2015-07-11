using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace WinRTAsyncCallbacks
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            webView.ScriptNotify += webView_ScriptNotify;

        }

        async void webView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e != null && !String.IsNullOrEmpty(e.Value))
            {
                Debug.WriteLine("JS Message  : {0}", e.Value);
                JObject obj = JsonConvert.DeserializeObject<JObject>(e.Value);

                JToken data, guid;

                if (obj.TryGetValue("data", out data) && obj.TryGetValue("guid", out guid)) 
                {
                    await DoSomethingWithData(data);
                    var guidStr = guid.Value<string>();

                    await NotifyScript("Message recieved with guid : "+ guidStr, guidStr);
                }
            }
        }

        private Task DoSomethingWithData(JToken data)
        {
            return Task.Delay(100);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
       { 
            Uri path = webView.BuildLocalStreamUri("someIdentifier", "index.html");
            var uriResolver = new LocalDolerUrlResolver();
            try
            {
                webView.NavigateToLocalStreamUri(path, uriResolver);
             
            }
            catch (Exception ex)
            {
                if (ex != null)
                    Debug.WriteLine(ex.ToString());
            }
        }

        public async Task NotifyScript(string guid, string data)
        {
            try
            {
                await webView.InvokeScriptAsync("NotifyJSFromWinRT", new string[] { guid, data });
            }
            catch (Exception e)
            {
                //throw;
            }
        }
    }
}





