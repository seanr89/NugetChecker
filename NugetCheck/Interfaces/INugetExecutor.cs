
namespace NugetCheck.Interfaces
{
    public interface INugetExecutor
    {
        bool TryExecuteCmdTest();

        bool TryExecuteCmd(string packageName, string packageVersion, string folderPath);
    }
}