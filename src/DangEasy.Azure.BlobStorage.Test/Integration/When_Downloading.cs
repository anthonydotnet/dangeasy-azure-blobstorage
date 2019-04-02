using System.IO;
using System.Text;
using Xunit;

namespace DangEasy.Azure.BlobStorage.Test.Integration
{
    public class When_Downloading : BaseIntegration
    {
        [Fact]
        public void Blob_Is_Returned()
        {
            // upload file
            var filePath = $"example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveAsync(filePath, stream).GetAwaiter().GetResult();

            var downloadedStream = Client.GetAsync(filePath).Result as MemoryStream;
            string result = Encoding.UTF8.GetString(downloadedStream.ToArray());

            Assert.Equal(TextFileBody, result);
        }
    }
}
