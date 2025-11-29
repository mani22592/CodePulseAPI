using Azure;
using CodePulse.Models.Domain;
using CodePulse.Models.DTO;
using CodePulse.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace CodePulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,ICategoryRepository categoryRepository)
        {
            this._blogPostRepository = blogPostRepository;
            this._categoryRepository = categoryRepository;
        }

        //POST: {apibaseUrl}/api/BlogPosts/CreateBlogPost
        [HttpPost]
        [Route("CreateBlogPost")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto requestDto)
        {
            //convert Dto to Domain model
            var blogpost = new BlogPost
            {
                Title = requestDto.Title,
                Author = requestDto.Author,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                ShortDescription = requestDto.ShortDescription,
                UrlHandle = requestDto.UrlHandle,
                PublishedDate = requestDto.PublishedDate,
                IsVisible = requestDto.IsVisible,
                Categories = new List<Category>()
            };

            foreach(var categoryGuid in requestDto.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(categoryGuid);
                if (existingCategory is not null)
                {
                    blogpost.Categories.Add(existingCategory);
                }
            }

            blogpost = await _blogPostRepository.CreateAsync(blogpost);

            //convert domain model to Dto

            var response = new BlogPostDto
            {
                Author = blogpost.Author,
                Content = blogpost.Content,
                UrlHandle = blogpost.UrlHandle,
                PublishedDate = blogpost.PublishedDate,
                IsVisible = blogpost.IsVisible,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                ShortDescription = blogpost.ShortDescription,
                Title = blogpost.Title,
                Id= blogpost.Id,
                Categories =blogpost.Categories.Select(c=> new CategoryDto { 
                    Id=c.Id,
                    Name=c.Name,
                    UrlHandle=c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        //GET: {apibaseUrl}/api/BlogPosts/GetAllBlogPosts
        [HttpGet]
        [Route("GetAllBlogPosts")]
        public async Task<IActionResult> GetAllBlogPostsAsync()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();

            //convert domain model to Dto

            var response = new List<BlogPostDto>();

            foreach(var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    ShortDescription = blogPost.ShortDescription,
                    IsVisible = blogPost.IsVisible,
                    Categories = blogPost.Categories.Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList()
                });
            }

            return Ok(response);
        }

        //GET: {apibaseUrl}/api/BlogPosts/GetBlogPostById/{id}
        [HttpGet]
        [Route("GetBlogPostById/{id:guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await _blogPostRepository.GetBlogPostByIdAsync(id);
            if(blogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                ShortDescription = blogPost.ShortDescription,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        //GET:{apibaseUrl}/api/BlogPosts/GetBlogPostByUrlHandle/{urlHandle}
        [HttpGet]
        [Route("GetBlogPostByUrlHandle/{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await _blogPostRepository.GetBlogPostByUrlHandleAsync(urlHandle);
            if (blogPost is null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                ShortDescription = blogPost.ShortDescription,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };
            return Ok(response);
        }

        //PUT: {apibaseUrl}/api/BlogPosts/UpdateBlogPostById/{id}
        [HttpPut]
        [Route("UpdateBlogPostById/{id:guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto requestDto)
        {
            //convert Dto to Domain model
            var blogpost = new BlogPost
            {
                Id = id,
                Title = requestDto.Title,
                Author = requestDto.Author,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                ShortDescription = requestDto.ShortDescription,
                UrlHandle = requestDto.UrlHandle,
                PublishedDate = requestDto.PublishedDate,
                IsVisible = requestDto.IsVisible,
                Categories = new List<Category>()
            };

            //foreach
            foreach (var categoryGuid in requestDto.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(categoryGuid);
                if (existingCategory is not null)
                {
                    blogpost.Categories.Add(existingCategory);
                }
            }

            //call repository to update blogpost domain model
            var updatedBlogPost = await _blogPostRepository.UpdateAsync(blogpost);
            if (updatedBlogPost is null)
            {
                return NotFound();
            }

            //convert domain model to Dto
            var response = new BlogPostDto
            {
                Id = blogpost.Id,
                Title = blogpost.Title,
                Author = blogpost.Author,
                Content = blogpost.Content,
                UrlHandle = blogpost.UrlHandle,
                PublishedDate = blogpost.PublishedDate,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                ShortDescription = blogpost.ShortDescription,
                IsVisible = blogpost.IsVisible,
                Categories = blogpost.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    UrlHandle = c.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        //DELETE: {apibaseUrl}/api/BlogPosts/DeleteBlogPostById/{id}
        [HttpDelete]
        [Route("DeleteBlogPostById/{id:guid}")]
        public async Task<IActionResult> DeleteBlogPostById([FromRoute] Guid id)
        {
           var blogPost = await _blogPostRepository.DeleteBlogPostById(id);

            if (blogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                ShortDescription = blogPost.ShortDescription,
                IsVisible = blogPost.IsVisible
            };

            return Ok(response);
        }
    }
}
