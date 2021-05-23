using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
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
        private readonly UpdaterFactory _updaterFactory;

        public FolderSearcher(ILogger<FolderSearcher> logger, IFileHandler fileHandler
            , INugetService nugetService, UpdaterFactory updaterFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _filehandler = fileHandler ?? throw new ArgumentNullException(nameof(fileHandler));
            _nugetService = nugetService ?? throw new ArgumentNullException(nameof(nugetService));
            _updaterFactory = updaterFactory ?? throw new ArgumentNullException(nameof(updaterFactory));
            _inputProvider = Console.ReadLine;
            _outputProvider = Console.WriteLine;
            _projects = new List<ProjectDetails>();
        }

        /// <summary>
        /// Base method that is execute on startup
        /// </summary>
        /// <param name="folderPath">Base folder path to search for relevant csproj files!</param>
        /// <returns></returns>
        public async Task Run(string folderPath)
        {
            _outputProvider($"Run : {folderPath}");
            string[] files = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories);

            if (!files.Any())
                return;

            var stepSearch = false; //ConsoleMethods.Confirm("Do you wish to search each project invidually?");
            await this.ProcessCSProjFiles(files, stepSearch);
            await BeginPackageChecks(_projects);
        }

        /// <summary>
        /// Support the processing each found CSProj File for Data and Packages
        /// </summary>
        /// <param name="files">array of filenames and paths!</param>
        private async Task ProcessCSProjFiles(string[] files, bool stepSearch)
        {
            foreach (string filePath in files)
            {
                var result = await _filehandler.ReadFileAndProcessContents(filePath);
                _projects.Add(result);
                CheckStagedSearchAndWaitIfNeeded(stepSearch);
            }
        }

        /// <summary>
        /// Checks to see if the user has requested a delay and if so requests the user continue through a console event!
        /// </summary>
        /// <param name="stepSearch">if the user has agreed or rejected to manually step through each</param>
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
                //TODO: we could include a question here to ask user if they want to update!
                if (proj.Packages.Any())
                    ReviewProjectPackagesForUpdatesAvailable(proj, true);
            }
        }

        private void ReviewProjectPackagesForUpdatesAvailable(ProjectDetails project, bool update = false)
        {
            _outputProvider($"FolderSearcher:ReviewProjectPackagesForUpdatesAvailable");
            var updater = _updaterFactory.GetUpdater();
            bool packagesUpdated = false;
            foreach (var pack in project.Packages)
            {
                if (pack.Response != null)
                {
                    var latestPackage = pack.Response.data[0];
                    if (pack.CurrentVersion != latestPackage.Version)
                    {
                        //package is not on latest version!
                        _outputProvider($"Package: {pack.Name} can be updated to version : {latestPackage.Version}");
                        if (update)
                        {
                            bool updated = updater.TryExecuteCmd(pack.Name, latestPackage.Version, project.Path);
                            if (updated)
                            {
                                packagesUpdated = true;
                                _outputProvider($"Updated package {pack.Name}");
                            }

                        }
                    }
                }
            }
            if (update && packagesUpdated)
                updater.TryRestorePackages(project.Path);
        }
    }
}