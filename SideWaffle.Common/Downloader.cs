namespace SideWaffle.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using SideWaffle.Commons.Extensions;
    using System.IO;

    public class Downloader {
        public async Task Download(string url, string destination) {
            if (string.IsNullOrEmpty(url)) { throw new ArgumentNullException("url"); }
            if (string.IsNullOrEmpty(destination)) { throw new ArgumentNullException("destination"); }

            // download the file to the destinaiton
            await DownloadFileAsync(new Uri(url), destination);
        }

        private async Task DownloadFileAsync(Uri uri, string destniation) {
            if (uri == null) { throw new ArgumentNullException("uri"); }
            if (string.IsNullOrEmpty(destniation)) { throw new ArgumentNullException("destniation"); }

            // see if the directory for the file exists, if not create it
            var destFi = new FileInfo(destniation);
            if (!destFi.Directory.Exists) {
                destFi.Directory.Create();
            }

            using (var client = new HttpClient()) {
                var result = await client.GetAsync(uri);
                result.EnsureSuccessStatusCode();

                await result.Content.LoadIntoBufferAsync();

                await result.Content.ReadAsFileAsync(destFi.FullName, true);
            }

        }
    }
}
