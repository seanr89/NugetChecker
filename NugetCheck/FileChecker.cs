using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using System.Runtime.InteropServices;

namespace NugetCheck
{
    /// <summary>
    /// File checker class to start reading through the provider file list and support the update process!
    /// </summary>
    public class FileChecker
    {
        private readonly NugetService _nugetService;
        private readonly PackageComparer _comparer;

        public FileChecker(NugetService nugetService, PackageComparer comparer)
        {
            _nugetService = nugetService;
            _comparer = comparer;
        }

        /// <summary>
        /// Handles process execution
        /// TODO: needs split to support Open/Closed Principle more
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="attemptUpdate"></param>
        /// <returns></returns>
        public async Task Execute(string[] filePaths, bool attemptUpdate)
        {
            Console.WriteLine("FileChecker: Execute");
            if (!filePaths.Any())
                return;

            List<ProjectPackages> projects = new List<ProjectPackages>();

            //Loop through each provided file path of a .csproj file
            foreach (string filePath in filePaths)
            {
                //support initialisation of model to maintain details for a single project!
                var projectDetails = initialiseProjectPackage(filePath);
                //Now split the file up into its individual lines
                string[] lines = await File.ReadAllLinesAsync(filePath);
                this.ProcessProjectReferences(lines, projectDetails, filePath);
                await QueryPackUpdatesForProject(projectDetails);

                projects.Add(projectDetails);
            }
            this.RunPackageChecksForProjects(projects, attemptUpdate);
        }

        /// <summary>
        /// Supports the dedicated querying of package update parameters from nuget!
        /// </summary>
        /// <param name="projectDetails"></param>
        /// <returns></returns>
        private async Task QueryPackUpdatesForProject(ProjectPackages projectDetails)
        {
            if (projectDetails.Packages.Any())
            {
                foreach (var p in projectDetails.Packages)
                {
                    var nugetQueryResponse = await _nugetService.queryPackageByName(p.Name);
                    if (nugetQueryResponse != null)
                    {
                        p.Response = (NugetResponse)nugetQueryResponse;
                    }
                }
            }
        }

        /// <summary>
        /// Initialise and create a new project package model
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ProjectPackages initialiseProjectPackage(string filePath)
        {
            var result = new ProjectPackages();
            result.Path = filePath;
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
            result.Name = filePath.Substring(filePathSlashIndex).Replace(".csproj", "").Trim();
            return result;
        }

        /// <summary>
        /// read each project csproj line and start to process the contents within
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="projectDetails"></param>
        /// <param name="filePath"></param>
        private void ProcessProjectReferences(string[] lines, ProjectPackages projectDetails, string filePath)
        {
            if (!lines.Any())
                return;
            foreach (string reference in lines)
            {
                GetProjectTargetFramework(projectDetails, reference);
                FindAndParsePackageReference(projectDetails, filePath, reference);
            }
        }

        private void FindAndParsePackageReference(ProjectPackages projectDetails, string filePath, string reference)
        {
            if (reference.TrimStart().StartsWith("<PackageReference"))
            {
                var package = new PackageDetails();
                //Package name - to be tidied up!
                string packageName = tryGetPackageName(reference);
                //Version - to be tidied up
                string packageVersion = tryGetPackageVersion(reference);

                //TODO: note this could be the place to update that!
                package.UpdatePackageDetails(filePath, packageName, packageVersion);
                projectDetails.Packages.Add(package);
            }
        }

        private void GetProjectTargetFramework(ProjectPackages projectDetails, string reference)
        {
            if (reference.Contains("TargetFramework"))
            {
                String result = checkAndProcessTargetFramework(reference);
                projectDetails.Framework = result;
            }
        }

        /// <summary>
        /// TODO : add simple comment
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="attemptUpdate"></param>
        private void RunPackageChecksForProjects(List<ProjectPackages> projects, bool attemptUpdate)
        {
            foreach (var proj in projects)
            {
                //Ok now we need to write a checker
                _comparer.tryComparePackagesForProjectAndLogIfOutOfDate(proj, attemptUpdate);
            }
        }

        #region Private File Project

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

        /// <summary>
        /// check the provided line for the included name
        /// </summary>
        /// <param name="line">file line</param>
        /// <returns>the package name</returns>
        private string tryGetPackageName(string line)
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
        private string tryGetPackageVersion(string line)
        {
            int versionIndex = line.IndexOf("Version=") + "Version=".Length;

            string packageVersion = line.Substring(versionIndex);
            packageVersion = packageVersion.Replace("\"", "");
            packageVersion = packageVersion.Replace("/>", "").Trim();
            return packageVersion;
        }

        #endregion
    }
}