using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Ace;

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
        public void Read(string fileName)
        {
            root = XElement.Load(fileName);
            visualElements = root.Element("VisualElements");
        }
        public void Save(string fileName)
        {
            root.Save(fileName);
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
                return visualElements.Attribute(nameof(BackgroundColor)).Value.ToInt32().ToColor();
            }
            set
            {
                visualElements.SetAttributeValue(nameof(BackgroundColor), value.ToInt32());
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
