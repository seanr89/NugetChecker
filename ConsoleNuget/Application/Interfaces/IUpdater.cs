namespace Application.Interfaces
{
    public interface IUpdater
    {
        bool TryExecuteCmdTest();

        bool TryExecuteCmd(string packageName, string packageVersion, string folderPath);

        bool TryRestorePackages(string folderPath);
    }
}