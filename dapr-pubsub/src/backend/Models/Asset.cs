using Microsoft.AspNetCore.Http;

namespace backend.Models
{
    public class Asset
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}