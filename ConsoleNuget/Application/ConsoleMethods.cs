using System;
using System.Threading;

namespace Application
{
    /// <summary>
    /// Helper class to simplify styling and formatting of the excel worksheet
    /// </summary>
    public static class ConsoleMethods
    {
        /// <summary>
        /// Provides console commands to handle Yes/No selection on console apps
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

        /// <summary>
        /// Provides logic to monitor and read for ctrl+c events to close the app
        /// </summary>
        public static void EnableCloseOnCtrlC()
        {
            //Handle cancellation/closure events
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, a) =>
            {
                a.Cancel = true;
                cts.Cancel();
                Console.WriteLine("EnableCloseOnCtrlC");
                Environment.Exit(0);
            };
        }
    }
}