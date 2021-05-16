
using Application.Interfaces;

namespace Application.Services.Updaters
{
    public class BashUpdater : IUpdater
    {
        public bool TryExecuteCmd(string packageName, string packageVersion, string folderPath)
        {
            throw new System.NotImplementedException();
        }

        public bool TryExecuteCmdTest()
        {
            throw new System.NotImplementedException();
        }
    }
}