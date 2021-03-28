using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IProjectManager
    {
        Task ProcessProjectDetails(ProjectDetails proj);
    }
}