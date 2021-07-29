using Microsoft.AspNetCore.Http;

namespace frontend.Models
{
    public class AssetData
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}