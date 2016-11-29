using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyHero.Data
{
    [Table("GameTags")]
    public class GameTag
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Game")]
        public string GameName { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Tag")]
        public string TagName { get; set; }

        public Game Game { get; set; }
        public Tag Tag { get; set; }
    }
}
