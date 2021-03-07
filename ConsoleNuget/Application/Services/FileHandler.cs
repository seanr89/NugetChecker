using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Application.Interfaces;
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

        public bool ReadFileAndProcessContents(string filePath)
        {
            _logger.LogInformation("ReadFileAndProcessContents");
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

        #endregion
    }
}