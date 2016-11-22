using System;

namespace PartyHero.Service.Exceptions
{
    public class TagNotFoundException : Exception
    {
        public TagNotFoundException(string name)
            : base(string.Format("Tag with name {0} not found.", name))
        {
        }
    }
}
