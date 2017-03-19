using System;
using System.Diagnostics;
using System.IO;

namespace BookmarkStats
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var outputPath = "stats.txt";
            var inputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\Default\Bookmarks");

            try
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    switch (args[i])
                    {
                        case "-o": outputPath = args[++i]; break;
                        case "-i": inputPath = args[++i]; break;
                        default: throw new Exception(); // ArgumentException("Wrong argument format");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Wrong argument format");
                return;
            }

            var bookmarks = BookmarkParser.Parse(File.ReadAllText(inputPath));
            var stats = BookmarkStats.Get(bookmarks);
            
            File.WriteAllText(outputPath, stats);
            Process.Start(outputPath);
        }
    }
}
