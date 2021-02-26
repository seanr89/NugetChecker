using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileHandler
    {
        Task ReadFileAndProcessContents();
    }
}