using System;

namespace PartyHero.Service.Exceptions
{
    public class SystemNotFoundException : Exception
    {
        public SystemNotFoundException(string name)
            : base(string.Format("System with name {0} not found.", name))
        {
        }
    }
}
