using System;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class FolderSearcher
    {
        private readonly ILogger<FolderSearcher> _logger;
        private readonly Func<string> _inputProvider;
        private readonly Action<string> _outputProvider;
        public FolderSearcher(ILogger<FolderSearcher> logger)
        {
            _logger = logger;
            _inputProvider = Console.ReadLine;
            _outputProvider = Console.WriteLine;
        }

        public void Run()
        {
            _outputProvider("Run");
            //_logger.LogInformation("FolderSearcher:Run");
        }
    }
}