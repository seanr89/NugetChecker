namespace NugetCheck
{
    public class PackageDetails
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public PackageDetails()
        {
            
        }

        public void UpdatePackageDetails(string name, string version)
        {
            Name = name;
            Version = version;        
        }

        public override string ToString()
        {
            return string.Format("Package Name: {0} with version: {1}", Name, Version);
        }
    }
}