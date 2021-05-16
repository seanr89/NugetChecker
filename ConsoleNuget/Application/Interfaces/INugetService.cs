using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface INugetService
    {
        Task queryPackagesForProject(ProjectDetails proj);

        Task<NugetResponse?> queryIndividualPackageDetails(PackageInfo package);
    }
}