﻿using BlogProject.Models;
using BlogProject.RepoLayer;

namespace BlogProject.APIs
{
    public static class BlogApi
    {
        public static void RegisterBlogApis(this WebApplication app)
        {
            app.MapGet("/Blogs/GetAllPostsWithCategories", GetAllPostsWithCategories);
            app.MapGet("/Blogs/GetPostById/{postId}", GetPostById);
            app.MapPost("/Blogs/CreateNewPost", CreateNewPost);
            app.MapPost("/Blogs/CreateNewCategory", CreateNewCategory);
            app.MapPost("/Blogs/GetCategoryWithFilter/", GetCategoryWithFilter);
        }
        private static async Task<IResult> GetAllPostsWithCategories(IBlogRepo repo)
        {
            try
            {
                var blog = await repo.GetAllPostsWithCategories();
                return Results.Ok(blog);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
        private static async Task<IResult> CreateNewPost(BlogModel blogObj, IBlogRepo repo)
        {
            try
            {
                var blog = await repo.CreateNewPost(blogObj);
                return Results.Ok(blog);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
        private static async Task<IResult> GetPostById(int postId, IBlogRepo repo)
        {
            try
            {
                var blog = await repo.GetPostById(postId);
                return Results.Ok(blog);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
        private static async Task<IResult> CreateNewCategory(CategoryModel CategoryObj, IBlogRepo repo)
        {
            try
            {
                var category = await repo.CreateNewCategory(CategoryObj);
                return Results.Ok(category);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
        private static async Task<IResult> GetCategoryWithFilter(CategoryModel model, IBlogRepo repo)
        {
            try
            {
                var category = await repo.GetCategoryWithFilter(model);
                return Results.Ok(category);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
