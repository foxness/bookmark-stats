using System;
using System.IO;

namespace ChromeBookmarks
{
    public class Program
    {
        private const string PATH = @"C:\Users\foxneSs\Desktop\Bookmarks";

        public static void Main(string[] args)
        {
            var bookmarks = BookmarkParser.Parse(File.ReadAllText(PATH));

            Console.ReadKey();
        }
    }
}
