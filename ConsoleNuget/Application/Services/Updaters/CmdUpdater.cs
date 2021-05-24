using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Services.Updaters
{
    public class CmdUpdater : IUpdater
    {
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
                Console.WriteLine($"CmdExecutor InvalidOperationException {ie.Message}");
                return false;
            }
            catch
            {
                Console.WriteLine($"CmdExecutor UnHandledException");
                return false;
            }
        }

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



        public bool TryRestorePackages(string folderPath)
        {
            //Console.WriteLine($"CmdExecutor: TryExecuteCmd {packageName} and version: {packageVersion}");
            try
            {
                using (Process Process = new Process())
                {
                    ProcessStartInfo ProcessInfo;
                    //Set a time-out value.
                    ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + "dotnet restore & exit");
                    var path = TrimPathToFolderOnly(folderPath);
                    ProcessInfo.WorkingDirectory = TrimPathToFolderOnly(folderPath);
                    ProcessInfo.CreateNoWindow = true;
                    ProcessInfo.UseShellExecute = true;
                    Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.StartInfo.RedirectStandardInput = true;
                    Process.StartInfo.RedirectStandardOutput = true;
                    Process.StartInfo.RedirectStandardError = true;
                    //TODO - handle process response and potential cmd closure!
                    // Allows to raise event when the process is finished
                    Process.EnableRaisingEvents = true;
                    // Eventhandler wich fires when exited
                    Process.Exited += new EventHandler(restoreProcess_Exited);
                    var pro = Process.Start(ProcessInfo);
                    //Wait for the window to finish loading.
                    //pro.WaitForInputIdle();
                    pro.WaitForExit(2000);
                    // if (!Process.HasExited)
                    // {
                    //     int exitCode = Process.ExitCode;
                    //     Process.Close();
                    // }
                    return true;
                }
            }
            catch (InvalidOperationException ie)
            {
                Console.WriteLine($"Restore InvalidOperationException {ie.Message}");
                return false;
            }
            catch
            {
                Console.WriteLine($"Restore UnHandledException");
                return false;
            }
        }

        #region private

        /// <summary>
        /// Support a quick / trimming step
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string TrimPathToFolderOnly(string path)
        {
            return path.Substring(0, path.LastIndexOf("\\"));
        }

        /// <summary>
        /// Create the dotnet add command that will update the package!
        /// </summary>
        /// <param name="package"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private string CreatePackageCommand(string package, string version)
        {
            string cmd = $"dotnet add package {package} -v {version} & exit";
            return cmd;
        }

        #endregion

        #region Events

        private void restoreProcess_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine("restoreProcess_Exited");
        }

        private static Task<bool> WaitForExitAsync(Process process, int timeout)
        {
            return Task.Run(() => process.WaitForExit(timeout));
        }

        #endregion
    }
}