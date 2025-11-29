using CodePulse.Data;
using CodePulse.Models.Domain;
using CodePulse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAllBlogImages()
        {
            return await _dbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //upload the image to wwwroot/images folder
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "images",$"{blogImage.FileName}{blogImage.FileExtension}");

            using (var stream = new FileStream(localPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var httpRequest = _httpContextAccessor?.HttpContext?.Request;
            var url = $"{httpRequest?.Scheme}://{httpRequest?.Host}{httpRequest?.PathBase}/images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = url;
            _dbContext.BlogImages.Add(blogImage);
            await _dbContext.SaveChangesAsync();
            return blogImage;

        }
    }
}
