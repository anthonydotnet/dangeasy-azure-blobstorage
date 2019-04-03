using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DangEasy.Azure.BlobStorage.Test.Integration
{
    public class BaseIntegration : IDisposable
    {
        protected IConfigurationRoot Configuration;
        protected BlobStorageClient Client;
        protected const string TextFileBody = "This is a text file";
        protected string ContainerName;


        public BaseIntegration()
        {
            var sharedFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Example.Console");

            var builder = new ConfigurationBuilder()
                          .SetBasePath(sharedFolder)
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            ContainerName = $"my-test-container{DateTime.UtcNow.Ticks}";
            Client = new BlobStorageClient(Configuration["AppSettings:ConnectionString"]);

            // force this to execute!
            Client.CreateContainerAsync(ContainerName).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        public void Dispose()
        {
            Client.CloudBlobClient.GetContainerReference(ContainerName).DeleteIfExistsAsync().Wait();
        }
    }
}