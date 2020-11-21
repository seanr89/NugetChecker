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
        public FileChecker()
        {
            packages = new List<PackageDetails>();
        }

        public async Task Execute(string filePath)
        {
            Console.WriteLine("FileChecker: Execute");

            string[] lines = await File.ReadAllLinesAsync(filePath);

            if(lines.Any())
            {
                foreach (string reference in lines)
                {
                    if(reference.Contains("TargetFramework")){
                        int pFrom = reference.IndexOf(">") + ">".Length;
                        int pTo = reference.LastIndexOf("<");

                        String result = reference.Substring(pFrom, pTo - pFrom);
                        Console.WriteLine($"TargetFramework : {result}");
                    }
                    if(reference.Contains("PackageReference"))
                    {
                        var package = new PackageDetails();
                        //Console.WriteLine(reference);

                        int pacakgeIndex = reference.IndexOf("Include=") + "Include=".Length;
                        
                        string packageName = reference.Substring(pacakgeIndex);
                        int versionStart = packageName.IndexOf("Version=");
                        packageName = packageName.Remove(versionStart);
                        packageName = packageName.Replace("\"", "").Trim();
                        //Console.WriteLine($"packageName = {packageName}");

                        //Version - to be tidied
                        int versionIndex = reference.IndexOf("Version=") + "Version=".Length;
                        //Console.WriteLine($"indexVersion: {versionIndex}");

                        string packageVersion = reference.Substring(versionIndex);
                        packageVersion = packageVersion.Replace("\"", "");
                        packageVersion = packageVersion.Replace("/>", "").Trim();
                        //Console.WriteLine($"version = {packageVersion}");
                        package.UpdatePackageDetails(packageName, packageVersion);
                        packages.Add(package);
                    }
                }

                if(packages.Any())
                {
                    foreach(var p in packages)
                    {
                        Console.WriteLine(p.ToString());
                    }
                }

            }else{
                Console.WriteLine("no project found");
            }

        }
    }
}