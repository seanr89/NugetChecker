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
    /// <summary>
    /// Open provided project folder to then process the csproj files and prep for processing!
    /// </summary>
    public class FolderSearcher
    {
        private readonly ILogger<FolderSearcher> _logger;
        private readonly Func<string> _inputProvider;
        private readonly Action<string> _outputProvider;

        private readonly IFileHandler _filehandler;
        private readonly INugetService _nugetService;
        private readonly List<ProjectDetails> _projects;
        //private readonly IProjectManager _projectManager;

        public FolderSearcher(ILogger<FolderSearcher> logger, IFileHandler fileHandler, INugetService nugetService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _filehandler = fileHandler ?? throw new ArgumentNullException(nameof(fileHandler));
            _nugetService = nugetService ?? throw new ArgumentNullException(nameof(nugetService));
            _inputProvider = Console.ReadLine;
            _outputProvider = Console.WriteLine;
            _projects = new List<ProjectDetails>(); //TODO: remove this!
        }

        public async Task Run(string folderPath)
        {
            _outputProvider($"Run : {folderPath}");

            //Console.WriteLine($"Folder to search: {folderPath} - Searching");
            string[] files = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories);

            if (!files.Any())
                return;

            var stepSearch = false; //ConsoleMethods.Confirm("Do you wish to search each project invidually?");
            await this.ProcessFiles(files, stepSearch);
            await BeginPackageChecks(_projects);
        }

        /// <summary>
        /// Support the processing of all of each found file!
        /// </summary>
        /// <param name="files"></param>
        private async Task ProcessFiles(string[] files, bool stepSearch)
        {
            foreach (string filePath in files)
            {
                //_logger.LogInformation(filePath);
                var result = await TryProcessFile(filePath);
                _projects.Add(result);

                CheckStagedSearchAndWaitIfNeeded(stepSearch);
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

        private async Task BeginPackageChecks(List<ProjectDetails> projects)
        {
            _logger.LogInformation("FolderSearcher:BeginPackageChecks");
            foreach (var proj in projects)
            {
                await _nugetService.queryPackagesForProject(proj);
                ReviewProjectPackagesForUpdatesAvailable(proj);
            }
        }

        private void ReviewProjectPackagesForUpdatesAvailable(ProjectDetails project, bool update = false)
        {
            _outputProvider($"FolderSearcher:ReviewProjectPackagesForUpdatesAvailable");
            foreach (var pack in project.Packages)
            {
                if (pack.Response != null)
                {
                    var latestPackage = pack.Response.data[0];
                    if (pack.CurrentVersion != latestPackage.Version)
                    {
                        //package is not on latest version!
                        Console.WriteLine($"Package: {pack.Name} can be updated to version : {latestPackage.Version}");
                        if (update)
                        {
                            // bool updated = updater.TryExecuteCmd(package.Name, latestPackage.Version, project.Path);
                            // if (updated)
                            //     Console.WriteLine($"Updated package {package.Name}");
                        }
                    }
                }
            }
        }
    }
}