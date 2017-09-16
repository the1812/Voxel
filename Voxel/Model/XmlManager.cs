using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                new XAttribute("BackgroundColor", Ace.Wpf.DwmEffect.ColorizationColor.ToInt32()),
                new XAttribute("ShowNameOnSquare150x150Logo", "on"),
                new XAttribute("Square150x150Logo", ""),
                new XAttribute("Square70x70Logo", "")
            );
            root.Add(visualElements);
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
    }
}
