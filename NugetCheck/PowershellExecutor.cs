using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
namespace NugetCheck
{
    public class PowershellExecutor
    {
        //https://gist.github.com/miteshsureja/f9cbc2f09264a01277a6555a7425debc
        public bool TryExecutePowershellCmd(string packageName, string folderPath)
        {
            Console.WriteLine("PowershellExecutor: TryExecutePowershellCmd");
            try
            {
                using (PowerShell PowerShellInst = PowerShell.Create())
                {
                    string criteria = "system*";
                    string cmd = $"Get-Service -DisplayName {criteria}";

                    PowerShellInst.AddScript(cmd);
                    Collection<PSObject> PSOutput = PowerShellInst.Invoke();
                    foreach (PSObject obj in PSOutput)
                    {
                        if (obj != null)
                        {
                            Console.Write(obj.Properties["Status"].Value.ToString() + " - ");
                            Console.WriteLine(obj.Properties["DisplayName"].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"TryExecutePowershellCmd exception: {e.Message}");
            }

            return false;
        }
    }
}