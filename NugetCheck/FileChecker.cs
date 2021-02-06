using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using System.Runtime.InteropServices;

namespace NugetCheck
{
    public class FileChecker
    {
        private List<PackageDetails> packages;
        private readonly NugetService _nugetService;
        public FileChecker(NugetService nugetService)
        {
            packages = new List<PackageDetails>();
            _nugetService = nugetService;
        }

        public async Task Execute(string[] filePaths, bool attemptUpdate)
        {
            Console.WriteLine("FileChecker: Execute");
            if (!filePaths.Any())
                return;

            List<ProjectPackages> projects = new List<ProjectPackages>();

            //Loop through each provided file path of a .csproj file
            foreach (string filePath in filePaths)
            {
                //support initialisation of model to maintain details for a single project!
                var projectDetails = initialiseProjectPackage(filePath);
                projects.Add(projectDetails);

                //Now split the file up into its individual lines
                string[] lines = await File.ReadAllLinesAsync(filePath);
                if (lines.Any())
                {
                    foreach (string reference in lines)
                    {
                        //Support checks for target framework
                        if (reference.Contains("TargetFramework"))
                        {
                            String result = checkAndProcessTargetFramework(reference);
                            projectDetails.Framework = result;
                        }

                        if (reference.TrimStart().StartsWith("<PackageReference"))
                        {
                            var package = new PackageDetails();
                            //Package name - to be tidied up!
                            string packageName = tryGetPackageName(reference);
                            //Version - to be tidied up
                            string packageVersion = tryGetPackageVersion(reference);

                            package.UpdatePackageDetails(filePath, packageName, packageVersion);
                            packages.Add(package);
                        }
                    }
                    projectDetails.Packages = packages;

                    if (packages.Any())
                    {
                        foreach (var p in packages)
                        {
                            var nugetQueryResponse = await _nugetService.queryPackageByName(p.Name);
                            if (nugetQueryResponse != null)
                            {
                                p.Response = nugetQueryResponse;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No file content found!");
                }
            }

            PackageComparer comparer = new PackageComparer();
            foreach (var proj in projects)
            {
                //Ok now we need to write a checker
                comparer.tryComparePackagesForProjectAndLogIfOutOfDate(proj);
            }
        }

        /// <summary>
        /// Initialise and create a new project package model
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ProjectPackages initialiseProjectPackage(string filePath)
        {
            var result = new ProjectPackages();
            result.Path = filePath;
            int filePathSlashIndex = 0;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Do something
                filePathSlashIndex = filePath.LastIndexOf("\\");
            }
            else
            {
                //mac OR linux
                filePathSlashIndex = filePath.LastIndexOf("/");
            }

            result.Name = filePath.Substring(filePathSlashIndex).Replace(".csproj", "").Trim();

            return result;
        }

        /// <summary>
        /// Query the content of a project framework version
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string checkAndProcessTargetFramework(string line)
        {
            //Console.WriteLine("checkAndProcessTargetFramework");
            string frameWork = "Unknown";

            int pFrom = line.IndexOf(">") + ">".Length;
            int pTo = line.LastIndexOf("<");

            frameWork = line.Substring(pFrom, pTo - pFrom);
            //Console.WriteLine($"TargetFramework : {result}");

            return frameWork;
        }

        /// <summary>
        /// check the provided line for the included name
        /// </summary>
        /// <param name="line">file line</param>
        /// <returns>the package name</returns>
        private string tryGetPackageName(string line)
        {
            //Console.WriteLine($"tryGetPackageName : {line}");
            int packageIndex = line.IndexOf("Include=") + "Include=".Length;

            string packageName = line.Substring(packageIndex);
            int versionStart = packageName.IndexOf("Version=");
            packageName = packageName.Remove(versionStart);
            packageName = packageName.Replace("\"", "").Trim();

            return packageName;
        }

        /// <summary>
        /// check the provided line for the included package name version
        /// </summary>
        /// <param name="line">file line</param>
        /// <returns>string formatted version number</returns>
        private string tryGetPackageVersion(string line)
        {
            //Console.WriteLine("tryGetPackageVersion");
            int versionIndex = line.IndexOf("Version=") + "Version=".Length;

            string packageVersion = line.Substring(versionIndex);
            packageVersion = packageVersion.Replace("\"", "");
            packageVersion = packageVersion.Replace("/>", "").Trim();

            return packageVersion;
        }
    }
}