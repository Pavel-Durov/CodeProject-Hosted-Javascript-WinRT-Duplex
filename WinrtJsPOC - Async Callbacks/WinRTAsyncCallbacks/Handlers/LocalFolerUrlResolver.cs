using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web;

namespace WinrtJsPOC.Handlers
{
    public class LocalDolerUrlResolver : IUriToStreamResolver
    {
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri fileName)
        {
            IAsyncOperation<IInputStream> result = null;
            
            result = GetContent(fileName.AbsolutePath).AsAsyncOperation();
            return result;
        }

        private async Task<IInputStream> GetContent(string fileName)
        {
            IRandomAccessStream result = null;
            String path = fileName.Replace("/", "");
            var storageFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync(path);
            if (storageFile != null && storageFile.IsOfType(StorageItemTypes.File))
            {
                StorageFile file = storageFile as StorageFile;
                result = await file.OpenAsync(FileAccessMode.Read);
            }
            return result;
        }
    }

}
