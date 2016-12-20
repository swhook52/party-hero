using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using PartyHero.Data;
using PartyHero.Data.Stores;
using PartyHero.Service;
using PartyHero.Launch;
using PartyHero.Service.Exceptions;

namespace PartyHero.Cli
{
    class Program
    {
        public static IDataStore DataStore;
        public static IGameService GameService;
        public static ITagService TagService;
        public static ILaunchService LaunchService;
        public static ISystemService SystemService;

        static void Main(string[] args)
        {
            DataStore = new DataStore();
            GameService = new GameService(DataStore);
            TagService = new TagService(DataStore);
            LaunchService = new LaunchService(new ShellLauncher());
            SystemService = new SystemService(DataStore);

            Console.WriteLine("Party Hero Command Line Interface");
            Console.WriteLine(" > party help");
            Console.WriteLine(" > party import game [path to xml database]");
            Console.WriteLine(" > party launch [game name] [system name]");

            var command = ReadCommand();
            while (!string.IsNullOrEmpty(command))
            {
                ProcessCommand(command);
                command = ReadCommand();
            }
        }

        private static void ProcessCommand(string command)
        {
            // TODO: Don't split, find the first space instead.
            // test with: party import game "c:\Nintendo Virtual Boy.xml"
            var subcommands = command.Trim().ToUpperInvariant().Split(' ');

            if (subcommands[0].ToUpperInvariant() != "PARTY" || subcommands.Length < 2)
                return;

            var upperCommand = subcommands[1].ToUpperInvariant();
            switch (upperCommand)
            {
                case "IMPORT":
                    ParseImport(subcommands.Skip(2).ToArray());
                    return;
                case "LAUNCH":
                    ParseLaunchGame(subcommands.Skip(2).ToArray());
                    return;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    return;
            }
        }

        private static void ParseLaunchGame(string[] subcommands)
        {
            if (subcommands.Length < 2)
            {
                ImportHelp();
                return;
            }

            var gameName = subcommands[0];
            var systemName = subcommands[1];
            LaunchService.LaunchGame(gameName, systemName);
        }

        private static void ParseImport(string[] subcommands)
        {
            if (subcommands.Length == 0)
            {
                ImportHelp();
                return;
            }

            var importType = subcommands[0].ToUpperInvariant();
            switch (importType)
            {
                case "GAME":
                    ParseImportGame(subcommands.Skip(1).ToArray());
                    return;
                default:
                    ImportHelp();
                    return;
            }
        }

        private static void ParseImportGame(string[] subcommands)
        {
            if (subcommands.Length == 0)
            {
                ImportHelp();
                return;
            }

            ImportGames(subcommands[0]);
        }

        private static void ImportHelp()
        {
            Console.WriteLine("import usage:");
            Console.WriteLine("import [import type] [filepath]");
            Console.WriteLine("EXAMPLE: import game c:\\Arcade\\games.xml");
        }

        private static string ReadCommand()
        {
            Console.WriteLine();
            Console.Write("> ");
            return Console.ReadLine();
        }

        private static bool CreateGame(Data.System system, XElement gameElement)
        {
            var gameName = gameElement.Attribute("name")?.Value;
            if (GameService.Exists(gameName))
                return false;

            Console.WriteLine($"Importing {gameName}");

            var genre = gameElement.Element("genre")?.Value;
            var manufacturer = gameElement.Element("manufacturer")?.Value;
            var rating = gameElement.Element("rating")?.Value;

            var game = new Game
            {
                Name = gameName,
                Description = gameElement.Element("description")?.Value,
                Crc = gameElement.Element("crc")?.Value,
                Manufacturer = manufacturer,
                Genre = genre,
                Rating = gameElement.Element("rating")?.Value,
                CloneOf = gameElement.Element("cloneof")?.Value,
                Enabled = gameElement.Element("enabled")?.Value.ToUpper() == "YES",
                System = system
            };

            int year;
            if (int.TryParse(gameElement.Element("year")?.Value, out year))
            {
                game.Year = year;
                AddTag(game, year.ToString());
            }

            AddTag(game, system.Name);
            AddTag(game, genre);
            AddTag(game, manufacturer);
            AddTag(game, rating);

            GameService.Add(game);
            return true;
        }

        private static void AddTag(Game game, string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
                return;

            TagService.Add(game, tagName);
        }

        private static void ImportGames(string databasePath)
        {
            try
            {
                var document = GetDatabaseDocument(databasePath);
                var system = CreateSystemFromDatabase(document);
                if (system == null)
                    return;

                CreateGamesFromDatabase(system, document);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void CreateGamesFromDatabase(Data.System system, XDocument document)
        {
            var gameElements = document.Descendants("game").ToList();
            if (gameElements.Count > 0)
            {
                var gamesImported = 0;
                var duplicateGames = 0;
                foreach (var gameElement in gameElements)
                {
                    if (CreateGame(system, gameElement))
                    {
                        gamesImported++;
                    }
                    else
                    {
                        duplicateGames++;
                    }
                    DataStore.SaveChanges();
                }
                Console.WriteLine($"{gameElements.Count} games found. {gamesImported} games imported. {duplicateGames} duplicate games skipped.");
            }
            else
            {
                Console.WriteLine("No games were found in the XML Database.");
            }

        }

        private static Data.System CreateSystemFromDatabase(XDocument document)
        {
            var header = document.Descendants("header").ToList();
            if (header.Count == 0)
            {
                Console.WriteLine("System name not found in database.");
                return null;
            }

            var systemName = header[0].Element("listname").Value;
            try
            {
                var system = SystemService.GetByName(systemName);
                Console.WriteLine("System already exists");
                return system;
            }
            catch (SystemNotFoundException)
            {
                Console.WriteLine($"Importing {systemName}");
                var system = new Data.System
                {
                    Name = systemName
                };

                DataStore.Systems.Add(system);
                DataStore.SaveChanges();

                return SystemService.GetByName(systemName);
            }
        }

        private static XDocument GetDatabaseDocument(string databasePath)
        {
            if (string.IsNullOrEmpty(databasePath))
                throw new FileNotFoundException("Invalid file path");

            var path = databasePath.Trim();

            if (!File.Exists(path))
                throw new FileNotFoundException("Enter a valid path to the XML database file that contains all the games for a particular emulator.");

            return XDocument.Load(path);
        }
    }
}
