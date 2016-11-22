using PartyHero.Service.Stores;

namespace PartyHero.Service.Stores
{
    public interface IDataStore
    {
        IGameStore Games { get;  }
        ICollectionStore Collections { get; }
        ITagStore Tags { get; }
    }

    public class DataStore : IDataStore
    {
        private ICollectionStore _collections;
        public ICollectionStore Collections
        {
            get
            {
                if (_collections == null)
                    _collections = new CollectionStore();

                return _collections;
            }
        }

        private IGameStore _games;
        public IGameStore Games
        {
            get
            {
                if (_games == null)
                    _games = new GameStore();

                return _games;
            }
        }

        private ITagStore _tags;
        public ITagStore Tags
        {
            get
            {
                if (_tags == null)
                    _tags = new TagStore();

                return _tags;
            }
        }
    }
}
