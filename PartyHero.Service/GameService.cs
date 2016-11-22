using PartyHero.Data;
using PartyHero.Service.Exceptions;
using PartyHero.Service.Stores;
using System;
using System.Linq;

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
            var games = _store.Games.FindAll(p => p.Name == name);
            if (!games.Any())
                throw new GameNotFoundException(name);

            return games.First();
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
            return _store.Games.FindAll(p => p.Name.Contains(title) || p.Description.Contains(title)).ToArray();
        }

        public Game Add(Game game)
        {
            _store.Games.Add(game);
            return game;
        }

        public bool Exists(string name)
        {
            var lowercaseName = name.ToLower();
            return _store.Games.FindAll(p => p.Name.ToLower() == lowercaseName).Any();
        }
    }
}
