using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;

namespace Voxel.Model.Languages
{
    sealed class MainLanguage : Language
    {
        public MainLanguage(CultureInfo cultureInfo) : base(cultureInfo) { }
        public MainLanguage() : base() { }

        protected override void Load()
        {
            keys = new List<string>
            {
                "WindowTitle"
            };
            simplifiedChinese = new List<string>
            {
                "Voxel"
            };
            americanEnglish = new List<string>
            {
                "Voxel"
            };
        }
    }
}