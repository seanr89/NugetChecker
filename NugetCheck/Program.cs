﻿using System;
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

            // Console.WriteLine("Input how many days to calculate back, and then press Enter (1, 2, 3, etc...)");
            // int days = Convert.ToInt32(Console.ReadLine());

            string filePath = @"C:\Users\craft\Documents\Programming\GIT\NugetChecker\NugetCheck\NugetCheck.csproj";
            
            string inputFilePath = args[1];
            //Console.WriteLine($"fileName: {fileName}");

            try
            {
                //Asynchronous method executed with Wait added to ensure that console request is not output too early
                serviceProvider.GetService<FileChecker>().Execute().Wait();
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
            //Console.WriteLine("LoadAppSettings");
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
            Console.WriteLine("RegisterAndInjectServices");

            services.AddLogging(logging =>
            {
                logging.AddConsole();
            }).Configure<LoggerFilterOptions>(options => options.MinLevel =
                                                LogLevel.Warning);

            services.AddSingleton<FileChecker>();
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
