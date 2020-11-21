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
        public FileChecker()
        {
            
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
                    //Console.WriteLine(reference);

                    if(reference.Contains("PackageReference"))
                    {
                        int pacakgeIndex = reference.IndexOf("Include=") + "Include=".Length;
                        
                        string packageName = reference.Substring(pacakgeIndex);
                        int versionStart = packageName.IndexOf("Version=");
                        packageName = packageName.Remove(versionStart);
                        packageName = packageName.Replace("\"", "").Trim();
                        Console.WriteLine($"packageName = {packageName}");

                        //Version - to be tidied
                        int versionIndex = reference.IndexOf("Version=") + "Version=".Length;
                        //Console.WriteLine($"indexVersion: {versionIndex}");

                        string sub = reference.Substring(versionIndex);
                        sub = sub.Replace("\"", "");
                        sub = sub.Replace("/>", "").Trim();
                        Console.WriteLine($"version = {sub}");
                    }
                }
            }else{
                Console.WriteLine("no project found");
            }

        }
    }
}