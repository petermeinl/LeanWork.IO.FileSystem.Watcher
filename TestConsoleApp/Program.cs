using LeanWork.IO.FileSystem;
using System;
using System.IO;
using System.Threading.Tasks;
using static TestConsoleApp.ConsoleHelpers;

namespace TestConsoleApp
{
    class Program
    {
        //const string TestPath = @"\\gogo\test\in";
        const string TestPath = @"D:\Temp\in";

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            Console.WindowWidth = 130;

            try
            {
                //RunBufferingWatcher();
                RunRecoveringWatcher();
            }
            catch (Exception ex)
            {
                PromptUser(ex.Message, ConsoleColor.Red);
            }
        }

        static void RunBufferingWatcher()
        {
            using (var watcher = new BufferingFileSystemWatcher(TestPath))
            {
                watcher.All += (_, e) => { WriteLineInColor($"{e.ChangeType} {e.Name}", ConsoleColor.Yellow); };

                //watcher.Existed += (_, e) => { WriteLineInColor($"Existed {e.Name}", ConsoleColor.Yellow); };
                //watcher.Created += (_, e) => { WriteLineInColor($"Created {e.Name}", ConsoleColor.Yellow); };
                //watcher.Deleted += (_, e) => { WriteLineInColor($"Deleted {e.Name}", ConsoleColor.Yellow); };
                //watcher.Renamed += (_, e) => { WriteLineInColor($"Renamed {e.OldName} to {e.Name}", ConsoleColor.Yellow); };
                watcher.Error += (_, e) => { WriteLineInColor(e.GetException().Message, ConsoleColor.Red); };
                //watcher.EventQueueCapacity = 1;
                watcher.EnableRaisingEvents = true;

                //PromptUser("Interrupt...");
                //watcher.EnableRaisingEvents = false;
                //PromptUser("Interupted", ConsoleColor.Red);
                //watcher.EnableRaisingEvents = true;

                PromptUser("Processing...");
                Console.WriteLine("Stopping...");
            }
            PromptUser("Stopped.");
        }

        static void RunRecoveringWatcher()
        {
            Console.WriteLine("Will auto-detect unavailability of watched directory");
            Console.WriteLine(" - Windows timeout accessing network shares: ~110 sec on start, ~45 sec while watching.");
            using (var watcher = new RecoveringFileSystemWatcher(TestPath))
            {
                watcher.All += (_, e) => { WriteLineInColor($"{e.ChangeType} {e.Name}", ConsoleColor.Yellow); };
                watcher.Error += (_, e) => { WriteLineInColor(e.Error.Message, ConsoleColor.Red); };
                //watcher.Error += (_, e) =>
                //    { WriteLineInColor($"Suppressing auto handling of error {e.Error.Message}", ConsoleColor.Red);
                //        //...
                //        e.Handled = true;
                //    };
                //watcher.DirectoryMonitorInterval = TimeSpan.FromSeconds(10);
                //watcher.EventQueueCapacity = 1;
                watcher.EnableRaisingEvents = true;

                PromptUser("Processing...");
                Console.WriteLine("Stopping...");
            }
            PromptUser("Stopped.");
        }
    }

    public static class ConsoleHelpers
    {
        public static void PromptUser(string message, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            WriteLineInColor($"? {message} !Press <Enter> to contine.", foregroundColor);
            do { } while (!(Console.ReadKey(true).Key == ConsoleKey.Enter));
        }

        public static void WriteLineInColor(string message, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
