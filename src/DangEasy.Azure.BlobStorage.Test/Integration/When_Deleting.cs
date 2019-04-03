using System.IO;
using System.Text;
using Xunit;

namespace DangEasy.Azure.BlobStorage.Test.Integration
{
    public class When_Deleting : BaseIntegration
    {
        [Fact]
        public async void Blob_Is_Deleted()
        {
            // upload file
            var filePath = $"/{ContainerName}/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            await  Client.SaveAsync(filePath, stream);

            var result = await Client.DeleteAsync(filePath);

            Assert.True(result);
        }
    }
}
