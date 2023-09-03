using BlogProject.DataAccess;
using BlogProject.DataLayer;
using BlogProject.RepoLayer;

namespace BlogProject.Services
{
    public static class BlogServices
    {
        public static IServiceCollection AddBlogServices(this IServiceCollection services)
        {
            services.AddTransient<IBlogDLL, BlogDLL>();
            services.AddTransient<IBlogRepo, BlogRepo>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();

            return services;
        }
    }
}
