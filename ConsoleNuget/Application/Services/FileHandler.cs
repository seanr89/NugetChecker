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
    }
}