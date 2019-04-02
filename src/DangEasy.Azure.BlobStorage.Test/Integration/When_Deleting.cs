using System.IO;
using System.Text;
using Xunit;

namespace DangEasy.Azure.BlobStorage.Test.Integration
{
    public class When_Deleting : BaseIntegration
    {
        [Fact]
        public void Blob_Is_Deleted()
        {
            // upload file
            var filePath = $"example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveFileAsync(filePath, stream).GetAwaiter().GetResult();

            var result = Client.DeleteAsync(filePath).Result;

            Assert.True(result);
        }
    }
}
