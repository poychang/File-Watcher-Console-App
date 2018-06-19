using System;

namespace FileWatcherConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string monitoringPath = @"C:\TestFileWatcher";
            Console.WriteLine($"Monitoring file changed in target folder.");
            Console.WriteLine($"Target Folder: {monitoringPath}");

            new FileSystemWatcher(monitoringPath, 1)
                .WatchCreated()
                .WatchChanged();

            Console.ReadLine();
        }
    }
}
