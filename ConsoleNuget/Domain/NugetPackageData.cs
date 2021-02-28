namespace Domain
{
    public class NugetPackageData
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] tags { get; set; }
        public bool verified { get; set; }
    }
}