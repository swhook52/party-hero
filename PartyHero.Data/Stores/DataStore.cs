using System.Data.Entity;

namespace PartyHero.Data.Stores
{
    public interface IDataStore
    {
        int SaveChanges();
        IDbSet<Collection> Collections { get; set; }
        IDbSet<Game> Games { get; set; }
        IDbSet<Tag> Tags { get; set; }
        IDbSet<System> Systems { get; set; }
    }

    public class DataStore : DbContext, IDataStore
    {
        public DataStore() : base("PartyHeroData")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataStore>());
            Database.Initialize(false);
        }

        public IDbSet<Collection> Collections { get; set; }
        public IDbSet<Game> Games { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<System> Systems { get; set; }
    }
}
