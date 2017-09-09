using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model.Languages
{
    sealed class ColorPickerLanguage : Language
    {
        public ColorPickerLanguage() : base()
        {
        }

        public ColorPickerLanguage(CultureInfo cultureInfo) : base(cultureInfo)
        {
        }

        protected override void Load()
        {
            keys = new List<string>
            {
                "WindowTitle",
                "ButtonOK",
                "ButtonCancel",
            };
            simplifiedChinese = new List<string>
            {
                "颜色设定",
                "确定",
                "取消",
            };
            americanEnglish = new List<string>
            {
                "Set Color",
                "OK",
                "Cancel",
            };
        }
    }
}
