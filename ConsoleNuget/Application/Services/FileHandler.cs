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
            int filePathSlashIndex = getFilePathSlashIndex(filePath);
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
        /// <param name="lines">All provided file lines</param>
        /// <param name="project">the project record to be updated!</param>
        private void TryGetProjectTargetFramework(string[] lines, ProjectDetails project)
        {
            foreach (string lineRecord in lines)
            {
                if (lineRecord.Contains("TargetFramework"))
                {
                    project.Framework = _lineReader.checkAndProcessTargetFramework(lineRecord);
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
                res.Name = _lineReader.tryGetPackageName(line);
                //Version - to be tidied up
                res.CurrentVersion = _lineReader.tryGetPackageVersion(line);
            }
            return res;
        }

        private int getFilePathSlashIndex(string filePath)
        {
            //TODO: follow open/closed and try and get rid of the else!
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Do something
                return filePath.LastIndexOf("\\");
            }
            //mac OR linux
            return filePath.LastIndexOf("/");
        }

        #endregion
    }
}