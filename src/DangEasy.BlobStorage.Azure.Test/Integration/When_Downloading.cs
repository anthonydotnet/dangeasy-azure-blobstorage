using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace DangEasy.BlobStorage.Azure.Test.Integration
{
    public class When_Downloading : BaseIntegration
    {
        [Fact]
        public async void Blob_Is_Returned()
        {
            // upload file
            var filePath = $"/{ContainerName}/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveAsync(filePath, stream).GetAwaiter().GetResult();



            var downloadedStream = await Client.GetAsync(filePath) as MemoryStream;
            string result = Encoding.UTF8.GetString(downloadedStream.ToArray());

            Assert.Equal(TextFileBody, result);
        }
    }
}
