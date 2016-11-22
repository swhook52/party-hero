using System.Collections.Generic;

namespace PartyHero.Data
{
    public class Collection
    {
        public string Name { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
