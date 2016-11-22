using System;

namespace PartyHero.Service.Exceptions
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(string name)
            : base(string.Format("Game with name {0} not found.", name))
        {
        }
    }
}
