using System.Collections.Generic;

namespace NugetCheck.Models
{
    public class ProjectPackages
    {
        public string ProjectPath { get; set; }
        public List<PackageDetails> Packages { get; set; }
    }
}