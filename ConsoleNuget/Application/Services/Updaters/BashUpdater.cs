
using System;
using System.Diagnostics;
using Application.Interfaces;

namespace Application.Services.Updaters
{
    public class BashUpdater : IUpdater
    {
        public bool TryExecuteCmd(string packageName, string packageVersion, string folderPath)
        {
            Console.WriteLine($"BashExecutor: TryExecuteCmd {packageName} and version: {packageVersion}");
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
                Console.WriteLine($"BashExecutor InvalidOperationException {ie.Message}");
                return false;
            }
            catch
            {
                Console.WriteLine($"BashExecutor UnHandledException");
                //some exception has been caught
                return false;
            }
        }

        public bool TryExecuteCmdTest()
        {
            Console.WriteLine("BashExecutor: TryExecuteCmd");
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

        public bool TryRestorePackages(string folderPath)
        {
            try
            {
                string command = CreateRestoreCommand();
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
                Console.WriteLine($"BashExecutor InvalidOperationException {ie.Message}");
                return false;
            }
            catch
            {
                Console.WriteLine($"BashExecutor UnHandledException");
                //some exception has been caught
                return false;
            }

        }

        #region private

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string CreateRestoreCommand()
        {
            string cmd = $"dotnet restore & exit";
            return cmd;
        }

        #endregion
    }
}