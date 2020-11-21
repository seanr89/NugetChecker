using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NugetCheck
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
    /// </summary>
    public class NugetService
    {
        private readonly HttpClient _httpClient;
        private const string _ServiceIndex = "https://azuresearch-usnc.nuget.org/query?q=packageid:";
        //private const string _ServiceIndex = "https://www.nuget.org/packages/";
        public NugetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> queryPackageByName(string packageName){

            var url = _ServiceIndex+packageName;
            Console.WriteLine("queryPackageByName: " + url);
            var response = await _httpClient.GetStringAsync(_ServiceIndex+packageName);
            return response;
        }
    }
}