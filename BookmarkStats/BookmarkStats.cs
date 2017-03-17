using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookmarkStats
{
    public static class BookmarkStats
    {
        private static Regex websiteRegex = new Regex(@"^https?://([^\./]+\.)*(?<name>[^\./]+\.[^/]+)/.*$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private const int TOP_COUNT = 20;

        public static string Get(BookmarkFolder folder)
        {
            var contents = "";

            contents += $"Bookmark file fetch date: {folder.DateAdded}\n";

            var bookmarks = (List<Bookmark>)folder;

            contents += GetDateExtremes(bookmarks);
            contents += GetMostBookmarkedWebsites(bookmarks);

            return contents;
        }

        private static string GetDateExtremes(List<Bookmark> bookmarks)
        {
            var contents = "\n";

            var ordered = bookmarks.OrderBy(pair => pair.DateAdded);
            var first = ordered.First().DateAdded;
            var last = ordered.Last().DateAdded;
            contents += $"The first bookmark was added at {first}\n";
            contents += $"The last bookmark was added at {last}\n";
            contents += $"Time span in between the these dates: {Readable(last - first)}\n";

            return contents;
        }

        private static string GetMostBookmarkedWebsites(List<Bookmark> bookmarks)
        {
            var contents = "\n";

            var websites = new Dictionary<string, int>();
            int nonWebsiteCount = 0;
            foreach (var bookmark in bookmarks)
            {
                var match = websiteRegex.Match(bookmark.Url);
                if (match.Success)
                {
                    var name = match.Groups["name"].Value;
                    if (!websites.ContainsKey(name))
                        websites.Add(name, 0);

                    websites[name]++;
                }
                else
                    nonWebsiteCount++;
            }

            contents += $"Non-website bookmark count: {nonWebsiteCount}\n\n";

            contents += $"Top {TOP_COUNT} most bookmarked websites:\n\n";

            var orderedWebsites = websites.OrderByDescending(pair => pair.Value).Take(TOP_COUNT).ToList();
            for (int i = 0; i < orderedWebsites.Count; ++i)
                contents += $"{i + 1}. {orderedWebsites[i].Key} : {orderedWebsites[i].Value} hits\n";

            return contents + "\n";
        }

        private static string Readable(TimeSpan ts)
        {
            const double daysInYear = 365.242199;
            const double daysInMonth = daysInYear / 12;

            var totalDays = ts.TotalDays;
            int years = (int)(totalDays / daysInYear);
            totalDays -= years * daysInYear;
            int months = (int)(totalDays / daysInMonth);
            totalDays -= months * daysInMonth;
            int days = (int)totalDays;
            
            var contents = "";
            if (years > 0)
                contents += $"{years} year{(years != 1 ? "s" : "")} ";

            if (months > 0)
                contents += $"{months} month{(months != 1 ? "s" : "")} ";

            contents += $"{days} day{(days != 1 ? "s" : "")}";

            return contents.TrimEnd();
        }
    }
}
