using System.Collections.Generic;

namespace Domain
{
    public class NugetResponse
    {
        //public int totalHits { get; set; }

        public List<NugetPackageData> data { get; set; }
    }

    /// <summary>
    /// Detailed nuget response data that was queried
    /// </summary>
    public struct NugetPackageData
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] tags { get; set; }
        public bool verified { get; set; }
        public List<PackageVersionInfo> Versions { get; set; }
    }

    /// <summary>
    /// Information for available versions from Nuget for a particular package
    /// </summary>
    public struct PackageVersionInfo
    {
        public string Version { get; set; }
        public string @id { get; set; }
    }
}