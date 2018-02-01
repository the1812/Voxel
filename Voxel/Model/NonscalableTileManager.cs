using Ace;
using Ace.Files;
using Ace.Files.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using static Voxel.Model.Settings;

namespace Voxel.Model
{
    sealed class NonscalableTileManager : TileManager
    {

        private NonscalableTile tile = new NonscalableTile()
        {
            LargeImagePath = null,
            SmallImagePath = null,
            Background = Ace.Wpf.DwmEffect.ColorizationColor,
            Theme = Ace.Wpf.DwmEffect.ForegroundColor == Colors.Black ? TextTheme.Dark : TextTheme.Light,
            ShowName = true,
        };
        public NonscalableTile Tile => tile;

        public override void AddToStart()
        {
            var file = new ShortcutFile(Tile.StartMenuTargetPath)
            {
                TargetPath = tile.TargetPath,
                WorkingDirectory = tile.TargetPath.GetParentFolder(),
            };
            file.Save();
        }

        public override void Generate()
        {
            if (!tile.TargetExists)
            {
                return;
            }
            var xml = new XmlManager();
            xml.FillFrom(tile);
            if (File.Exists(tile.LargeImagePath))
            {
                var targetFolder = tile.TargetType == TargetType.File ?
                    tile.TargetPath.RemoveFileName().ToLower() :
                    tile.TargetPath.GetParentFolder().ToLower();
                if (targetFolder != tile.LargeImagePath.RemoveFileName().ToLower())
                {
                    File.Copy(tile.LargeImagePath, targetFolder.Backslash() + tile.LargeImagePath.GetFileName(), true);
                }
                if (tile.SmallImagePath != tile.LargeImagePath &&
                    targetFolder != tile.SmallImagePath.RemoveFileName().ToLower())
                {
                    File.Copy(tile.SmallImagePath, targetFolder.Backslash() + tile.SmallImagePath.GetFileName(), true);
                }
            }
            xml.Save(tile.XmlPath);

            var clearCache = GetBoolean(MakeKey(nameof(NonscalableTile), ClearTileCacheOnGenerateKey));
            if (clearCache)
            {
                RefreshShortcut();
            }
        }
        public override void RefreshShortcut()
        {
            var filter = new Func<FileInfo, bool>(file =>
            {
                if (file.Extension != ".lnk")
                {
                    return false;
                }
                var shortcutFile = new ShortcutFile(file.FullName);
                shortcutFile.Load();
                return tile.TargetPath.GetFileName().ToLower() == shortcutFile.TargetPath.GetFileName().ToLower();
            });
            ViewModel.MainViewModel.ClearTileCache(filter);
        }

        public override void LoadData()
        {
            base.LoadData();
            if (data[TypeKey].StringValue != nameof(NonscalableTile))
            {
                throw new TileTypeNotMatchException();
            }
            try
            {
                tile.SmallImagePath = data[nameof(tile.SmallImagePath)].StringValue;
                tile.LargeImagePath = data[nameof(tile.LargeImagePath)].StringValue;
                tile.ShowName = data[nameof(tile.ShowName)].BooleanValue.Value;
                var background = data[nameof(tile.Background)];
                if (background.Type == JsonValue.DataType.Number) // Compatibility for old versions
                {
                    tile.Background = ((int) data[nameof(tile.Background)].NumberValue.Value).ToColor();
                }
                else
                {
                    tile.Background = data[nameof(tile.Background)].StringValue.FromHexString();
                }
                tile.TargetPath = data[nameof(tile.TargetPath)].StringValue;
                var themeString = data[nameof(tile.Theme)].StringValue;
                tile.Theme = themeString == DarkThemeString ? TextTheme.Dark : TextTheme.Light;
                var targetTypeString = data[nameof(tile.TargetType)].StringValue;
                tile.TargetType = targetTypeString == FolderTargetString ? TargetType.Folder : TargetType.File;
            }
            catch (KeyNotFoundException)
            {
                throw new BadVoxelFileException();
            }
        }
        public void LoadFromXml()
        {
            var xml = new XmlManager();
            xml.Load(tile.XmlPath);
            xml.FillTo(ref tile);
        }
        public override void SaveData()
        {
            data = new JsonObject
            {
                [TypeKey] = nameof(NonscalableTile),
                [nameof(tile.SmallImagePath)] = tile.SmallImagePath ?? new JsonValue(),
                [nameof(tile.LargeImagePath)] = tile.LargeImagePath ?? new JsonValue(),
                [nameof(tile.ShowName)] = tile.ShowName,
                [nameof(tile.Background)] = tile.Background.ToHexString(),
                [nameof(tile.TargetPath)] = tile.TargetPath,
                [nameof(tile.Theme)] = tile.Theme == TextTheme.Dark ? DarkThemeString : LightThemeString,
                [nameof(tile.TargetType)] = tile.TargetType == TargetType.File ? FileTargetString : FolderTargetString,
            };
            base.SaveData();
        }
    }
}
