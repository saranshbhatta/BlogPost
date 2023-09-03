using BlogProject.DataLayer;
using BlogProject.Models;

namespace BlogProject.RepoLayer
{
    public class BlogRepo : IBlogRepo
    {
        private readonly IBlogDLL _dll;
        public BlogRepo(IBlogDLL dll)
        {
            _dll = dll;
        }
        public async Task<List<BlogModel>> GetAllPostsWithCategories()
        {
            try
            {
                List<BlogModel> blogs = await _dll.GetAllPostsWithCategories();
                return blogs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<string> CreateNewPost(BlogModel blogObj)
        {
            try
            {
                string blogs = await _dll.CreateNewPost(blogObj);
                return blogs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<BlogModel> GetPostById(int postId)
        {
            try
            {
                BlogModel blogs = await _dll.GetPostById(postId);
                return blogs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
