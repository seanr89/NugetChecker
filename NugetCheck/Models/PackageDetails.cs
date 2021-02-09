namespace NugetCheck
{
    public class PackageDetails
    {
        public string ProjectPath { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public NugetResponse Response { get; set; }

        public void UpdatePackageDetails(string projectPath, string name, string version)
        {
            ProjectPath = projectPath;
            Name = name;
            Version = version;
        }

        public override string ToString()
        {
            return string.Format("Package Name: {0} with version: {1}", Name, Version);
        }
    }
}