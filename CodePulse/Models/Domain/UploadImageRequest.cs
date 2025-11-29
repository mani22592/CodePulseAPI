namespace CodePulse.Models.Domain
{
    public class UploadImageRequest
    {
        public IFormFile ImageFile { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
    }
}
