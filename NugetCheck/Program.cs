using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
namespace NugetCheck
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        private static string env { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Nuget Checker\r");
            Console.WriteLine("------------------------\n");

            Configuration = LoadAppSettings();

            var serviceCollection = new ServiceCollection();
            RegisterAndInjectServices(serviceCollection, Configuration);
            //Initialise netcore dependency injection provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            string filePath = @"C:\Users\craft\Documents\Programming\GIT\NugetChecker\NugetCheck\NugetCheck.csproj";
            
            try{
                string inputFilePath = args[1];
                if(string.IsNullOrEmpty(inputFilePath) == false)
                    filePath = inputFilePath;
            }
            catch
            {
                //we just want to skip past the error if no input
                //Console.WriteLine($"File argument exception");
            }

            Console.WriteLine($"File to search: {filePath}");

            try
            {
                //Asynchronous method executed with Wait added to ensure that console request is not output too early
                serviceProvider.GetService<FileChecker>().Execute(filePath).Wait();
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine($"Implementation Exception caught: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Generic Exception caught: {e.Message}");
            }

            Console.WriteLine("App Completed");
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
        /// <param name="services"></param>
        /// <param name="config"></param>
        private static void RegisterAndInjectServices(IServiceCollection services, IConfiguration config)
        {
            services.AddLogging(logging =>
            {
                logging.AddConsole();
            }).Configure<LoggerFilterOptions>(options => options.MinLevel =
                                                LogLevel.Warning);

            services.AddSingleton<FileChecker>();

            services.AddTransient<NugetService>();

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
