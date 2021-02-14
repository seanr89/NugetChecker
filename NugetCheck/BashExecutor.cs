using System;
using System.Diagnostics;
using NugetCheck.Interfaces;

namespace NugetCheck
{
    public class BashExecutor : INugetExecutor
    {
        public bool TryExecuteCmdTest()
        {
            Console.WriteLine("MacExecutor: TryExecuteCmd");
            string command = "dotnet --version";
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = @"/bin/bash";
            proc.StartInfo.Arguments = "-c \" " + command + " \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                Console.WriteLine(proc.StandardOutput.ReadLine());
            }

            return true;
        }

        public bool TryExecuteCmd(string packageName, string packageVersion, string folderPath)
        {
            Console.WriteLine($"MacExecutor: TryExecuteCmd {packageName} and version: {packageVersion}");
            try
            {
                string command = CreatePackageCommand(packageName, packageVersion);
                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = @"/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                while (!proc.StandardOutput.EndOfStream)
                {
                    Console.WriteLine(proc.StandardOutput.ReadLine());
                }
                return true;
            }
            catch (InvalidOperationException ie)
            {
                return false;
            }
            catch
            {
                //some exeception has been caught
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string TrimPathToFolderOnly(string path)
        {
            return path.Substring(0, path.LastIndexOf("\\"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private string CreatePackageCommand(string package, string version)
        {
            string cmd = $"dotnet add package {package} -v {version} & exit";
            return cmd;
        }
    }
}