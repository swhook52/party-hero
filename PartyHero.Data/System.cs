using System.ComponentModel.DataAnnotations;

namespace PartyHero.Data
{
    public class System
    {
        /*
        <header>
		    <listname>Nintendo Virtual Boy</listname>
		    <lastlistupdate>03/06/2016</lastlistupdate>
		    <listversion>1.2</listversion>
		    <exporterversion>HyperList XML Exporter Version 1.3 Copywrite (c) 2009-2011 William Strong</exporterversion>
	    </header>
        */
        public System()
        {

        }

        [Key]
        public string Name { get; set; }
    }
}
