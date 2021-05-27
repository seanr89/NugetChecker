using System.Collections.Generic;
namespace Domain
{
    /// <summary>
    /// Breakdown of project information that has been scanned!
    /// </summary>
    public class ProjectDetails
    {
        /// <summary>
        /// pathway to the project/csproj
        /// </summary>
        /// <value></value>
        public string Path { get; set; }
        /// <summary>
        /// the name pulled from the csproj file
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// the .net/core framework version
        /// </summary>
        /// <value></value>
        public string Framework { get; set; }
        /// <summary>
        /// the location external/nuget package references found!
        /// </summary>
        /// <typeparam name="PackageInfo"></typeparam>
        /// <returns></returns>
        public List<PackageInfo> Packages { get; set; } = new List<PackageInfo>();

        public ProjectDetails(string path)
        {
            Path = path;
        }
    }
}