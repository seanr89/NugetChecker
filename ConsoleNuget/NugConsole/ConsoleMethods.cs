using System;
using System.Threading;

namespace NugConsole
{
    /// <summary>
    /// Helper class to simplify styling and formatting of the excel worksheet
    /// </summary>
    public static class ConsoleMethods
    {
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

        public static void EnableCloseOnCtrlC()
        {
            //Handle cancellation/closure events
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, a) =>
            {
                a.Cancel = true;
                cts.Cancel();
                Console.WriteLine("EnableCloseOnCtrlC");
                //await connection.InvokeAsync("sendMessage", "ConsoleClient", $"{_userName} has left");
                Environment.Exit(0);
            };
        }
    }
}