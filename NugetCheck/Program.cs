using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace NugetCheck
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        private static string env { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Nuget Version Checker and Updater\r");
            Console.WriteLine("------------------------\n");

            Configuration = LoadAppSettings();

            var serviceCollection = new ServiceCollection();
            RegisterAndInjectServices(serviceCollection, Configuration);
            //Initialise netcore dependency injection provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //windows path
            //string folderPath = @"C:\Users\seanr\Source\Repos\PersonalApps\NugetChecker";
            string folderPath = @"C:\Users\craft\Documents\Programming\GIT\NugetChecker";
            //mac path
            //string folderPath = @"/Users/seanrafferty/Documents/Projects/NugetChecker";

            try
            {
                string inputFilePath = args[0];
                if (string.IsNullOrEmpty(inputFilePath) == false)
                    folderPath = inputFilePath;
            }
            catch
            {
                //TODO: should error out here as we need a file/folderpath
            }
            Console.WriteLine($"Folder to search: {folderPath} - Searching");
            string[] files = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories);

            bool attemptUpdate = Confirm("Do you want to attempt to update?");

            try
            {
                //Asynchronous method executed with Wait added to ensure that console request is not output too early
                serviceProvider.GetService<FileChecker>().Execute(files, attemptUpdate).Wait();
            }
            catch (NotImplementedException nie)
            {
                Console.WriteLine($"Implementation Exception caught: {nie.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Generic Exception caught: {e.Message}");
            }
            Console.WriteLine("App Completed");
            return;
        }

        /// <summary>
        /// Query app settings json content
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot LoadAppSettings()
        {
            try
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

                return config;
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Error trying to load app settings");
                return null;
            }
        }

        /// <summary>
        /// Prep/Configure Dependency Injection
        /// </summary>
        /// <param name="services">DI service layer</param>
        /// <param name="config">App Settings configuration</param>
        private static void RegisterAndInjectServices(IServiceCollection services, IConfiguration config)
        {
            services.AddLogging(logging =>
            {
                logging.AddConsole();
            }).Configure<LoggerFilterOptions>(options => options.MinLevel =
                                                LogLevel.Warning);

            services.AddSingleton<FileChecker>();

            services.AddTransient<NugetService>();

            services.AddTransient<CmdExecutor>();
            services.AddTransient<MacExecutor>();
            services.AddTransient<PackageComparer>();

            services.AddHttpClient<NugetService>();
        }

        /// <summary>
        /// Test method to handle Yes/No selection on console apps
        /// </summary>
        /// <param name="title">The text to display on the read message</param>
        /// <returns></returns>
        public static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{ title } [y/n] ");
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return (response == ConsoleKey.Y);
        }
    }
}
