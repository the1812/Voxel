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
                "ButtonTarget",
                "OpenFileDialogTitle",
                "OpenFileDialogFilter",
                "ButtonBackColor",
            };
            simplifiedChinese = new List<string>
            {
                "无缩放磁贴",
                "选择目标",
                "选择文件（右键点击\"选择目标\"以选择文件夹）",
                "程序文件 (*.exe)|*.exe",
                "选择背景色",
            };
            americanEnglish = new List<string>
            {
                "Non-scalable Tile",
                "Select Target",
                "Select File (Right click \"Select Target\" to select a folder)",
                "Program File (*.exe)|*.exe",
                "Set Back Color",

            };
        }
    }
}
