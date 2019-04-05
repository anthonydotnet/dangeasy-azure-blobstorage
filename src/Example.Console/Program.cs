using System.Linq;
using DangEasy.BlobStorage.Azure;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Example.Console
{
    class Program
    {
        const string TextFileBody = "This is a text file";
        const string ContainerName = "mycontainer";

        IConfigurationRoot Configuration;
        BlobStorageClient _client;
        public Program()
        {
            var builder = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            _client = new BlobStorageClient(Configuration["AppSettings:ConnectionString"]); // must be lower case

            _client.CreateContainerAsync(ContainerName).GetAwaiter().GetResult();  
        }


        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello Storage!");

            var p = new Program();
            p.Run();
        }


        public void Run()
        {
            // upload file
            var filePath = $"/{ContainerName}/myfolder/example.txt"; // relative to the container
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            System.Console.WriteLine($"\nUploading file {filePath}");
            var saved = _client.SaveAsync(filePath, stream).GetAwaiter().GetResult();
            System.Console.WriteLine($"Uploaded: {saved}");


            // file exists
            System.Console.WriteLine($"\nFile exists?");
            var exists = _client.ExistsAsync(filePath).Result;
            System.Console.WriteLine($"{exists}");


            // file info
            System.Console.WriteLine($"\nBlob info:");
            var info = _client.GetInfoAsync(filePath).Result;
            System.Console.WriteLine($"{info.Path}, Created:{info.Created}, Modified:{info.Modified}, Size:{info.Size}");


            // show root blobs - should have 1 blob
            System.Console.WriteLine($"\nShowing blobs root");
            var blobNames = _client.GetListAsync($"/{ContainerName}").Result;
            blobNames.ToList().ForEach(x => System.Console.WriteLine(x));


            // show blobs in myfolder - should have 1 blobs
            System.Console.WriteLine($"\nShowing blobs in myfolder");
            blobNames = _client.GetListAsync($"/{ContainerName}/myfolder").Result;
            blobNames.ToList().ForEach(x => System.Console.WriteLine(x));


            // download file
            System.Console.WriteLine($"\nDownloading {filePath}");
            var downloadedStream = _client.GetAsync(filePath).Result as MemoryStream;
            var resultString = Encoding.UTF8.GetString(downloadedStream.ToArray());
            System.Console.WriteLine(resultString);


            // delete file
            System.Console.WriteLine($"\nDelete {filePath}");
            var deleted = _client.DeleteAsync(filePath).Result;
            System.Console.WriteLine($"Deleted: {deleted}");

            System.Console.ReadLine();
        }
    }
}
