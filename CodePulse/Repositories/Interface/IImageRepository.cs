using CodePulse.Models.Domain;
using System.Net;

namespace CodePulse.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAllBlogImages();
    }
}
