using BlogProject.Models;

namespace BlogProject.DataLayer
{
    public interface IBlogDLL
    {
        Task<List<BlogModel>> GetAllPostsWithCategories();
        Task<string> CreateNewPost(BlogModel blogObj);
        Task<BlogModel> GetPostById(int postId);


    }
}
