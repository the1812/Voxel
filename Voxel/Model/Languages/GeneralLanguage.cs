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
                "文件中的磁贴类型与当前编辑器不匹配。",
            };
            americanEnglish = new List<string>
            {
                "The tile type in file doesn't match the current editor."
            };
        }
    }
}
