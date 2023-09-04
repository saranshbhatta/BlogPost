using BlogProject.Models;

namespace BlogProject.DataLayer
{
    public interface IBlogDLL
    {
        Task<List<BlogModel>> GetAllPostsWithCategories();
        Task<string> CreateNewPost(BlogModel blogObj);
        Task<BlogModel> GetPostById(int postId);
        Task<string> CreateNewCategory(CategoryModel CategoryObj);
        Task<List<CategoryModel>> GetCategoryWithFilter(CategoryModel model);



    }
}
