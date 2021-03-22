using System;
using System.Collections.Generic;
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
        private readonly List<ProjectDetails> _projects;

        public FolderSearcher(ILogger<FolderSearcher> logger, IFileHandler fileHandler)
        {
            _logger = logger;
            _filehandler = fileHandler;
            _inputProvider = Console.ReadLine;
            _outputProvider = Console.WriteLine;
            _projects = new List<ProjectDetails>();
        }

        public async Task Run(string folderPath)
        {
            _outputProvider($"Run : {folderPath}");
            //_logger.LogInformation("FolderSearcher:Run");

            Console.WriteLine($"Folder to search: {folderPath} - Searching");
            string[] files = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories);

            if (!files.Any())
                return;
            var stepSearch = ConsoleMethods.Confirm("Do you wish to search each project invidually?");
            await this.ProcessFiles(files, stepSearch);

            //now pass files over to run nuget processing
        }

        /// <summary>
        /// Support the processing of all of each found file!
        /// </summary>
        /// <param name="files"></param>
        private async Task ProcessFiles(string[] files, bool stepSearch)
        {
            foreach (string filePath in files)
            {
                _logger.LogInformation(filePath);
                var result = await TryProcessFile(filePath);
                _projects.Add(result);

                CheckStagedSearchAndWaitIfNeeded(stepSearch);
            }
        }

        /// <summary>
        /// check if the user asked to process through each project individually and wait if required!
        /// </summary>
        /// <param name="stepSearch"></param>
        private void CheckStagedSearchAndWaitIfNeeded(bool stepSearch)
        {
            if (stepSearch)
            {
                _outputProvider($"Scan next file press any key");
                var response = _inputProvider() ?? string.Empty;
                return;
            }
            return;
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