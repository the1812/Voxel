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
            var file = new JsonFile(fileName);
            if (!file.Exists)
            {
                CreateDefault();
            }
            else
            {
                try
                {
                    file.Load();
                    Json = file.ObjectContent;
                }
                catch (JsonParseException)
                {
                    CreateDefault();
                }
            }
        }
        public static void Save()
        {
            var file = new JsonFile(fileName)
            {
                Content = Json
            };
            file.Save();
        }
        private static JsonValue getValue(string keyPath)
        {
            var path = keyPath.Split('.');
            var lastIndex = path.Length - 1;
            var jsonObject = Json;
            for (var index = 0; index < path.Length - 1/* ignore last index */; index++)
            {
                var name = path[index];
                if (jsonObject.ContainsName(name))
                {
                    jsonObject = jsonObject[name].ObjectValue;
                }
                else
                {
                    SetValue(keyPath, defaultValues[keyPath]);
                    return defaultValues[keyPath];
                }
            }
            var lastName = path[lastIndex];
            if (jsonObject.ContainsName(lastName))
            {
                return jsonObject[path[lastIndex]];
            }
            else
            {
                SetValue(keyPath, defaultValues[keyPath]);
                return defaultValues[keyPath];
            }
        }
        public static bool GetBoolean(string keyPath) => (bool) getValue(keyPath);
        public static string GetString(string keyPath) => (string) getValue(keyPath);
        public static decimal GetNumber(string keyPath) => (decimal) getValue(keyPath);
        public static void SetValue(string keyPath, JsonValue jsonValue)
        {
            var path = keyPath.Split('.');
            var lastIndex = path.Length - 1;
            var jsonObject = Json;
            for (var index = 0; index < path.Length - 1/* ignore last index */; index++)
            {
                var name = path[index];
                if (jsonObject.ContainsName(name))
                {
                    jsonObject = jsonObject[name].ObjectValue;
                }
                else
                {
                    var newObject = new JsonObject();
                    jsonObject[name] = newObject;
                    jsonObject = newObject;
                }
            }
            jsonObject[path[lastIndex]] = jsonValue;
        }
        private static Dictionary<string, JsonValue> defaultValues = new Dictionary<string, JsonValue>
        {
            {ShowSplashScreenKey, true },
            {MakeKey(StartMenuKey, nameof(TileSize.Fullscreen)), false },
            {MakeKey(StartMenuKey, nameof(TileSize.ShowMoreTiles)), false },
            {MakeKey(nameof(NonscalableTile), AutoLoadXmlKey), true },
            {MakeKey(nameof(NonscalableTile), AutoLoadVoxelFileKey), false },
            {MakeKey(nameof(NonscalableTile), ClearTileCacheOnGenerateKey), true },
            {MakeKey(nameof(NonscalableTile), nameof(ColorPickerView), PreviewOnTileKey), true },
            {MakeKey(nameof(NonscalableTile), nameof(ColorPickerView), RgbModeKey), false },
        };
        public static string MakeKey(params string[] keys) => string.Join(".", keys);
        public static void CreateDefault()
        {
            Json = new JsonObject();
            foreach (var pair in defaultValues)
            {
                SetValue(pair.Key, pair.Value);
            }
            Save();
        }
        public const string ShowSplashScreenKey = "ShowSplashScreen";
        public const string StartMenuKey = "StartMenu";
        public const string AutoLoadXmlKey = "AutoLoadXml";
        public const string AutoLoadVoxelFileKey = "AutoLoadVoxelFile";
        public const string ClearTileCacheOnGenerateKey = "ClearTileCacheOnGenerate";
        public const string PreviewOnTileKey = "PreviewOnTile";
        //public const string ShowSampleTextKey = "ShowSampleText";
        public const string RgbModeKey = "RgbMode";
    }
}
