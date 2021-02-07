namespace NugetCheck
{
    public class ProjectUpdater
    {
        private readonly CmdExecutor _cmdExecute;
        public ProjectUpdater(CmdExecutor cmdExecute)
        {
            _cmdExecute = cmdExecute;
        }

        public void TryUpdatePackageForProject(ProjectPackages proj)
        {

        }
    }
}