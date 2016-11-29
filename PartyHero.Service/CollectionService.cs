using PartyHero.Data;
using PartyHero.Service.Exceptions;
using System.Linq;
using PartyHero.Data.Stores;

namespace PartyHero.Service
{
    public interface ICollectionService
    {
        void Add(string name, string[] tags);
        void Remove(string name);
        Collection Edit(Collection updatedCollection);
    }

    public class CollectionService : ICollectionService
    {
        private readonly IDataStore _store;
        private readonly ITagService _tagService;

        public CollectionService(IDataStore store, ITagService tagService)
        {
            _store = store;
            _tagService = tagService;
        }

        private Collection GetCollection(string name)
        {
            var collection = _store.Collections.SingleOrDefault(p => p.Name == name);
            if (collection == null)
                throw new CollectionNotFoundException(name);

            return collection;
        }

        public void Add(string name, string[] tagNames)
        {
            if (CollectionExists(name))
                throw new CollectionAlreadyExistsException(name);

            var collection = new Collection
            {
                Name = name
            };

            foreach (var tag in tagNames)
            {
                _tagService.Add(collection, tag);
            }
            _store.Collections.Add(collection);
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
            existingCollection.CollectionTags = updatedCollection.CollectionTags;
            return existingCollection;
        }

        private bool CollectionExists(string name)
        {
            return _store.Collections.Any(p => p.Name == name);
        }
    }
}
