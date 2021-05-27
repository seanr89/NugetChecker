using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        // private readonly List<NugetSource> _nugetSources;

        public NugetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task queryPackagesForProject(ProjectDetails proj)
        {
            foreach (var pack in proj.Packages)
            {
                var nugetValue = await queryIndividualPackageDetails(pack);
                pack.Response = nugetValue;
            }
        }

        [return: MaybeNull]
        public async Task<NugetResponse?> queryIndividualPackageDetails(PackageInfo package)
        {
            try
            {
                var url = _ServiceIndex + package.Name;
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    //TODO: Check for authentication or connection issues here too!
                    return null;
                }
                else
                {
                    String urlContents = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<NugetResponse>(urlContents);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"NugetService Exception caught: {e.Message}");
                return null;
            }
        }
    }
}