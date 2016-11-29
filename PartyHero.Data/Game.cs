using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartyHero.Data
{
    public class Game
    {
        /*
        <game name="Activision Decathlon, The (USA)" index="true" image="a">
            <description>Activision Decathlon, The (USA)</description>
            <cloneof></cloneof>
            <crc>F43E7CD0</crc>
            <manufacturer>Activision</manufacturer>
            <year>1984</year>
            <genre>Olympic/Sports</genre>
            <rating>HSRS - GA (General Audience)</rating>
            <enabled>Yes</enabled>
        </game>
        */

        public Game()
        {
            GameTags = new List<GameTag>();
        }

        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Crc { get; set; }
        public string Manufacturer { get; set; }
        public int? Year { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string CloneOf { get; set; }
        public ICollection<GameTag> GameTags { get; set; }
        public bool Enabled { get; set; }
    }
}
