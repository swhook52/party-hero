using System;
using System.Linq;
using PartyHero.Data.Stores;
using PartyHero.Service.Exceptions;

namespace PartyHero.Service
{
    public interface ISystemService
    {
        Data.System GetByName(string name);
        Data.System[] Search(string title);
        Data.System Add(Data.System system);
        bool Exists(string name);
    }

    public class SystemService : ISystemService
    {
        private readonly IDataStore _store;

        public SystemService(IDataStore store)
        {
            _store = store;
        }

        private Data.System GetSystem(string name)
        {
            var system = _store.Systems.SingleOrDefault(p => p.Name == name);
            if (system == null)
                throw new SystemNotFoundException(name);

            return system;
        }

        public Data.System GetByName(string name)
        {
            return GetSystem(name);
        }

        public void RemoveTag(string systemName, string tagName)
        {
            throw new NotImplementedException();
        }

        public Data.System[] Search(string title)
        {
            return _store
                .Systems
                .Where(p => p.Name.Contains(title))
                .ToArray();
        }

        public Data.System Add(Data.System system)
        {
            _store.Systems.Add(system);
            return system;
        }

        public bool Exists(string name)
        {
            var lowercaseName = name.ToLower();
            return _store.Systems.Any(p => p.Name.ToLower() == lowercaseName);
        }
    }
}