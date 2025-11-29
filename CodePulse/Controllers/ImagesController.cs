using CodePulse.Models.Domain;
using CodePulse.Models.DTO;
using CodePulse.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        //GET: {apibaseurl}/api/images/getallblogimages
        [HttpGet("getallblogimages")]
        public async Task<IActionResult> GetAllBlogImages()
        {
            var blogImages = await _imageRepository.GetAllBlogImages();

            //convert domain model to Dto

            var response = new List<BlogImageDto>();
            foreach (var blogImage in blogImages)
            {
                response.Add(new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    FileName = blogImage.FileName,
                    FileExtension = blogImage.FileExtension,
                    Url = blogImage.Url,
                    DateCreated = blogImage.DateCreated
                });
            }

            return Ok(response);
        }

        // Request model for multipart/form-data - Swagger generates correct schema when using a single model        
        // POST: {apibaseurl}/api/images/UploadImage
        [HttpPost("UploadImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
        {
            if (request?.ImageFile == null || request.ImageFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            ValidateFileUpload(request.ImageFile);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fileNameToUse = string.IsNullOrWhiteSpace(request.FileName)
                ? Path.GetFileNameWithoutExtension(request.ImageFile.FileName)
                : request.FileName;

            var blogImage = new BlogImage
            {
                FileName = fileNameToUse,
                FileExtension = Path.GetExtension(request.ImageFile.FileName),
                Title = request.Title,
                DateCreated = DateTime.UtcNow
            };

            blogImage = await _imageRepository.Upload(request.ImageFile, blogImage);

            var response = new BlogImageDto
            {
                Id = blogImage.Id,
                Title = blogImage.Title,
                FileName = blogImage.FileName,
                FileExtension = blogImage.FileExtension,
                Url = blogImage.Url,
                DateCreated = blogImage.DateCreated
            };

            return Ok(response);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
            var maxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB
            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? string.Empty;

            if (!allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(UploadImageRequest.ImageFile), "Invalid file type. Only image files are allowed.");
            }
            if (file.Length > maxFileSizeInBytes)
            {
                ModelState.AddModelError(nameof(UploadImageRequest.ImageFile), "File size exceeds the maximum limit of 10 MB.");
            }
        }
    }
}
