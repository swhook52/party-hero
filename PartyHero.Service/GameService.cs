using PartyHero.Data;
using PartyHero.Service.Exceptions;
using System;
using System.Linq;
using PartyHero.Data.Stores;

namespace PartyHero.Service
{
    public interface IGameService
    {
        Game GetByName(string name);
        Game[] Search(string title);
        Game Add(Game game);
        bool Exists(string name);
    }

    public class GameService : IGameService
    {
        private readonly IDataStore _store;

        public GameService(IDataStore store)
        {
            _store = store;
        }

        private Game GetGame(string name)
        {
            var game = _store.Games.SingleOrDefault(p => p.Name == name);
            if (game == null)
                throw new GameNotFoundException(name);

            return game;
        }

        public Game GetByName(string name)
        {
            return GetGame(name);
        }

        public void RemoveTag(string gameName, string tagName)
        {
            throw new NotImplementedException();
        }

        public Game[] Search(string title)
        {
            return _store
                .Games
                .Where(p => p.Name.Contains(title) || p.Description.Contains(title))
                .ToArray();
        }

        public Game Add(Game game)
        {
            _store.Games.Add(game);
            return game;
        }

        public bool Exists(string name)
        {
            var lowercaseName = name.ToLower();
            return _store.Games.Any(p => p.Name.ToLower() == lowercaseName);
        }
    }
}
