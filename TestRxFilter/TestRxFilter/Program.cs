using LeanWork.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reactive.Linq;

namespace TestRxFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var watcher = new RecoveringFileSystemWatcher(@"d:\temp\in");
                watcher.DirectoryMonitorInterval = TimeSpan.FromSeconds(5);
                Observable
                  .FromEventPattern<FileSystemEventArgs>(watcher, "Created")
                  .Select(x => x.EventArgs)
                  .Delay(TimeSpan.FromMilliseconds(10))
                  .Where(x => x.ChangeType == WatcherChangeTypes.Deleted || File.Exists(x.FullPath))
                  .Subscribe(x => Console.WriteLine(x.Name));
                Observable
                  .FromEventPattern<FileWatcherErrorEventArgs>(watcher, "Error")
                  .Subscribe(x => Console.WriteLine(x.EventArgs.Error.Message));

                watcher.EnableRaisingEvents = true;
                Console.WriteLine("watching...");
                Console.ReadLine();
                watcher.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
