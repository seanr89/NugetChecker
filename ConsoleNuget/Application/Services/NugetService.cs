using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Newtonsoft.Json;

namespace Application.Services
{
    public class NugetService : INugetService
    {
        private readonly HttpClient _httpClient;
        //Basic nuget search URL
        private const string _ServiceIndex = "https://azuresearch-usnc.nuget.org/query?q=packageid:";
        public NugetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task queryNugetForPackage(PackageInfo pack)
        {
            throw new System.NotImplementedException();
        }

        public async Task<NugetResponse> queryPackagesForProject(ProjectDetails proj)
        {
            try
            {
                //var url = this._ServiceIndex += proj.Name;
                HttpResponseMessage response = await _httpClient.GetAsync(_ServiceIndex + proj.Name);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                string urlContents = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<NugetResponse>(urlContents);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Generic Exception caught: {e.Message}");
                return;
            }
        }
    }
}