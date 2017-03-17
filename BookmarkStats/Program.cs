using System;
using System.IO;

namespace BookmarkStats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Bookmarks");
            var bookmarks = BookmarkParser.Parse(File.ReadAllText(path));

            Console.ReadKey();
        }
    }
}
