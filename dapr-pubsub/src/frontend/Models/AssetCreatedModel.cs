using Microsoft.AspNetCore.Http;

namespace frontend.Models
{
    public class AssetCreatedModel
    {
        public string FileName { get; set; }
        public string AssetEtag { get; set; }
        public string AssetTemplateName { get; set; }
    }
}