using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace NugetCheck
{
    public class PackageComparer
    {
        /// <summary>
        /// New process to support querying for a single project
        /// </summary>
        /// <param name="project">the project to compare and output</param>
        public void tryComparePackagesForProjectAndLogIfOutOfDate(ProjectPackages project, bool update = false)
        {
            if (!project.Packages.Any())
                return;

            CmdExecutor updater = null;

            if (update && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                updater = new CmdExecutor();
            }

            Console.WriteLine($"Project: {project.Name}\n");

            foreach (var package in project.Packages)
            {
                var latestPackage = package.Response.data[0];
                if (package.Version != latestPackage.Version)
                {
                    //package is not on latest version!
                    Console.WriteLine($"Package: {package.Name} can be updated to version : {latestPackage.Version}");
                    if (update)
                    {
                        bool updated = updater.TryExecuteCmd(package.Name, latestPackage.Version, project.Path);
                        if (updated)
                            Console.WriteLine($"Updated package {package.Name}");
                    }
                }
            }
            Console.WriteLine("-------------");
        }
    }
}