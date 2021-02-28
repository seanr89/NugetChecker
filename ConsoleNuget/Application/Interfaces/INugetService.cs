using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INugetService
    {
        Task<bool> queryPackageByName(string packageName);
    }
}