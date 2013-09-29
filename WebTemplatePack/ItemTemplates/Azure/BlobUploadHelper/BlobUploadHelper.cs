using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace $rootnamespace$
{
    public class $safeitemname$
    {
        private string _connectionString;
        private CloudStorageAccount StorageAccount { get; set; }

        public $safeitemname$()
            : this(ConfigurationManager
                .ConnectionStrings["$safeitemname$.ConnectionString"]
                    .ConnectionString)
        {
        }

        public $safeitemname$(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CloudBlobClient GetBlobClient()
        {
            StorageAccount = CloudStorageAccount.Parse(_connectionString);
            return StorageAccount.CreateCloudBlobClient();
        }

        public CloudBlobContainer GetBlobContainer(string containerName,
            BlobContainerPublicAccessType accessType = BlobContainerPublicAccessType.Blob)
        {
            var container = GetBlobClient().GetContainerReference(containerName);
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = accessType
            });
            return container;
        }

        public string UploadFileToBlob(HttpPostedFileBase postedFile, string containerName = "uploads")
        {
            var container = GetBlobContainer(containerName);
            var blob = container.GetBlockBlobReference(Path.GetFileName(postedFile.FileName));
            using (postedFile.InputStream)
            {
                blob.UploadFromStream(postedFile.InputStream);
            }
            return blob.Uri.AbsoluteUri;
        }

        public List<string> GetBlobList(string containerName)
        {
            var container = GetBlobContainer(containerName);
            var ret = container.ListBlobs().Select(x => x.Uri.AbsoluteUri).ToList();
            return ret;
        }
    }
}