using PartyHero.Data;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PartyHero.Service.Stores
{
    public interface IGameStore
    {
        void Add(Game item);
        bool Remove(Game item);
        IQueryable<Game> FindAll(Predicate<Game> predicate);
    }

    public class GameStore : IGameStore
    {
        private List<Game> _games;

        public GameStore()
        {
            _games = new List<Game>();
            LoadGames();
        }

        private void LoadGames()
        {
            
        }

        public void Add(Game item)
        {
            _games.Add(item);
        }

        public bool Remove(Game item)
        {
            return _games.Remove(item);
        }

        public IQueryable<Game> FindAll(Predicate<Game> predicate)
        {
            return _games.FindAll(predicate).AsQueryable();
        }
    }
}