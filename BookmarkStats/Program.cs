using System;
using System.Diagnostics;
using System.IO;

namespace BookmarkStats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var outputPath = args[0] == "-o" ? args[1] : "stats.txt";

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Bookmarks");
            var bookmarks = BookmarkParser.Parse(File.ReadAllText(path));
            var stats = BookmarkStats.Get(bookmarks);
            
            File.WriteAllText(outputPath, stats);
            Process.Start(outputPath);
        }
    }
}
