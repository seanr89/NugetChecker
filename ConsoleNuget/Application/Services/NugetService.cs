
using System.Threading.Tasks;
using Application.Interfaces;
namespace Application.Services
{
    public class NugetService : INugetService
    {
        public Task<bool> queryPackageByName(string packageName)
        {
            throw new System.NotImplementedException();
        }
    }
}