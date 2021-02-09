using System.Collections.Generic;

namespace NugetCheck
{
    /// <summary>
    /// A breakdown of a project, its folder location and package details!
    /// </summary>
    public class ProjectPackages
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Framework { get; set; }
        public List<PackageDetails> Packages { get; set; } = new List<PackageDetails>();
    }
}