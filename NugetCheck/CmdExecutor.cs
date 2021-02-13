using System;
using System.Diagnostics;
using NugetCheck.Interfaces;

namespace NugetCheck
{
    public class CmdExecutor : INugetExecutor
    {
        public bool TryExecuteCmdTest()
        {
            Console.WriteLine("CmdExecutor: TryExecuteCmd");
            ProcessStartInfo ProcessInfo;
            Process Process;

            string command = "dotnet --version";
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + command);
            ProcessInfo.CreateNoWindow = false;
            ProcessInfo.UseShellExecute = true;
            Process = Process.Start(ProcessInfo);

            return false;
        }

        public bool TryExecuteCmd(string packageName, string packageVersion, string folderPath)
        {
            Console.WriteLine($"CmdExecutor: TryExecuteCmd {packageName} and version: {packageVersion}");
            try
            {
                ProcessStartInfo ProcessInfo;
                Process Process = new Process();
                //Set a time-out value.
                int timeOut = 10000;

                string command = CreatePackageCommand(packageName, packageVersion);
                ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + command);
                var path = TrimPathToFolderOnly(folderPath);
                ProcessInfo.WorkingDirectory = TrimPathToFolderOnly(folderPath);
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = true;
                Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //TODO - handle process response and potential cmd closure!
                Process.Start(ProcessInfo);
                //Wait for the window to finish loading.
                Process.WaitForInputIdle();
                //Added a step to wait for an exit
                Process.WaitForExit(timeOut);

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