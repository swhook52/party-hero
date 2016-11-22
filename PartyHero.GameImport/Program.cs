using PartyHero.Data;
using PartyHero.Service;
using PartyHero.Service.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PartyHero.GameImport
{
    class Program
    {
        public static GameService GameService;
        public static TagService TagService;

        static void Main(string[] args)
        {
            var dataStore = new DataStore();
            GameService = new GameService(dataStore);
            TagService = new TagService(dataStore);

            var gameElements = GetGamesFromFile();
            if (gameElements.Count > 0)
            {
                foreach (var gameElement in gameElements)
                {
                    CreateGame(gameElement);
                }
                Console.WriteLine($"{gameElements.Count} games imported.");
            }
            else
            {
                Console.WriteLine("No games were found in the XML Database.");
            }

            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.Read();
        }

        private static void CreateGame(XElement gameElement)
        {
            var gameName = gameElement.Attribute("name")?.Value;
            if (GameService.Exists(gameName))
                return;

            Console.WriteLine($"Importing {gameName}");

            var genre = gameElement.Element("genre")?.Value;

            var game = new Game
            {
                Name = gameName,
                Description = gameElement.Element("description")?.Value,
                Crc = gameElement.Element("crc")?.Value,
                Manufacturer = gameElement.Element("manufacturer")?.Value,
                Genre = genre,
                Rating = gameElement.Element("rating")?.Value,
                CloneOf = gameElement.Element("cloneof")?.Value,
                Enabled = gameElement.Element("enabled")?.Value.ToUpper() == "YES"
            };

            int year;
            if (int.TryParse(gameElement.Element("year")?.Value, out year))
            {
                game.Year = year;
            }

            AddTag(game, genre);
            GameService.Add(game);
        }

        private static void AddTag(Game game, string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
                return;

            var tag = TagService.GetOrCreate(tagName);
            game.Tags.Add(tag);
        }

        private static List<XElement> GetGamesFromFile()
        {
            Console.WriteLine("Enter the path, filename and extension of the XML database file to import into Party Hero games.");
            var databasePath = Console.ReadLine();
            if (string.IsNullOrEmpty(databasePath))
                throw new FileNotFoundException("Invalid file path");

            var path = databasePath.Trim();

            if (!File.Exists(path))
                throw new FileNotFoundException("Enter a valid path to the XML database file that contains all the games for a particular emulator.");

            var document = XDocument.Load(path);
            return document.Descendants("game").ToList();
        }
    }
}
