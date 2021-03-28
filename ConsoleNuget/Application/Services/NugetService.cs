using System.Net.Http;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;

namespace Application.Services
{
    public class NugetService : INugetService
    {
        private readonly HttpClient _httpClient;
        public NugetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task queryPackagesForProject(ProjectDetails proj)
        {

        }
    }
}