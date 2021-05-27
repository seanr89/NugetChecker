namespace Application.Services
{
    public class LineReader
    {
        /// <summary>
        /// check the provided line for the included name
        /// </summary>
        /// <param name="line">file line</param>
        /// <returns>the package name</returns>
        public string tryGetPackageName(string line)
        {
            int packageIndex = line.IndexOf("Include=") + "Include=".Length;

            string packageName = line.Substring(packageIndex);
            int versionStart = packageName.IndexOf("Version=");
            packageName = packageName.Remove(versionStart);
            packageName = packageName.Replace("\"", "").Trim();

            return packageName;
        }

        /// <summary>
        /// check the provided line for the included package name version
        /// </summary>
        /// <param name="line">file line</param>
        /// <returns>string formatted version number</returns>
        public string tryGetPackageVersion(string line)
        {
            int versionIndex = line.IndexOf("Version=") + "Version=".Length;

            string packageVersion = line.Substring(versionIndex);
            packageVersion = packageVersion.Replace("\"", "");
            packageVersion = packageVersion.Replace("/>", "").Trim();
            return packageVersion;
        }

        /// <summary>
        /// Query the content of a project framework version
        /// </summary>
        /// <param name="line"></param>
        /// <returns>breakdown of the line content for a targetline record</returns>
        public string checkAndProcessTargetFramework(string line)
        {
            int pFrom = line.IndexOf(">") + ">".Length;
            int pTo = line.LastIndexOf("<");

            return line.Substring(pFrom, pTo - pFrom) ?? string.Empty;
        }
    }
}