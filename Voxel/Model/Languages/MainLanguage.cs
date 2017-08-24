using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;

namespace Voxel.Model.Languages
{
    sealed class MainLanguage : Model
    {
        private static List<string> DefaultLanguage => americanEnglish;
        private static List<string> keys = new List<string>
        {
            "WindowTitle"
        };
        private static List<string> simplifiedChinese = new List<string>
        {
            "Voxel"
        };
        private static List<string> americanEnglish = new List<string>
        {
            "Voxel"
        };
        public CultureInfo Culture { get; set; }
        public Dictionary<string, string> Dictionary
        {
            get
            {
                switch (Culture?.Name)
                {
                    case "zh-CN":
                        return keys.MakeDictionary(simplifiedChinese);
                    case "en-US":
                        return keys.MakeDictionary(americanEnglish);
                    default:
                        return keys.MakeDictionary(americanEnglish);
                }
            }
        }
        public MainLanguage(CultureInfo cultureInfo)
        {
            Culture = cultureInfo;
        }
    }
}
