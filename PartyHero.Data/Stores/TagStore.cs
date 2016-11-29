using System;
using System.Collections.Generic;
using System.Linq;

namespace PartyHero.Data.Stores
{
    public interface ITagStore
    {
        void Add(Tag item);
        bool Remove(Tag item);
        IQueryable<Tag> FindAll(Predicate<Tag> predicate);
    }

    public class TagStore : ITagStore
    {
        private List<Tag> _tags;

        public TagStore()
        {
            _tags = new List<Tag>();
        }

        public void Add(Tag item)
        {
            _tags.Add(item);
        }

        public bool Remove(Tag item)
        {
            return _tags.Remove(item);
        }

        public IQueryable<Tag> FindAll(Predicate<Tag> predicate)
        {
            return _tags.FindAll(predicate).AsQueryable();
        }
    }
}