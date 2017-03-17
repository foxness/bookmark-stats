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

            contents += $"Bookmark file fetch date: {folder.DateAdded}\n\n";

            var bookmarks = (List<Bookmark>)folder;
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

            return contents;
        }
    }
}
