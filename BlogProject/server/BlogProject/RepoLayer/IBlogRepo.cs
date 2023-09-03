using BlogProject.Models;

namespace BlogProject.RepoLayer
{
    public interface IBlogRepo
    {
        Task<List<BlogModel>> GetAllPostsWithCategories();
        Task<string>CreateNewPost(BlogModel blogObj);
        Task<BlogModel> GetPostById(int postId);

    }
}
