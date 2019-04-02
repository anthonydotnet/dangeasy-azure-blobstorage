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
        public readonly CloudBlobContainer CloudBlobContainer;
        public readonly CloudStorageAccount StorageAccount;

        public BlobStorageClient(string connectionString, string containerName)
        {
            StorageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient = StorageAccount.CreateCloudBlobClient();

            CloudBlobContainer = CreateContainerAsync(containerName).Result;
        }




        public async Task<bool> DeleteAsync(string path)
        {
            var blob = CloudBlobContainer.GetBlockBlobReference(path);

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
            var blob = CloudBlobContainer.GetBlockBlobReference(path);

            return await blob.ExistsAsync().ConfigureAwait(false);
        }


        public async Task<Stream> GetAsync(string path)
        {
            var blob = CloudBlobContainer.GetBlockBlobReference(path);

            MemoryStream ms = new MemoryStream();
            await blob.DownloadToStreamAsync(ms).ConfigureAwait(false);

            return ms;
        }


        public async Task<IBlobInformation> GetInfoAsync(string path)
        {
            var blob = CloudBlobContainer.GetBlockBlobReference(path);
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
            var directory = CloudBlobContainer.GetDirectoryReference(path);

            CloudBlobDirectory dir = CloudBlobContainer.GetDirectoryReference(path);

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


        public async Task<bool> SaveFileAsync(string path, Stream stream)
        {
            var blob = CloudBlobContainer.GetBlockBlobReference(path);
            await blob.UploadFromStreamAsync(stream).ConfigureAwait(false);

            return true;
        }




        protected async Task<CloudBlobContainer> CreateContainerAsync(string containerName)
        {
            var container = CloudBlobClient.GetContainerReference(containerName);
            var res = await container.CreateIfNotExistsAsync();

            return container;
        }


        protected IBlobInformation GetBlobInfo(CloudBlob blob)
        {
            if (blob.Properties.Length == -1)
            {
                return null;
            }

            return new BlobInformation
            {
                Path = blob.Name,
                AbsoluteUri = blob.Uri.AbsoluteUri,
                Size = blob.Properties.Length,
                Created = blob.Properties.LastModified.Value.UtcDateTime,
                Modified = blob.Properties.LastModified.Value.UtcDateTime
            };
        }
    }
}
