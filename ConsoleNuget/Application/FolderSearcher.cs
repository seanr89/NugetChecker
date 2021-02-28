using System;
using System.IO;
using System.Linq;
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

        public void Run(string folderPath)
        {
            _outputProvider($"Run : {folderPath}");

            //var records = _inputProvider() ?? string.Empty;
            //_logger.LogInformation("FolderSearcher:Run");

            Console.WriteLine($"Folder to search: {folderPath} - Searching");
            string[] files = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories);

            //TODO: Begin Search
            this.Search(files);
        }

        private void Search(string[] files)
        {
            if (!files.Any())
                return;

            var stepSearch = ConsoleMethods.Confirm("Do you wish to search each project invidually?");
        }
    }
}