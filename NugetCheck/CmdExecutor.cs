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

        public bool TryExecuteCmd(string packageName, string folderPath)
        {
            Console.WriteLine("CmdExecutor: TryExecuteCmd");
            ProcessStartInfo ProcessInfo;
            Process Process;

            string command = "dotnet --version";
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + command);
            //ProcessInfo = new ProcessStartInfo("dotnet --version");
            ProcessInfo.CreateNoWindow = false;
            ProcessInfo.UseShellExecute = true;
            Process = Process.Start(ProcessInfo);

            return false;
        }

        private string CreatePackageCommand(string package, string version)
        {
            string cmd = $"dotnet add package {package} --version {version}";
            return cmd;
        }
    }
}