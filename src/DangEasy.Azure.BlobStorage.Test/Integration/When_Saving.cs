using System;
using System.IO;
using System.Text;
using Xunit;

namespace DangEasy.Azure.BlobStorage.Test.Integration
{
    public class When_Saving : BaseIntegration
    {
        [Fact]
        public void File_Is_Uploaded()
        {
            // upload file
            var filePath = $"example.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(TextFileBody));
            var result = Client.SaveFileAsync(filePath, stream).Result;

            Assert.True(result);
        }
    }
}
