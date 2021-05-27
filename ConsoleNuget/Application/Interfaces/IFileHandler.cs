using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IFileHandler
    {
        Task<ProjectDetails> ReadFileAndProcessContents(string filePath);
    }
}