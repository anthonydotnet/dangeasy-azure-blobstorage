using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace DangEasy.Azure.BlobStorage.Test.Integration
{
    public class When_Getting_Blob_Info : BaseIntegration
    {
        [Fact]
        public void BlobList_Is_Returned()
        {
            // upload file
            var filePath = $"example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveFileAsync(filePath, stream).GetAwaiter().GetResult();

            var result = Client.GetListAsync($"").Result;

            Assert.NotEmpty(result);
            Assert.EndsWith(filePath, result.First());
        }


        [Fact]
        public void Existing_Blob_Returns_True()
        {
            // upload file
            var filePath = $"example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveFileAsync(filePath, stream).GetAwaiter().GetResult();

            var result = Client.ExistsAsync(filePath).Result;

            Assert.True(result);
        }


        [Fact]
        public void Existing_Blob_Returns_Info()
        {
            // upload file
            var filePath = $"example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            Client.SaveFileAsync(filePath, stream).GetAwaiter().GetResult();

            var result = Client.GetInfoAsync(filePath).Result;

            Assert.Equal(filePath, result.Path);

            Assert.EndsWith(filePath, result.AbsoluteUri);
            Assert.True(result.Created > DateTime.MinValue);
            Assert.True(result.Modified > DateTime.MinValue);
            Assert.True(result.Size > 0);
        }
    }
}
