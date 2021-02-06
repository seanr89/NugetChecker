###########################################################
#
# Script to upgrade all NuGet packages in solution to last version
#
# USAGE
# Place this file (Upgrade-Package.ps1) to your solution folder. 
# From Package Manager Console execute
#
# .\Upgrade-Package.ps1 -PackageID:Castle.Core
#
# Do not hestitate to contact me at any time
#
# Update to NuGet 1.1 is done by Sean, see comments bellow
#
##########################################################

param($PackageID)

$packageManager = $host.PrivateData.packageManagerFactory.CreatePackageManager()

foreach ($project in Get-Project -all) {
    $fileSystem = New-Object NuGet.PhysicalFileSystem($project.Properties.Item("FullPath").Value) 	
    $repo = New-Object NuGet.PackageReferenceRepository($fileSystem, $packageManager.LocalRepository)

    foreach ($package in $repo.GetPackages() | ? { $_.Id -eq $PackageID }) {
        Update-Package $package.Id -Project:$project.Name 
    }
}