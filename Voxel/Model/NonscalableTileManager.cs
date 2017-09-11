using Ace;
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

        public override void Generate()
        {
            if (!File.Exists(tile.TargetPath))
            {
                return;
            }

            XElement root = new XElement("Application",
                new XAttribute(XNamespace.Xmlns + "xsi", @"http://www.w3.org/2001/XMLSchema-instance")
            );
            XElement visualElements = new XElement("VisualElements",
                new XAttribute("ForegroundText", tile.Theme == TextTheme.Dark ? "dark" : "light"),
                new XAttribute("BackgroundColor", tile.Background.ToHexString()),
                new XAttribute("ShowNameOnSquare150x150Logo", tile.ShowName ? "on" : "off")
            );
            
            if (File.Exists(tile.LargeImagePath))
            {
                visualElements.Add(new XAttribute("Square150x150Logo", tile.LargeImagePath.GetFileName()));
                if (/*tile.SmallImagePath == null || */!File.Exists(tile.SmallImagePath))
                {
                    tile.SmallImagePath = tile.LargeImagePath;
                }
                visualElements.Add(new XAttribute("Square70x70Logo", tile.SmallImagePath.GetFileName()));

                string targetFolder = tile.TargetPath.RemoveFileName().ToLower();
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

            root.Add(visualElements);
            root.Save(tile.XmlPath);
        }

        public override void LoadData()
        {
            base.LoadData();
            if (data[TypeKey].StringValue != nameof(NonscalableTile))
            {
                throw new TileTypeNotMatchException();
            }
            tile.SmallImagePath = data[nameof(tile.SmallImagePath)].StringValue;
            tile.LargeImagePath = data[nameof(tile.LargeImagePath)].StringValue;
            tile.ShowName = data[nameof(tile.ShowName)].BooleanValue.Value;
            tile.Background = ((int) data[nameof(tile.Background)].NumberValue).ToColor();
            tile.TargetPath = data[nameof(tile.TargetPath)].StringValue;
            //tile.XmlPath = data[nameof(tile.XmlPath)].StringValue;
            string themeString = data[nameof(tile.Theme)].StringValue;
            tile.Theme = themeString == DarkThemeString ? TextTheme.Dark : TextTheme.Light;
        }
        public override void SaveData()
        {
            data = new JsonObject
            {
                [TypeKey] = nameof(NonscalableTile),
                [nameof(tile.SmallImagePath)] = tile.SmallImagePath,
                [nameof(tile.LargeImagePath)] = tile.LargeImagePath,
                [nameof(tile.ShowName)] = tile.ShowName,
                [nameof(tile.Background)] = tile.Background.ToInt32(),
                [nameof(tile.TargetPath)] = tile.TargetPath,
                //[nameof(tile.XmlPath)] = tile.XmlPath,
                [nameof(tile.Theme)] = tile.Theme == TextTheme.Dark ? DarkThemeString : LightThemeString,
            };
            base.SaveData();
        }
    }
}
