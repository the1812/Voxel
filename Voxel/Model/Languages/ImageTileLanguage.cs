using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model.Languages
{
    sealed class ImageTileLanguage : Language
    {
        public ImageTileLanguage() : base()
        {
        }

        protected override void Load()
        {
            keys = new List<string>
            {
                "WindowTitle",
                "ButtonGenerate",
                "ButtonAddToStart",
                "ButtonExport",
                "ButtonImport",
                "ButtonSelectImage",

            };
            simplifiedChinese = new List<string>
            {
                "图像磁贴",
                "生成",
                "添加到开始",
                "导出",
                "导入",
                "选择图片"
            };
            americanEnglish = new List<string>
            {
                "ImageTile",
                "Generate",
                "Add to Start",
                "Export",
                "Import",
                "Select Image",

            };
        }
    }
}
