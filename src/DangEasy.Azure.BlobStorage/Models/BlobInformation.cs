using System;
using DangEasy.Interfaces.BlobStorage;

namespace DangEasy.Azure.BlobStorage.Models
{
    public class BlobInformation : IBlobInformation
    {
        public string AbsoluteUri { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public long Size { get; set; }
    }
}
