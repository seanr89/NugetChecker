using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class FolderSearcher
    {
        private readonly ILogger<FolderSearcher> _logger;
        private readonly Func<string> _inputProvider;
        private readonly Action<string> _outputProvider;
        private readonly IFileHandler _filehandler;
        public FolderSearcher(ILogger<FolderSearcher> logger, IFileHandler fileHandler)
        {
            _logger = logger;
            _filehandler = fileHandler;
            _inputProvider = Console.ReadLine;
            _outputProvider = Console.WriteLine;
        }

        public void Run(string folderPath)
        {
            _outputProvider($"Run : {folderPath}");
            //_logger.LogInformation("FolderSearcher:Run");

            Console.WriteLine($"Folder to search: {folderPath} - Searching");
            string[] files = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories);

            if (!files.Any())
                return;
            this.ProcessFiles(files);
        }

        /// <summary>
        /// Support the processing of all of each found file!
        /// </summary>
        /// <param name="files"></param>
        private void ProcessFiles(string[] files)
        {
            var stepSearch = ConsoleMethods.Confirm("Do you wish to search each project invidually?");
            foreach (string filePath in files)
            {
                _logger.LogInformation(filePath);
                var result = TryProcessFile(filePath);
                if (stepSearch)
                {
                    _outputProvider($"Scan next file press any key");
                    var drink = _inputProvider() ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Executes the reading/parsing handling of the file details!
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task<ProjectDetails> TryProcessFile(string filePath)
        {
            return await _filehandler.ReadFileAndProcessContents(filePath);
        }
    }
}