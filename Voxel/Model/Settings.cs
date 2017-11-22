using Ace.Files.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.View;

namespace Voxel.Model
{
    static class Settings
    {
        public static JsonObject Json { get; private set; }
        private const string fileName = "Voxel.settings.json";
        public static void Load()
        {
            JsonFile file = new JsonFile(fileName);
            if (!file.Exists)
            {
                CreateDefault();
            }
            else
            {
                file.Load();
                Json = file.Content;
            }
        }
        public static void Save()
        {
            JsonFile file = new JsonFile(fileName)
            {
                Content = Json
            };
            file.Save();
        }
        public static void CreateDefault()
        {
            Json = new JsonObject
            {
                new JsonProperty("ShowSplashScreen", true),
                new JsonProperty("StartMenu", new JsonObject
                {
                    new JsonProperty(nameof(TileSize.Fullscreen), false),
                    new JsonProperty(nameof(TileSize.ShowMoreTiles), false),
                }),
                new JsonProperty(nameof(NonscalableTile), new JsonObject
                {
                    new JsonProperty("AutoLoadXml", true),
                    new JsonProperty("AutoLoadVoxelFile", false),
                    new JsonProperty("ClearTileCacheOnGenerate", true),
                    new JsonProperty(nameof(ColorPickerView), new JsonObject
                    {
                        new JsonProperty("PreviewOnTile", true),
                        new JsonProperty("ShowSampleText", true),
                    }),
                }),
            };
            Save();
        }
    }
}
