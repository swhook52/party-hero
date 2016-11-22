using System;

namespace PartyHero.Service.Exceptions
{
    public class CollectionAlreadyExistsException : Exception
    {
        public CollectionAlreadyExistsException(string name)
            : base(string.Format("Collection with name {0} already exists.", name))
        {
        }
    }
}
