namespace Domain
{
    /// <summary>
    /// package details, both current and latest version!
    /// </summary>
    public class PackageInfo
    {
        public string Name { get; set; }

        public string CurrentVersion { get; set; }
        public NugetResponse Response { get; set; }
    }
}