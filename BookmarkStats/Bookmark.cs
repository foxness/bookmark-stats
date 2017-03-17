using System;

namespace BookmarkStats
{
    public class Bookmark : BookmarkBase
    {
        public readonly string Url;
        public readonly DateTime? LastVisited;

        public Bookmark(string name, DateTime dateAdded, string url, DateTime? lastVisited) : base(name, dateAdded)
        {
            Url = url;
            LastVisited = lastVisited;
        }
    }
}
