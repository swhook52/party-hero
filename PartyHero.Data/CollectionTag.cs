using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyHero.Data
{
    [Table("CollectionTags")]
    public class CollectionTag
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Collection")]
        public string CollectionName { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Tag")]
        public string TagName { get; set; }

        public Collection Collection { get; set; }
        public Tag Tag { get; set; }

    }
}