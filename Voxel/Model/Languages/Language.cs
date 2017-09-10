using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;

namespace Voxel.Model.Languages
{
    abstract class Language : NotificationObject
    {
        protected static List<string> DefaultLanguage => americanEnglish;
        protected static List<string> keys;
        protected static List<string> simplifiedChinese;
        protected static List<string> americanEnglish;
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
                        return keys.MakeDictionary(DefaultLanguage);
                }
            }
        }
        public Language(CultureInfo cultureInfo)
        {
            Culture = cultureInfo;
            Load();
        }
        public Language()
        {
            //Culture = new CultureInfo("en-US");
            Culture = CultureInfo.CurrentUICulture;
            Load();
        }
        protected abstract void Load();
    }
}
