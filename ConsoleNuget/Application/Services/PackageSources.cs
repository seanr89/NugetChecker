

using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Application.Services
{
    //https://stackoverflow.com/questions/60767909/c-sharp-console-app-how-do-i-make-an-interactive-menu
    public class PackageSources
    {
        private readonly List<NugetSource> _sources;
        public PackageSources()
        {
            _sources = new List<NugetSource>(){
                new NugetSource() {Name = "Nuget", URL = "https://azuresearch-usnc.nuget.org/query?q=packageid:"},
                new NugetSource() {Name = "Randox", URL = ""},
                new NugetSource() {Name = "Telerik", URL = ""}
            };
        }

        public NugetSource? GetPackageSourceByName(string name)
        {
            return _sources.FirstOrDefault(n => n.Name.ToUpper().Trim() == name.ToUpper().Trim());
        }
    }
}