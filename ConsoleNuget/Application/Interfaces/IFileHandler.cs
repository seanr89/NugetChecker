using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileHandler
    {
        Task<bool> ReadFileAndProcessContents(string filePath);
    }
}