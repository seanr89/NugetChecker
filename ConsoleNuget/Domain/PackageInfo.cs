namespace Domain
{
    /// <summary>
    /// Project Nuget package details - both current and latest version!
    /// </summary>
    public class PackageInfo
    {
        /// <summary>
        /// Namme of the package to search
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// Parsed value for the current version in use
        /// </summary>
        /// <value></value>
        public string CurrentVersion { get; set; }
        public NugetResponse Response { get; set; }
        /// <summary>
        /// Flag to to denote if the package was updated or not!
        /// 
        /// </summary>
        /// <value></value>
        public bool Updated { get; set; } = false;

        /// <summary>
        /// TODO: rework to support easier logging/outputting of relevant data!
        /// </summary>
        /// <returns></returns>

        public override string ToString()
        {
            return base.ToString();
        }
    }
}