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
            if (!packages.Any())
                return;

            foreach (var package in packages)
            {
                var latestPackage = package.Response.data[0];
                if (package.Version != latestPackage.Version)
                {
                    //package is not on latest!
                    Console.WriteLine($"Package: {package.Name} can be updated to version : {latestPackage.Version}");
                }
                else
                {
                    Console.WriteLine($"Package: {package.Name} is up to date!");
                }
            }
        }

        /// <summary>
        /// New process to support querying for a single project
        /// </summary>
        /// <param name="project">the project to compare and output</param>
        public void CheckPackagesForProject(ProjectPackages project)
        {
            if (!project.Packages.Any())
                return;

            Console.WriteLine($"Project: {project.Name}\n");

            foreach (var package in project.Packages)
            {
                var latestPackage = package.Response.data[0];
                if (package.Version != latestPackage.Version)
                {
                    //package is not on latest!
                    Console.WriteLine($"Package: {package.Name} can be updated to version : {latestPackage.Version}");
                }
                // else
                // {
                //     Console.WriteLine($"Package: {package.Name} is up to date!");
                // }
            }
            Console.WriteLine("-------------");
        }
    }
}