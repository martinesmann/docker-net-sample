using Microsoft.AspNetCore.Http;

namespace backend.Models
{
    public class AssetData
    {
        public string FileName { get; set; }
        public string AssetEtag { get; set; }
        public string AssetTemplateName { get; set; }
    }
}