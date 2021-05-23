using System;
using System.Runtime.InteropServices;
using Application.Interfaces;
using Application.Services.Updaters;

namespace Application.Services
{
    /// <summary>
    /// Factory class to support Updater Selection based on OS version!
    /// </summary>
    public class UpdaterFactory
    {
        public IUpdater GetUpdater()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new CmdUpdater();
            }
            else
            {
                return new BashUpdater();
            }
        }
    }
}