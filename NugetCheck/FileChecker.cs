using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;

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

        public async Task Execute(string[] filePaths)
        {
            Console.WriteLine("FileChecker: Execute");
            List<ProjectPackages> projects = new List<ProjectPackages>();

            foreach (string filePath in filePaths)
            {
                //new logic for multiple project details handled!
                var projectDetails = new ProjectPackages();
                projectDetails.Path = filePath;
                //var filePathSlashIndex = filePath.LastIndexOf("\\");
                //mac/linux
                var filePathSlashIndex = filePath.LastIndexOf("/");
                projectDetails.Name = filePath.Substring(filePathSlashIndex).Replace(".csproj", "").Trim();
                projects.Add(projectDetails);

                string[] lines = await File.ReadAllLinesAsync(filePath);

                if (lines.Any())
                {
                    foreach (string reference in lines)
                    {
                        if (reference.Contains("TargetFramework"))
                        {
                            String result = checkAndProcessTargetFramework(reference);
                            Console.WriteLine($"TargetFramework : {result}");
                        }

                        if (reference.Contains("PackageReference"))
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

                        //Ok now we need to write a checker
                        PackageChecker checker = new PackageChecker();
                        checker.CheckEachPackage(packages);
                    }

                }
                else
                {
                    Console.WriteLine("No file content found!");
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string checkAndProcessTargetFramework(string line)
        {
            string frameWork = "Unknown";

            int pFrom = line.IndexOf(">") + ">".Length;
            int pTo = line.LastIndexOf("<");

            frameWork = line.Substring(pFrom, pTo - pFrom);
            //Console.WriteLine($"TargetFramework : {result}");

            return frameWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string tryGetPackageName(string line)
        {
            int packageIndex = line.IndexOf("Include=") + "Include=".Length;

            string packageName = line.Substring(packageIndex);
            int versionStart = packageName.IndexOf("Version=");
            packageName = packageName.Remove(versionStart);
            packageName = packageName.Replace("\"", "").Trim();

            return packageName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string tryGetPackageVersion(string line)
        {
            //Version - to be tidied up
            int versionIndex = line.IndexOf("Version=") + "Version=".Length;

            string packageVersion = line.Substring(versionIndex);
            packageVersion = packageVersion.Replace("\"", "");
            packageVersion = packageVersion.Replace("/>", "").Trim();

            return packageVersion;
        }
    }
}