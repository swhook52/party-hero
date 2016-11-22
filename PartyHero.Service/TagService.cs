using PartyHero.Data;
using System.Collections.Generic;
using PartyHero.Service.Exceptions;
using PartyHero.Service.Stores;
using System.Linq;

namespace PartyHero.Service
{
    public interface ITagService
    {
        Tag GetOrCreate(string name);
        void Add(Game game, string tagName);
        void Remove(Game game, string tagName);
    }

    public class TagService : ITagService
    {
        private IDataStore _store;

        public TagService(IDataStore store)
        {
            _store = store;
        }

        private Tag GetTag(string name)
        {
            var tags = _store.Tags.FindAll(p => p.Name == name);
            if (!tags.Any())
                throw new TagNotFoundException(name);

            return tags.First();
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

            if (!game.Tags.Contains(tag))
                game.Tags.Add(tag);
        }

        private Tag CreateTag(string name)
        {
            var tag = new Tag { Name = name };
            _store.Tags.Add(tag);

            return tag;
        }

        public void Remove(Game game, string tagName)
        {
            var newTags = new List<Tag>();
            foreach (var tag in game.Tags)
            {
                if (tag.Name != tagName)
                {
                    newTags.Add(tag);
                }
            }

            game.Tags = newTags;
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
