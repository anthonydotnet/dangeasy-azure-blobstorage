using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using DangEasy.Azure.BlobStorage.Models;
using DangEasy.Interfaces.BlobStorage;

namespace DangEasy.Azure.BlobStorage
{
    public class BlobStorageClient : IBlobStorage
    {
        public readonly CloudBlobClient CloudBlobClient;
        public readonly CloudStorageAccount StorageAccount;

        public BlobStorageClient(string connectionString)
        {
            StorageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient = StorageAccount.CreateCloudBlobClient();
        }

        public async Task<bool> CreateContainerAsync(string containerName)
        {
            var container = CloudBlobClient.GetContainerReference(containerName);
            return await container.CreateIfNotExistsAsync().ConfigureAwait(false);
        }


        public async Task<bool> DeleteAsync(string path)
        {
            var blob = GetBlobReference(path);

            return await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, null, null).ConfigureAwait(false);
        }


        public async Task<bool> DeleteContainerAsync(string containerName)
        {
            var container = CloudBlobClient.GetContainerReference(containerName);
            var res = await container.DeleteIfExistsAsync().ConfigureAwait(false);

            return res;
        }


        public async Task<bool> ExistsAsync(string path)
        {
            var blob = GetBlobReference(path);

            return await blob.ExistsAsync().ConfigureAwait(false);
        }


        public async Task<Stream> GetAsync(string path)
        {
            var blob = GetBlobReference(path);

            MemoryStream ms = new MemoryStream();
            await blob.DownloadToStreamAsync(ms).ConfigureAwait(false);

            return ms;
        }


        public async Task<IBlobInformation> GetInfoAsync(string path)
        {
            var blob = GetBlobReference(path);
            try
            {
                await blob.FetchAttributesAsync().ConfigureAwait(false);
                return GetBlobInfo(blob);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<IEnumerable<string>> GetListAsync(string path)
        {
            CloudBlobDirectory dir = GetCloudBlobDirectory(path);

            List<string> results = new List<string>();
            BlobContinuationToken token = null;
            do
            {
                var resultSegment = await dir.ListBlobsSegmentedAsync(token).ConfigureAwait(false);

                var blobPath = resultSegment.Results.Select(x => x.Uri.LocalPath);
                results.AddRange(blobPath);
                token = resultSegment.ContinuationToken;
            }
            while (token != null);

            return results;
        }


        public async Task<bool> SaveAsync(string path, Stream stream)
        {
            var blob = GetBlobReference(path);
            await blob.UploadFromStreamAsync(stream).ConfigureAwait(false);

            return true;
        }


        //--
        // Some messy building
        //--

        private CloudBlobContainer GetContainerReference(string path)
        {
            var trimmed = path.TrimStart('/');
            var urlParts = trimmed.Split("/".ToCharArray(), StringSplitOptions.None).ToList();

            var containerName = urlParts[0];
            var container = CloudBlobClient.GetContainerReference(urlParts[0]);

            return container;
        }


        private CloudBlobDirectory GetCloudBlobDirectory(string path)
        {
            var trimmed = path.TrimStart('/');
            var urlParts = trimmed.Split("/".ToCharArray(), StringSplitOptions.None).ToList();

            var containerName = urlParts[0];
            var container = CloudBlobClient.GetContainerReference(urlParts[0]);

            urlParts.RemoveAt(0);
            var relativePath = string.Join("/", urlParts);

            return container.GetDirectoryReference(relativePath);
        }


        private CloudBlockBlob GetBlobReference(string path)
        {
            var trimmed = path.TrimStart('/');
            var urlParts = trimmed.Split("/".ToCharArray(), StringSplitOptions.None).ToList();

            var containerName = urlParts[0];
            var container = CloudBlobClient.GetContainerReference(urlParts[0]);

            urlParts.RemoveAt(0);
            var relativePath = string.Join("/", urlParts);

            return container.GetBlockBlobReference(relativePath);
        }


        protected IBlobInformation GetBlobInfo(CloudBlob blob)
        {
            if (blob.Properties.Length == -1)
            {
                return null;
            }

            return new BlobInformation
            {
                Path = blob.Uri.LocalPath,
                AbsoluteUri = blob.Uri.AbsoluteUri,
                Size = blob.Properties.Length,
                Created = blob.Properties.LastModified.Value.UtcDateTime,
                Modified = blob.Properties.LastModified.Value.UtcDateTime
            };
        }        
    }
}
