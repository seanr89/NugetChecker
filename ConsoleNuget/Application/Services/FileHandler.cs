using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class FileHandler : IFileHandler
    {
        private readonly ILogger<FileHandler> _logger;
        private readonly LineReader _lineReader;
        public FileHandler(ILogger<FileHandler> logger, LineReader lineReader)
        {
            _logger = logger;
            _lineReader = lineReader;
        }

        public async Task<ProjectDetails> ReadFileAndProcessContents(string filePath)
        {
            _logger.LogInformation("ReadFileAndProcessContents");
            //Now split the file up into its individual lines

            var project = new ProjectDetails();
            project.Path = filePath;

            string[] lines = await File.ReadAllLinesAsync(filePath);

            project.Name = GetProjectNameFromPath(filePath);
            TryGetProjectTargetFramework(lines, project);
            return project;
        }

        #region Private Events

        /// <summary>
        /// Query and process the name of the project from the name of the project file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetProjectNameFromPath(string filePath)
        {
            int filePathSlashIndex = 0;
            //TODO: follow open/closed and try and get rid of the else!
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Do something
                filePathSlashIndex = filePath.LastIndexOf("\\");
            }
            else
            {
                //mac OR linux
                filePathSlashIndex = filePath.LastIndexOf("/");
            }
            return filePath.Substring(filePathSlashIndex).Replace(".csproj", "").Trim();
        }

        /// <summary>
        /// read each project csproj line and start to process the contents within
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="proj"></param>
        private void ProcessProjectReferences(string[] lines, ProjectDetails proj)
        {
            if (!lines.Any())
                return;

            foreach (string lineRec in lines)
            {
                FindAndParsePackageReference(lineRec);
            }
        }

        /// <summary>
        /// Read each line and try to process the target framework into the project
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="project"></param>
        private void TryGetProjectTargetFramework(string[] lines, ProjectDetails project)
        {
            foreach (string lineRecord in lines)
            {
                if (lineRecord.Contains("TargetFramework"))
                {
                    project.Framework = checkAndProcessTargetFramework(lineRecord);
                    return;
                }
            }
        }

        /// <summary>
        /// Read through, find packages references and record them
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private PackageInfo FindAndParsePackageReference(string line)
        {
            PackageInfo res = null;
            if (line.TrimStart().StartsWith("<PackageReference"))
            {
                res.Name = tryGetPackageName(line);
                //Version - to be tidied up
                res.CurrentVersion = tryGetPackageVersion(line);
            }
            return res;
        }

        /// <summary>
        /// Query the content of a project framework version
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string checkAndProcessTargetFramework(string line)
        {
            int pFrom = line.IndexOf(">") + ">".Length;
            int pTo = line.LastIndexOf("<");

            return line.Substring(pFrom, pTo - pFrom) ?? string.Empty;
        }

        #region package line - processes

        // /// <summary>
        // /// check the provided line for the included name
        // /// </summary>
        // /// <param name="line">file line</param>
        // /// <returns>the package name</returns>
        // private string tryGetPackageName(string line)
        // {
        //     int packageIndex = line.IndexOf("Include=") + "Include=".Length;

        //     string packageName = line.Substring(packageIndex);
        //     int versionStart = packageName.IndexOf("Version=");
        //     packageName = packageName.Remove(versionStart);
        //     packageName = packageName.Replace("\"", "").Trim();

        //     return packageName;
        // }

        // /// <summary>
        // /// check the provided line for the included package name version
        // /// </summary>
        // /// <param name="line">file line</param>
        // /// <returns>string formatted version number</returns>
        // private string tryGetPackageVersion(string line)
        // {
        //     int versionIndex = line.IndexOf("Version=") + "Version=".Length;

        //     string packageVersion = line.Substring(versionIndex);
        //     packageVersion = packageVersion.Replace("\"", "");
        //     packageVersion = packageVersion.Replace("/>", "").Trim();
        //     return packageVersion;
        // }

        #endregion

        #endregion
    }
}