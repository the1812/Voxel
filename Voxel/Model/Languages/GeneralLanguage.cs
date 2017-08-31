using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model.Languages
{
    sealed class GeneralLanguage : Language
    {
        public GeneralLanguage(CultureInfo cultureInfo) : base(cultureInfo) { }
        public GeneralLanguage() : base() { }

        protected override void Load()
        {
            keys = new List<string>
            {
                nameof(TileTypeNotMatchException),
            };
            simplifiedChinese = new List<string>
            {
                "尝试读取的磁贴类型不匹配。",
            };
            americanEnglish = new List<string>
            {
                "The type of tile is not match."
            };
        }
    }
}
