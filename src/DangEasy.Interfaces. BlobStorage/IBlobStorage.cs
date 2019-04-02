using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DangEasy.Interfaces.BlobStorage
{
    public interface IBlobStorage
    {
        Task<bool> DeleteAsync(string path);
        Task<bool> ExistsAsync(string path);
        Task<Stream> GetAsync(string filePath);
        Task<IBlobInformation> GetInfoAsync(string path);
        Task<IEnumerable<string>> GetListAsync(string path);
        Task<bool> SaveFileAsync(string filePath, Stream stream);
    }


    public interface IBlobInformation
    {
        string AbsoluteUri { get; set; }
        string Path { get; set; }
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
        long Size { get; set; }
    }
}