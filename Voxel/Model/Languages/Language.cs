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
        protected List<string> DefaultLanguage => americanEnglish;
        protected List<string> keys;
        protected List<string> simplifiedChinese;
        protected List<string> americanEnglish;
        protected Dictionary<string, string> cachedDictionary;

        public CultureInfo Culture { get; set; }
        public Dictionary<string, string> Dictionary
        {
            get
            {
                if (cachedDictionary is null)
                {
                    switch (Culture?.Name)
                    {
                        case "zh-CN":
                        {
                            cachedDictionary = keys.MakeDictionary(simplifiedChinese);
                            break;
                        }
                        case "en-US":
                        {
                            cachedDictionary = keys.MakeDictionary(americanEnglish);
                            break;
                        }
                        default:
                        {
                            cachedDictionary = keys.MakeDictionary(DefaultLanguage);
                            break;
                        }
                    }
                }
                return cachedDictionary;
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
        public string this[string key] => Dictionary[key];
    }
}
