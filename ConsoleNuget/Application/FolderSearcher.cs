using System;

namespace Application
{
    public class FolderSearcher
    {
        private readonly Func<string> _inputProvider;
        private readonly Action<string> _outputProvider;
        public FolderSearcher(Func<string> inputProvider, Action<string> outputProvider)
        {
            _inputProvider = inputProvider;
            _outputProvider = outputProvider;
        }

        /// <summary>
        /// First step to start the searching process!
        /// </summary>
        public void Run()
        {

        }
    }
}