using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartyHero.Data
{
    public class Collection
    {
        public Collection()
        {
            CollectionTags = new List<CollectionTag>();
        }

        [Key]
        public string Name { get; set; }
        public ICollection<CollectionTag> CollectionTags { get; set; }
    }
}
