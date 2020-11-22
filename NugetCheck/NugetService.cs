using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NugetCheck
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
    /// </summary>
    public class NugetService
    {
        private readonly HttpClient _httpClient;
        private const string _ServiceIndex = "https://azuresearch-usnc.nuget.org/query?q=packageid:";

        public NugetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<NugetResponse> queryPackageByName(string packageName)
        {

            var url = _ServiceIndex + packageName;
            //Console.WriteLine("queryPackageByName url: " + url);
            //var response = await _httpClient.GetStringAsync(_ServiceIndex+packageName);
            HttpResponseMessage response = await _httpClient.GetAsync(_ServiceIndex + packageName);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
            }
            String urlContents = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<NugetResponse>(urlContents);
            return jsonObject;
        }
    }
}