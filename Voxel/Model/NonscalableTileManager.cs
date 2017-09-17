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
        public NonscalableTile Tile
        {
            get => tile;
            //set
            //{
            //    tile = value;
            //    OnPropertyChanged(nameof(Tile));
            //}
        }

        public override void AddToStart()
        {
            ShortcutFile file = new ShortcutFile(Tile.StartMenuTargetPath)
            {
                TargetPath = tile.TargetPath,
            };
            file.Flush();
        }

        public override void Generate()
        {
            if (!tile.TargetExists)
            {
                return;
            }
            XmlManager xml = new XmlManager();
            xml.FillFrom(tile);
            if (File.Exists(tile.LargeImagePath))
            {
                string targetFolder = tile.TargetType == TargetType.File ?
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
                tile.SmallImagePath = data[nameof(tile.SmallImagePath)].StringValue?.FromJsonPath();
                tile.LargeImagePath = data[nameof(tile.LargeImagePath)].StringValue?.FromJsonPath();
                tile.ShowName = data[nameof(tile.ShowName)].BooleanValue.Value;
                tile.Background = ((int) data[nameof(tile.Background)].NumberValue).ToColor();
                tile.TargetPath = data[nameof(tile.TargetPath)].StringValue.FromJsonPath();
                string themeString = data[nameof(tile.Theme)].StringValue;
                tile.Theme = themeString == DarkThemeString ? TextTheme.Dark : TextTheme.Light;
                string targetTypeString = data[nameof(tile.TargetType)].StringValue;
                tile.TargetType = targetTypeString == FolderTargetString ? TargetType.Folder : TargetType.File;
            }
            catch (KeyNotFoundException)
            {
                throw new BadVoxelFileException();
            }
        }
        public void LoadFromXml()
        {
            XmlManager xml = new XmlManager();
            xml.Load(tile.XmlPath);
            xml.FillTo(ref tile);
        }
        public override void SaveData()
        {
            data = new JsonObject
            {
                [TypeKey] = nameof(NonscalableTile),
                [nameof(tile.SmallImagePath)] = tile.SmallImagePath?.ToJsonPath() ?? new JsonValue(),
                [nameof(tile.LargeImagePath)] = tile.LargeImagePath?.ToJsonPath() ?? new JsonValue(),
                [nameof(tile.ShowName)] = tile.ShowName,
                [nameof(tile.Background)] = tile.Background.ToInt32(),
                [nameof(tile.TargetPath)] = tile.TargetPath.ToJsonPath(),
                [nameof(tile.Theme)] = tile.Theme == TextTheme.Dark ? DarkThemeString : LightThemeString,
                [nameof(tile.TargetType)] = tile.TargetType == TargetType.File ? FileTargetString : FolderTargetString,
            };
            base.SaveData();
        }
    }
}
