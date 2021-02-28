using System.Collections.Generic;

namespace Domain
{
    public class ProjectDetails
    {
        public string Name { get; set; }
        public List<PackageInfo> Packages { get; set; }
    }
}