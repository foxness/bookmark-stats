using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace BookmarkStats
{
    public static class BookmarkParser
    {
        public static BookmarkFolder Parse(string json)
        {
            var roots = (JObject)JObject.Parse(json)["roots"];
            roots.Remove("sync_transaction_version");
            return new BookmarkFolder("Root", DateTime.Now, DateTime.Now, ParseFolderContents(new JArray(roots.Children().Select(a => a.First).ToArray())));
        }

        private static List<BookmarkBase> ParseFolderContents(JArray folder)
        {
            var result = new List<BookmarkBase>();

            foreach (var bookmarkBase in folder.Children<JObject>())
            {
                var name = bookmarkBase.Value<string>("name");
                var dateAdded = ParseDateTime(bookmarkBase.Value<long>("date_added"));

                switch (bookmarkBase.Value<string>("type"))
                {
                    case "folder":
                        var lastModified = ParseDateTime(bookmarkBase.Value<long>("date_modified"));
                        result.Add(new BookmarkFolder(name, dateAdded, lastModified, ParseFolderContents((JArray)bookmarkBase["children"])));

                        break;

                    case "url":
                        var url = bookmarkBase.Value<string>("url");
                        var lastVisited = bookmarkBase.TryGetValue("meta_info", out var metaInfo) ?
                            (DateTime?)ParseDateTime(metaInfo.Value<long>("last_visited_desktop")) : null;

                        result.Add(new Bookmark(name, dateAdded, url, lastVisited));

                        break;

                    default:
                        throw new ArgumentException($"Unexpected BookmarkBase type: {bookmarkBase.Value<string>("type")}");
                }
            }

            return result;
        }

        private static DateTime ParseDateTime(long raw)
            => new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(raw / 1000d); // is it really UTC though?
    }
}
