using System;
using System.Collections.Generic;
using System.Linq;

namespace NugetCheck
{
    public class PackageComparer
    {
        /// <summary>
        /// New process to support querying for a single project
        /// </summary>
        /// <param name="project">the project to compare and output</param>
        public void tryComparePackagesForProjectAndLogIfOutOfDate(ProjectPackages project)
        {
            if (!project.Packages.Any())
                return;

            Console.WriteLine($"Project: {project.Name}\n");

            foreach (var package in project.Packages)
            {
                var latestPackage = package.Response.data[0];
                if (package.Version != latestPackage.Version)
                {
                    //package is not on latest version!
                    Console.WriteLine($"Package: {package.Name} can be updated to version : {latestPackage.Version}");
                }
            }
            Console.WriteLine("-------------");
        }
    }
}