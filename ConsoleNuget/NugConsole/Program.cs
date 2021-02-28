using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NugConsole
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        private static string env { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!\r");
            // Console.WriteLine("\n");
            Configuration = LoadAppSettings();

            var serviceCollection = new ServiceCollection();
            RegisterAndInjectServices(serviceCollection, Configuration);

            //Initialise netcore dependency injection provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            //Initialise folderpath param and check for argument var!
            string folderPath = "";
            try
            {
                string inputFilePath = args[0];
                if (string.IsNullOrEmpty(inputFilePath) == false)
                    folderPath = inputFilePath;
            }
            catch
            {
                //TODO: should error out here as we need a file/folderpath
                Console.WriteLine("No folder path provided!");
            }

            ConsoleMethods.EnableCloseOnCtrlC();

            try
            {
                //Asynchronous method executed with Wait added to ensure that console request is not output too early
                serviceProvider.GetService<FolderSearcher>().Run();
            }
            catch (NotImplementedException nie)
            {
                Console.WriteLine($"Implementation Exception caught: {nie.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Generic Exception caught: {e.Message}");
            }
            //Console.ReadLine();
            Console.WriteLine("Closing App");
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
            services.AddApplication();
        }
    }
}
