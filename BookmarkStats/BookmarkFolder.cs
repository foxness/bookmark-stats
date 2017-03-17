using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BookmarkStats
{
    public class BookmarkFolder : BookmarkBase, IReadOnlyList<BookmarkBase>
    {
        public readonly DateTime LastModified;
        private readonly List<BookmarkBase> collection;

        public int Count => collection.Count;

        public BookmarkBase this[int i] => collection[i];

        public BookmarkFolder(string name, DateTime dateAdded, DateTime lastModified, List<BookmarkBase> bookmarkBases) : base(name, dateAdded)
        {
            LastModified = lastModified;
            collection = bookmarkBases;
        }

        public BookmarkFolder Flattened()
        {
            var flattened = new List<BookmarkBase>();
            foreach (var bookmarkBase in collection)
            {
                if (bookmarkBase is Bookmark)
                    flattened.Add(bookmarkBase);
                else
                {
                    foreach (BookmarkBase bookmark in ((BookmarkFolder)bookmarkBase).Flattened())
                        flattened.Add(bookmark);
                }
            }

            return new BookmarkFolder(Name, DateAdded, LastModified, flattened);
        }

        public static explicit operator List<Bookmark>(BookmarkFolder a)
            => a.Flattened().Select(bookmark => (Bookmark)bookmark).ToList();

        public IEnumerator GetEnumerator() => collection.GetEnumerator();

        IEnumerator<BookmarkBase> IEnumerable<BookmarkBase>.GetEnumerator() => collection.GetEnumerator();
    }
}
