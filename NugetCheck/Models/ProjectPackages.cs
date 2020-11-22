using System.Collections.Generic;

namespace NugetCheck
{
    public class ProjectPackages
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public List<PackageDetails> Packages { get; set; } = new List<PackageDetails>();
    }
}