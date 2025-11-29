using CodePulse.Models.Domain;

namespace CodePulse.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetBlogPostByIdAsync(Guid id);
        Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteBlogPostById(Guid id);
    }
}
