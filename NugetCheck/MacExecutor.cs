using System;
using System.Diagnostics;

namespace NugetCheck
{
    public class MacExecutor
    {
        public bool TryExecuteCmdTest()
        {
            Console.WriteLine("MacExecutor: TryExecuteCmd");

            return false;
        }

        public bool TryExecuteCmd(string packageName, string packageVersion, string folderPath)
        {
            Console.WriteLine($"MacExecutor: TryExecuteCmd {packageName} and version: {packageVersion}");
            try
            {
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