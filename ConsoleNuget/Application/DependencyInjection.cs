using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<FolderSearcher>();
            services.AddTransient<IFileHandler, FileHandler>();
            services.AddTransient<INugetService, NugetService>();
            services.AddTransient<LineReader>();

            //Add http client services at ConfigureServices(IServiceCollection services)
            services.AddHttpClient<INugetService, NugetService>();
            services.AddTransient<UpdaterFactory>();
            return services;
        }
    }
}