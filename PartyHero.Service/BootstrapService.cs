using Newtonsoft.Json;
using PartyHero.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PartyHero.Service
{
    public class BootstrapService
    {
        private string _collectionsPath;
        private string _collectionConfigName;
        private string[] _gameManifestPaths;
        private readonly ICollectionService _collectionService;
        private readonly ITagService _tagService;
        private readonly IGameService _gameService;
        private string _tagConfigFilePath;

        public BootstrapService(
            ICollectionService collectionService,
            ITagService tagService,
            IGameService gameService)
        {
            _collectionService = collectionService;
            _tagService = tagService;
            _gameService = gameService;
            _collectionsPath = @"D:\PartyHeroTest\Collections\";
            _collectionConfigName = @"collection.config";
            _gameManifestPaths = new[] { @"D:\PartyHeroTest\Games\System1", @"D:\PartyHeroTest\Games\System2" };
            _tagConfigFilePath = @"D:\PartyHeroTest\tags.config";
        }

        /// <summary>
        /// Finds all collections, games and tags in the configured locations.
        /// </summary>
        public void Update()
        {
            CreateGames();
            //CreateTags();
            CreateCollections();
        }

        private void CreateGamesFromManifest(string manifestPath)
        {
            var document = XDocument.Load(manifestPath);
            var gameNodes = document.Descendants("game");
            if (!gameNodes.Any())
                return;

            foreach (var gameNode in gameNodes)
            {
                var game = new Game();
                game.Name = gameNode.Attribute("name").Value;
                game.Description = gameNode.Element("description").Value;
                game.CloneOf = gameNode.Element("cloneof").Value;
                game.Crc = gameNode.Element("crc").Value;
                game.Manufacturer = gameNode.Element("manufacturer").Value;
                game.Year = int.Parse(gameNode.Element("year").Value);
                game.Genre = gameNode.Element("genre").Value;
                game.Rating = gameNode.Element("rating").Value;
                game.Enabled = bool.Parse(gameNode.Element("enabled").Value);

                _gameService.Add(game);
            }
        }
        
        private void CreateGames()
        {
            foreach (var manifestPath in _gameManifestPaths)
            {
                if (!Directory.Exists(manifestPath))
                    throw new DirectoryNotFoundException($"Game manifest directory \"{manifestPath}\" not found.");

                var fileNames = Directory.EnumerateFiles(manifestPath, "*.xml");
                foreach (var fileName in fileNames)
                {

                }
            }

        }

        private void CreateCollections()
        {
            if (!Directory.Exists(_collectionsPath))
                throw new DirectoryNotFoundException($"Collections directory \"{_collectionsPath}\" not found.");

            var folderNames = Directory.EnumerateDirectories(_collectionsPath);
            foreach (var folderName in folderNames)
            {
                var configFilePath = Path.Combine(folderName, _collectionConfigName);
                if (File.Exists(configFilePath))
                {
                    // This is a virtual collection that has a configuration file
                    var config = GetCollectionConfig(configFilePath);
                    var tags = GetTagsFromSearch(config.TagSearch);
                    var folderInfo = new DirectoryInfo(folderName);

                    _collectionService.Add(folderInfo.Name, tags);
                }
                else
                {
                    // This is a physical collection that has folders for the content

                }
            }
        }

        private string[] GetTagsFromSearch(string tagSearch)
        {
            return tagSearch.Split(',');
            /*
            var tags = new List<Tag>();
            var tagNames = tagSearch.Split(',');
            foreach (var tagName in tagNames)
            {
                if (string.IsNullOrWhiteSpace(tagName))
                    continue;

                tags.Add(_tagService.GetOrCreate(tagName));
            }

            return tags.ToArray();
            */
        }

        private CollectionConfig GetCollectionConfig(string configFilePath)
        {
            var json = File.ReadAllText(configFilePath);
            return JsonConvert.DeserializeObject<CollectionConfig>(json);
        }
    }
}
