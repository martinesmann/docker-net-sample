using Microsoft.AspNetCore.Http;

namespace frontend.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }

        // multiple files upload support, if needed
        //public List<IFormFile> FormFiles { get; set; }
    }
}