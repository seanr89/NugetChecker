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
        public FileHandler(ILogger<FileHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> ReadFileAndProcessContents(string filePath)
        {
            _logger.LogInformation("ReadFileAndProcessContents");
            //Now split the file up into its individual lines

            var project = new ProjectDetails();
            project.Path = filePath;

            string[] lines = await File.ReadAllLinesAsync(filePath);

            project.Name = GetProjectNameFromPath(filePath);
            return false;
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
        }


        #endregion
    }
}