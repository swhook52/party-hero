using System;
using System.Collections.Generic;

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
            Tags = new List<Tag>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Crc { get; set; }
        public string Manufacturer { get; set; }
        public int? Year { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }
        public string CloneOf { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public bool Enabled { get; set; }
    }
}
