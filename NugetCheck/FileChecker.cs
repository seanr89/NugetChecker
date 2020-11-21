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

        public async Task Execute(string filePath)
        {
            Console.WriteLine("FileChecker: Execute");

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
                        //Console.WriteLine(reference);

                        //Package name - to be tidied up!
                        string packageName = tryGetPackageName(reference);
                        //Version - to be tidied up
                        string packageVersion = tryGetPackageVersion(reference);

                        package.UpdatePackageDetails(packageName, packageVersion);
                        packages.Add(package);
                    }
                }

                if (packages.Any())
                {
                    foreach (var p in packages)
                    {
                        //Console.WriteLine(p.ToString());
                        var nugetQueryResponse = await _nugetService.queryPackageByName(p.Name);
                        Console.WriteLine(nugetQueryResponse);
                    }
                }

            }
            else
            {
                Console.WriteLine("No file content found!");
            }
        }

        private string checkAndProcessTargetFramework(string line)
        {
            string frameWork = "Unknown";

            int pFrom = line.IndexOf(">") + ">".Length;
            int pTo = line.LastIndexOf("<");

            frameWork = line.Substring(pFrom, pTo - pFrom);
            //Console.WriteLine($"TargetFramework : {result}");

            return frameWork;
        }

        private string tryGetPackageName(string line)
        {
            int packageIndex = line.IndexOf("Include=") + "Include=".Length;

            string packageName = line.Substring(packageIndex);
            int versionStart = packageName.IndexOf("Version=");
            packageName = packageName.Remove(versionStart);
            packageName = packageName.Replace("\"", "").Trim();

            return packageName;
        }

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