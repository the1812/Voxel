using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model.Languages
{
    sealed class AboutLanguage : Language
    {
        public AboutLanguage() : base()
        {
        }

        protected override void Load()
        {
            keys = new List<string>
            {
                "WindowTitle",
            };
            simplifiedChinese = new List<string>
            {
                "关于",
            };
            americanEnglish = new List<string>
            {
                "About",
            };
        }
    }
}
