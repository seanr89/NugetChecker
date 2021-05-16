using System;
using Application.Interfaces;

namespace Application.Services
{
    public class UpdaterFactory
    {
        public UpdaterFactory()
        {

        }

        public IUpdater GetUpdaterByOS()
        {
            throw new NotImplementedException();
        }
    }
}