using System;

namespace ChromeBookmarks
{
    public abstract class BookmarkBase
    {
        public readonly string Name;
        public readonly DateTime DateAdded;

        public BookmarkBase(string name, DateTime dateAdded)
        {
            Name = name;
            DateAdded = dateAdded;
        }

        public override string ToString()
            => Name;
    }
}
