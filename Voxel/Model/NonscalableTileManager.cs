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
            tile.SmallImagePath = data[nameof(tile.SmallImagePath)].StringValue.FromJsonPath();
            tile.LargeImagePath = data[nameof(tile.LargeImagePath)].StringValue.FromJsonPath();
            tile.ShowName = data[nameof(tile.ShowName)].BooleanValue.Value;
            tile.Background = ((int) data[nameof(tile.Background)].NumberValue).ToColor();
            tile.TargetPath = data[nameof(tile.TargetPath)].StringValue.FromJsonPath();
            string themeString = data[nameof(tile.Theme)].StringValue;
            tile.Theme = themeString == DarkThemeString ? TextTheme.Dark : TextTheme.Light;
        }
        public void LoadFromXml()
        {
            bool relativeExists(string relativePath)
            {
                return File.Exists(getAbsolutePath(relativePath));
            }
            string getAbsolutePath(string relativePath)
            {
                if (relativePath.StartsWith("\\"))
                {
                    relativePath = relativePath.Remove(0, 1);
                }
                string absolutePath = tile.TargetPath.RemoveFileName().Backslash() + relativePath;
                return absolutePath;
            }

            var root = XElement.Load(Tile.XmlPath);
            var visualElements = root.Element("VisualElements");
            tile.Theme = visualElements.Attribute("ForegroundText")?.Value == "dark" ? TextTheme.Dark : TextTheme.Light;
            tile.Background = visualElements.Attribute("BackgroundColor")?.Value.FromHexString()
                ?? Ace.Wpf.DwmEffect.ColorizationColor;
            tile.ShowName = visualElements.Attribute("ShowNameOnSquare150x150Logo")?.Value == "off" ? false : true;

            string largeImagePath = visualElements.Attribute("Square150x150Logo")?.Value;
            if (File.Exists(largeImagePath))
            {
                tile.LargeImagePath = largeImagePath;
            }
            else if (relativeExists(largeImagePath))
            {
                tile.LargeImagePath = getAbsolutePath(largeImagePath);
            }

            string smallImagePath = visualElements.Attribute("Square70x70Logo")?.Value;
            if (File.Exists(smallImagePath))
            {
                tile.SmallImagePath = smallImagePath;
            }
            else if (relativeExists(smallImagePath))
            {
                tile.SmallImagePath = getAbsolutePath(smallImagePath);
            }
        }
        public override void SaveData()
        {
            data = new JsonObject
            {
                [TypeKey] = nameof(NonscalableTile),
                [nameof(tile.SmallImagePath)] = tile.SmallImagePath.ToJsonPath(),
                [nameof(tile.LargeImagePath)] = tile.LargeImagePath.ToJsonPath(),
                [nameof(tile.ShowName)] = tile.ShowName,
                [nameof(tile.Background)] = tile.Background.ToInt32(),
                [nameof(tile.TargetPath)] = tile.TargetPath.ToJsonPath(),
                [nameof(tile.Theme)] = tile.Theme == TextTheme.Dark ? DarkThemeString : LightThemeString,
            };
            base.SaveData();
        }
    }
}
