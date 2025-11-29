using CodePulse.Data;
using CodePulse.Models.Domain;
using CodePulse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost> DeleteBlogPostById(Guid id)
        {
            var blogPost = await _dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (blogPost is not null)
            {
                _dbContext.BlogPosts.Remove(blogPost);
                await _dbContext.SaveChangesAsync();
                return blogPost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(c=>c.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(Guid id)
        {
            return await _dbContext.BlogPosts.Include(c => c.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetBlogPostByUrlHandleAsync(string urlHandle)
        {
            return await _dbContext.BlogPosts.Include(c => c.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await _dbContext.BlogPosts
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost is null)
            {
                return null;
            }
            //update blogpost
            _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            //update categories
            existingBlogPost.Categories = blogPost.Categories;
            await _dbContext.SaveChangesAsync();

            return blogPost;
        }

    }
}
