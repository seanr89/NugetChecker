using System.Collections.Generic;
namespace Domain
{
    public class ProjectDetails
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Framework { get; set; }
        public List<PackageInfo> Packages { get; set; } = new List<PackageInfo>();

        public ProjectDetails(string path)
        {
            Path = path;
        }
    }
}