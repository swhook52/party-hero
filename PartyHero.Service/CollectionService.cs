using PartyHero.Data;
using PartyHero.Service.Exceptions;
using PartyHero.Service.Stores;
using System.Linq;

namespace PartyHero.Service
{
    public interface ICollectionService
    {
        void Add(string name, Tag[] tags);
        void Remove(string name);
        Collection Edit(Collection updatedCollection);
    }

    public class CollectionService : ICollectionService
    {
        private IDataStore _store;

        public CollectionService(IDataStore store)
        {
            _store = store;
        }

        private Collection GetCollection(string name)
        {
            var collections = _store.Collections.FindAll(p => p.Name == name);
            if (!collections.Any())
                throw new CollectionNotFoundException(name);

            return collections.First();
        }

        public void Add(string name, Tag[] tags)
        {
            if (CollectionExists(name))
                throw new CollectionAlreadyExistsException(name);

            var collection = new Collection();
            collection.Name = name;
            collection.Tags = tags;
        }

        public void Remove(string name)
        {
            var collection = GetCollection(name);
            _store.Collections.Remove(collection);
        }

        public Collection Edit(Collection updatedCollection)
        {
            var existingCollection = GetCollection(updatedCollection.Name);
            existingCollection.Name = updatedCollection.Name;
            existingCollection.Tags = updatedCollection.Tags;
            return existingCollection;
        }

        private bool CollectionExists(string name)
        {
            return _store.Collections.FindAll(p => p.Name == name).Any();
        }
    }
}
