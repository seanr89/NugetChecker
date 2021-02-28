using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<FolderSearcher>();
            services.AddTransient<IFileHandler, FileHandler>();
            return services;
        }
    }
}