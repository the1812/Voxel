using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model.Languages
{
    sealed class NonscalableTileLanguage : Language
    {
        public NonscalableTileLanguage(CultureInfo cultureInfo) : base(cultureInfo) { }
        public NonscalableTileLanguage() : base() { }
        protected override void Load()
        {
            keys = new List<string>
            {
                "WindowTitle",
            };
            simplifiedChinese = new List<string>
            {
                "无缩放磁贴",
            };
            americanEnglish = new List<string>
            {
                "Non-scalable Tile",
            };
        }
    }
}
