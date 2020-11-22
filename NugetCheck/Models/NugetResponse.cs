using System.Collections.Generic;

namespace NugetCheck
{
    public class NugetResponse
    {
        public int totalHits { get; set; }

        public List<NugetPackageData> data { get; set; }
    }

    public class NugetPackageData{
        public string Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] tags { get; set; }
        public bool verified { get; set; }
        public List<PackageVersionInfo> Versions { get; set; }
    }

    public class PackageVersionInfo{
        public string Version { get; set; }
        public string @id { get; set; }
    }
}