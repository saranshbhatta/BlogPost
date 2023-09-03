using BlogProject.DataAccess;
using BlogProject.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Data;

namespace BlogProject.DataLayer
{
    public class BlogDLL : IBlogDLL
    {
        private readonly ISqlDataAccess _db;
        private readonly ILogger<BlogModel> _logger;

        public BlogDLL(ISqlDataAccess db)
        {
            _db = db;
        }
        public async Task<List<BlogModel>> GetAllPostsWithCategories()
        {
            try
            {
                string SP = @"select blog.get_all_postes();";
                List<BlogModel> blogsList = (await _db.LoadDataRefCursor<BlogModel, dynamic>(SP, new { })).ToList();
                foreach (var item in blogsList)
                {
                    int blog_id = item.BlogId;
                    string SPCategory = @"select blog.get_category_list(@blog_id)";
                    string SPIndg = @"select blog.get_ingredient_list(@blog_id)";
                    item.CategoriesList = (await _db.LoadDataRefCursor<CategoryModel, dynamic>(SPCategory, new { blog_id })).ToList();
                    item.IngredientsList = (await _db.LoadDataRefCursor<IngredientsModel, dynamic>(SPIndg, new { blog_id })).ToList();
                }
                return blogsList;
            }
            catch (Exception e)
            {
                _logger.LogError("BlogDLL:GettingBlog : Error:{0}", e.Message);
                throw new Exception("Error while Getting Blog Lists: Error: " + e.Message);
            }
        }

        public async Task<string> CreateNewPost(BlogModel blogObj)
        {
            using (IDbTransaction tran = _db.BeginTransaction())
            {
                try
                {
                    string SP = @"select blog.create_new_post(@BlogTitle,@BlogDescription,@PrepTime,@CookTime,@IsVegan,@IsVeg,@MinimumServingSize,@EntryBy)";
                    int blogId = await _db.ExecuteScalarAsync<BlogModel, dynamic>(SP, blogObj, transaction: tran);
                    foreach (var item in blogObj.CategoriesList)
                    {
                        string SPCategory = @"select blog.add_new_category(@blogId, @CategoryId)";
                        var parameters = new { blogId, CategoryId = item.Id };
                        int categoryId = await _db.ExecuteScalarAsync<dynamic, dynamic>(SPCategory, parameters, transaction: tran);
                    }
                    foreach (var item in blogObj.IngredientsList)
                    {
                        //how indg should be added should be final first so that lookup table should be updated or not
                        string SPIndg = @"select blog.add_indg_main(@IndgId , @IndgQuantity , @blogId)";
                        var parameters = new { blogId, IndgId = item.Id, IndgQuantity= item.IndgQuantity };
                        int indgId = await _db.ExecuteScalarAsync<dynamic, dynamic>(SPIndg, parameters, transaction: tran);
                    }
                    tran.Commit();
                    return blogId.ToString();
                }
                catch (Exception e)
                {
                    _logger.LogError("BudgetDLL:SavingBudget : Error:{0}", e.Message);
                    throw new Exception("Error while saving Budget: Error: " + e.Message);
                }
            }
        }

        public async Task<BlogModel> GetPostById(int postId)
        {
            try
            {
                string SP = @"select blog.get_post_by_id(@postId);";
                BlogModel blog = (await _db.LoadDataRefCursor<BlogModel, dynamic>(SP, new { postId })).FirstOrDefault();
                if (blog!= null)
                {
                    string SPCategory = @"select blog.get_category_list(@postId)";
                    string SPIndg = @"select blog.get_ingredient_list(@postId)";
                    blog.CategoriesList = (await _db.LoadDataRefCursor<CategoryModel, dynamic>(SPCategory, new { postId })).ToList();
                    blog.IngredientsList = (await _db.LoadDataRefCursor<IngredientsModel, dynamic>(SPIndg, new { postId })).ToList();
                }
                else
                {
                    return null;
                }
                return blog;
            }
            catch (Exception e)
            {
                _logger.LogError("BlogDLL:GettingBlog : Error:{0}", e.Message);
                throw new Exception("Error while Getting Blog Lists: Error: " + e.Message);
            }
        }

    }
}

