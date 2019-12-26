using System;
using System.IO;

namespace WatcherWeak
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            _wather = new FileSystemWatcher(@"D:\11.txt")
            {
                EnableRaisingEvents = true
            };

            var weakEvent = new FileSystemWatcherWeakEventRelay(_wather);
            weakEvent.Created += OnCreated;
            weakEvent.Changed += OnChanged;
            weakEvent.Renamed += OnRenamed;
            weakEvent.Deleted += OnDeleted;
        }

        private static FileSystemWatcher _wather;

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("OnCreated");
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("OnChanged");
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine("OnRenamed");
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("OnDeleted");
        }
    }
}