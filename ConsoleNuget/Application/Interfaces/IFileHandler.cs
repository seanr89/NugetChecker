using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileHandler
    {
        bool ReadFileAndProcessContents(string filePath);
    }
}