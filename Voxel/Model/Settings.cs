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
                file.Read();
                Json = file.Content;
            }
        }
        public static void Save()
        {
            JsonFile file = new JsonFile(fileName)
            {
                Content = Json
            };
            file.Flush();
        }
        public static void CreateDefault()
        {
            Json = new JsonObject
            {
                new JsonProperty("ShowSplashScreen", false),
                new JsonProperty(nameof(NonscalableTile), new JsonObject
                {
                    new JsonProperty("AutoLoadXml", false),
                    new JsonProperty("AutoLoadVoxelFile", true),
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
