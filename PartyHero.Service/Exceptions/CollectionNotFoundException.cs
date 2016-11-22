using System;

namespace PartyHero.Service.Exceptions
{
    public class CollectionNotFoundException : Exception
    {
        public CollectionNotFoundException(string name)
            : base(string.Format("Collection with name {0} not found.", name))
        {
        }
    }
}
