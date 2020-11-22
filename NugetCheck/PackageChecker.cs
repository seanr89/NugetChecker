using System;
using System.Collections.Generic;
using System.Linq;

namespace NugetCheck
{
    public class PackageChecker
    {
        public PackageChecker()
        {
            
        }

        /// <summary>
        /// Method to compare packages and output there status!
        /// </summary>
        /// <param name="packages">collection of packages</param>
        public void CheckEachPackage(List<PackageDetails> packages)
        {
            if(!packages.Any())
                return;

            foreach(var package in packages)
            {
                var latestPackage = package.Response.data[0];
                if(package.Version != latestPackage.Version)
                {
                    //package is not on latest!
                    Console.WriteLine($"Package: {package.Name} can be updated to version : {latestPackage.Version}");
                }
                else{
                    Console.WriteLine($"Package: {package.Name} is up to date!");
                }
            }
        }
    }
}