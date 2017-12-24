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
                "CheckBoxKeepRatio",
                "ButtonSetAction",
                "RadioButtonActionNone",
                "RadioButtonActionFile",
                "RadioButtonActionFolder",
                "RadioButtonActionUrl",
                "ButtonBackColor",
                "OpenImageDialogTitle",
                "OpenImageDialogFilter",
                "ImageMarginLabel",
                "ImageMarginTip",
                "PreviewGridWidthText",
                "PreviewGridHeightText",
            };
            simplifiedChinese = new List<string>
            {
                "图像磁贴",
                "生成",
                "添加到开始",
                "导出",
                "导入",
                "选择图片",
                "保持宽高比",
                "设置动作",
                "无",
                "文件",
                "文件夹",
                "链接",
                "设定背景色",
                "选择图片",
                "图片文件 (*.jpg;*.png;*.bmp;*.tif;*.jpeg;*.tiff)|*.jpg;*.png;*.bmp;*.tif;*.jpeg;*.tiff",
                "图片边距",
                "边距按照左、上、右、下的顺序",
                "整体宽度",
                "整体高度",
            };
            americanEnglish = new List<string>
            {
                "ImageTile",
                "Generate",
                "Add to Start",
                "Export",
                "Import",
                "Select Image",
                "Keep Aspect Ratio",
                "Set Action",
                "None",
                "File",
                "Folder",
                "Url",
                "Set Backcolor",
                "Select Image",
                "Image File (*.jpg;*.png;*.bmp;*.tif;*.jpeg;*.tiff)|*.jpg;*.png;*.bmp;*.tif;*.jpeg;*.tiff",
                "Image margin",
                "Margin is in the order of left, top, right and bottom",
                "Total width",
                "Total height",
            };
        }
    }
}
