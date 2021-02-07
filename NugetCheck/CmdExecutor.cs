using System;
using System.Diagnostics;

namespace NugetCheck
{
    public class CmdExecutor
    {
        public bool TryExecuteCmdTest()
        {
            Console.WriteLine("CmdExecutor: TryExecuteCmd");
            ProcessStartInfo ProcessInfo;
            Process Process;

            string command = "dotnet --version";
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + command);
            //processStartInfo.WorkingDirectory = @"c:\\Windows\\Downloaded Program Files";
            //ProcessInfo = new ProcessStartInfo("dotnet --version");
            ProcessInfo.CreateNoWindow = false;
            ProcessInfo.UseShellExecute = true;
            Process = Process.Start(ProcessInfo);

            return false;
        }

        public bool TryExecuteCmd(string packageName, string packageVersion, string folderPath)
        {
            Console.WriteLine($"CmdExecutor: TryExecuteCmd {packageName} and version: {packageVersion}");
            ProcessStartInfo ProcessInfo;
            Process Process = new Process();

            string command = CreatePackageCommand(packageName, packageVersion);
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + command);
            var path = TrimPathToFolderOnly(folderPath);
            ProcessInfo.WorkingDirectory = TrimPathToFolderOnly(folderPath);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;
            Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //TODO - handle process response and potential cmd closure!
            Process.Start(ProcessInfo);

            return false;
        }

        private string TrimPathToFolderOnly(string path)
        {
            return path.Substring(0, path.LastIndexOf("\\"));
        }

        private string CreatePackageCommand(string package, string version)
        {
            string cmd = $"dotnet add package {package} -v {version}";
            return cmd;
        }
    }
}