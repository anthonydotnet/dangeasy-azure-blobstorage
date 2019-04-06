using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace DangEasy.BlobStorage.Azure.Test.Integration
{
    public class When_Getting_Blob_Info : BaseIntegration
    {
        [Fact]
        public async void BlobList_Is_Returned()
        {
            // upload file
            var filePath = $"/{ContainerName}/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveAsync(filePath, stream).GetAwaiter().GetResult();
            

            var result = await Client.GetListAsync($"/{ContainerName}");

            Assert.NotEmpty(result);
            Assert.EndsWith(filePath, result.First());
        }


        [Fact]
        public async void Existing_Blob_Returns_True()
        {
            // upload file
            var filePath = $"/{ContainerName}/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveAsync(filePath, stream).GetAwaiter().GetResult();
            

            var result = await Client.ExistsAsync(filePath);

            Assert.True(result);
        }


        [Fact]
        public async void Existing_Blob_Returns_Info()
        {
            // upload file
            var filePath = $"/{ContainerName}/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveAsync(filePath, stream).GetAwaiter().GetResult();
            

            var result = await Client.GetInfoAsync(filePath);

            Assert.Equal(filePath, result.Path);

            Assert.EndsWith(filePath, result.AbsoluteUri);
            Assert.True(result.Created > DateTime.MinValue);
            Assert.True(result.Modified > DateTime.MinValue);
            Assert.True(result.Size > 0);
        }


        [Fact]
        public async void Existing_Blob_Folder_Returns_True()
        {
            // upload file
            var filePath = $"/{ContainerName}/myfolder/example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveAsync(filePath, stream).GetAwaiter().GetResult();
            

            var result = await Client.GetListAsync($"/{ContainerName}/myfolder");

            Assert.NotEmpty(result);
        }
    }
}
