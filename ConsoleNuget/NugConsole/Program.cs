using System;

namespace NugConsole
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        private static string env { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!\r");
            Console.WriteLine("\n");
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
            }
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
            this.AddApplication();
        }
    }
}
