using System;
using System.IO;
using System.Text;
using Xunit;

namespace DangEasy.BlobStorage.Azure.Test.Integration
{
    public class When_Saving : BaseIntegration
    {
        [Fact]
        public async void File_Is_Uploaded()
        {
            // upload file
            var filePath = $"/{ContainerName}/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            var result = await Client.SaveAsync(filePath, stream);

            Assert.True(result);
        }


        [Fact]
        public async void File_Is_Uploaded_To_Deep_Path()
        {
            // upload file
            var filePath = $"/{ContainerName}/folder/anotherfolder/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            var result = await Client.SaveAsync(filePath, stream);

            Assert.True(result);
        }
    }
}
