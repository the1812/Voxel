using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ace;
using System.IO;

namespace Voxel.Model
{
    sealed class XmlManager
    {
        private XElement root, visualElements;
        public XmlManager()
        {
            root = new XElement("Application",
                new XAttribute(XNamespace.Xmlns + "xsi", @"http://www.w3.org/2001/XMLSchema-instance")
            );
            visualElements = new XElement("VisualElements",
                new XAttribute(nameof(ForegroundText), TileManager.LightThemeString),
                new XAttribute(nameof(BackgroundColor), Ace.Wpf.DwmEffect.ColorizationColor.ToInt32()),
                new XAttribute(nameof(ShowNameOnSquare150x150Logo), "on"),
                new XAttribute(nameof(Square150x150Logo), ""),
                new XAttribute(nameof(Square70x70Logo), "")
            );
            root.Add(visualElements);
        }
        public void Load(string fileName)
        {
            root = XElement.Load(fileName);
            visualElements = root.Element("VisualElements");
        }
        public void Save(string fileName)
        {
            root.Save(fileName);
        }
        public void FillFrom(Tile tile)
        {
            ForegroundText = tile.Theme;
            BackgroundColor = tile.Background;
            ShowNameOnSquare150x150Logo = tile.ShowName;
            if (File.Exists(tile.LargeImagePath))
            {
                Square150x150Logo = tile.LargeImagePath.GetFileName();
                if (!File.Exists(tile.SmallImagePath))
                {
                    tile.SmallImagePath = tile.LargeImagePath;
                }
                Square70x70Logo = tile.SmallImagePath.GetFileName();
            }
        }
        public void FillTo<T>(ref T tile) where T : Tile
        {
            bool relativeExists(string relativePath, Tile sourceTile)
            {
                return File.Exists(getAbsolutePath(relativePath, sourceTile));
            }
            string getAbsolutePath(string relativePath, Tile sourceTile)
            {
                if (relativePath.StartsWith("\\"))
                {
                    relativePath = relativePath.Remove(0, 1);
                }
                string absolutePath = sourceTile.TargetPath.RemoveFileName().Backslash() + relativePath;
                return absolutePath;
            }

            tile.Theme = ForegroundText;
            tile.Background = BackgroundColor;
            tile.ShowName = ShowNameOnSquare150x150Logo;

            string largeImagePath = Square150x150Logo;
            if (File.Exists(largeImagePath))
            {
                tile.LargeImagePath = largeImagePath;
            }
            else if (relativeExists(largeImagePath, tile))
            {
                tile.LargeImagePath = getAbsolutePath(largeImagePath, tile);
            }

            string smallImagePath = Square70x70Logo;
            if (File.Exists(smallImagePath))
            {
                tile.SmallImagePath = smallImagePath;
            }
            else if (relativeExists(smallImagePath, tile))
            {
                tile.SmallImagePath = getAbsolutePath(smallImagePath, tile);
            }
        }

        public TextTheme ForegroundText
        {
            get
            {
                string text = visualElements.Attribute(nameof(ForegroundText))?.Value;
                return text == TileManager.DarkThemeString ? TextTheme.Dark : TextTheme.Light;
            }
            set
            {
                visualElements.SetAttributeValue(nameof(ForegroundText),
                    value == TextTheme.Dark
                    ? TileManager.DarkThemeString
                    : TileManager.LightThemeString
                );
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return visualElements.Attribute(nameof(BackgroundColor)).Value.FromHexString();
            }
            set
            {
                visualElements.SetAttributeValue(nameof(BackgroundColor), value.ToHexString());
            }
        }
        public bool ShowNameOnSquare150x150Logo
        {
            get
            {
                return visualElements.Attribute(nameof(ShowNameOnSquare150x150Logo)).Value == "off" ? false : true;
            }
            set
            {
                visualElements.SetAttributeValue(nameof(ShowNameOnSquare150x150Logo), value ? "on" : "off");
            }
        }
        public string Square150x150Logo
        {
            get
            {
                return visualElements.Attribute(nameof(Square150x150Logo)).Value;
            }
            set
            {
                visualElements.SetAttributeValue(nameof(Square150x150Logo), value);
            }
        }
        public string Square70x70Logo
        {
            get
            {
                return visualElements.Attribute(nameof(Square70x70Logo)).Value;
            }
            set
            {
                visualElements.SetAttributeValue(nameof(Square70x70Logo), value);
            }
        }
    }
}
