namespace Domain
{
    public class PackageInfo
    {
        public string Name { get; set; }

        public string CurrentVersion { get; set; }
        public NugetResponse Response { get; set; }
    }
}