using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace DangEasy.BlobStorage.Azure.Test.Integration
{
    public class When_Deleting : BaseIntegration
    {
        [Fact]
        public async void Blob_Is_Deleted()
        {
            // upload file
            var filePath = $"/{ContainerName}/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveAsync(filePath, stream).GetAwaiter().GetResult();
            

            var result = await Client.DeleteAsync(filePath);

            Assert.True(result);
        }
    }
}
