using System.Collections.Generic;
using System.Linq;

namespace NugetCheck
{
    public class PackageChecker
    {
        public PackageChecker()
        {
            
        }

        public void CheckEachPackage(List<PackageDetails> packages)
        {
            if(!packages.Any())
                return;

            foreach(var package in packages)
            {
                
            }
        }
    }
}