using System;
using System.IO;
using System.Linq;


namespace FolderSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string folderPath = @"C:\Users\craft\Documents\Programming\GIT\NugetChecker";
            
            try{
                string inputFilePath = args[1];
                if(string.IsNullOrEmpty(inputFilePath) == false)
                    folderPath = inputFilePath;
            }
            catch
            {
                //we just want to skip past the error if no input
                //Console.WriteLine($"File argument exception");
            }

            string[] files = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories);

            if(files.Any())
            {
                foreach(string file in files)
                {
                    Console.WriteLine($"found file: {file}");
                }
            }

            Console.WriteLine("App Completed");
        }
    }
}
