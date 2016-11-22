using PartyHero.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartyHero.Service.Stores
{
    public interface ICollectionStore
    {
        void Add(Collection item);
        bool Remove(Collection item);
        IQueryable<Collection> FindAll(Predicate<Collection> predicate);
    }

    public class CollectionStore : ICollectionStore
    {
        private List<Collection> _collections;

        public CollectionStore()
        {
            _collections = new List<Collection>();
        }

        public void Add(Collection item)
        {
            _collections.Add(item);
        }

        public bool Remove(Collection item)
        {
            return _collections.Remove(item);
        }

        public IQueryable<Collection> FindAll(Predicate<Collection> predicate)
        {
            return _collections.FindAll(predicate).AsQueryable();
        }
    }
}