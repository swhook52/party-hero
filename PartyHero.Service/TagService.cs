using PartyHero.Data;
using PartyHero.Service.Exceptions;
using System.Linq;
using PartyHero.Data.Stores;

namespace PartyHero.Service
{
    public interface ITagService
    {
        Tag GetOrCreate(string name);
        void Add(Game game, string tagName);
        void Add(Collection collection, string tagName);
        void Remove(Game game, string tagName);
    }

    public class TagService : ITagService
    {
        private readonly IDataStore _store;

        public TagService(IDataStore store)
        {
            _store = store;
        }

        private Tag GetTag(string name)
        {
            var lowercaseName = name.ToLower();
            var tag = _store.Tags.SingleOrDefault(p => p.Name.ToLower() == lowercaseName);
            if (tag == null)
                throw new TagNotFoundException(name);

            return tag;
        }

        public void Add(Game game, string tagName)
        {
            Tag tag;
            try
            {
                tag = GetTag(tagName);
            }
            catch (TagNotFoundException)
            {
                tag = CreateTag(tagName);
            }

            if (game.GameTags.All(p => p.TagName != tag.Name))
                game.GameTags.Add(new GameTag { Game = game, Tag = tag });
        }

        public void Add(Collection collection, string tagName)
        {
            Tag tag;
            try
            {
                tag = GetTag(tagName);
            }
            catch (TagNotFoundException)
            {
                tag = CreateTag(tagName);
            }

            if (collection.CollectionTags.All(p => p.TagName != tag.Name))
                collection.CollectionTags.Add(new CollectionTag { Collection = collection, Tag = tag });
        }

        private Tag CreateTag(string name)
        {
            var tag = _store.Tags.Create();
            tag.Name = name;
            _store.Tags.Add(tag);

            return tag;
        }

        public void Remove(Game game, string tagName)
        {
            var uppercaseTagName = tagName.ToUpper();
            var existingGameTag = game.GameTags.FirstOrDefault(p => p.TagName.ToUpper() == uppercaseTagName);

            if (existingGameTag != null)
                game.GameTags.Remove(existingGameTag);
        }

        public Tag GetOrCreate(string name)
        {
            try
            {
                return GetTag(name);
            }
            catch (TagNotFoundException)
            {
                return CreateTag(name);
            }
            
        }
    }
}
