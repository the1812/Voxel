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
                "OpenFolderDialogTitle",
                "ButtonTargetTip",
                "ButtonBackColorTip",
                "ButtonBackImage",
                "ButtonBackImageTip",
                "OpenImageDialogTitle",
                "OpenImageDialogFilter",
            };
            simplifiedChinese = new List<string>
            {
                "无缩放磁贴",
                "选择目标",
                "选择文件（右键点击\"选择目标\"以选择文件夹）",
                "程序文件 (*.exe)|*.exe",
                "设定背景色",
                "选择文件夹",
                "单击以选择文件，右击以选择文件夹",
                "单击以设定背景色，右击以恢复默认",
                "选择图片",
                "单击以选择图片，右击以清除图片",
                "选择图片",
                "图片文件 (*.jpg;*.png;*.bmp;*.tif;*.jpeg;*.tiff)|*.jpg;*.png;*.bmp;*.tif;*.jpeg;*.tiff",
            };
            americanEnglish = new List<string>
            {
                "Non-scalable Tile",
                "Select Target",
                "Select File (Right click \"Select Target\" to select a folder)",
                "Program File (*.exe)|*.exe",
                "Set Back Color",
                "Select Folder",
                "Click to select file, right-click to select folder",
                "Click to set back color, right-click to restore default color",
                "Select Image",
                "Click to select image, right-click to clear image",
                "Select Image",
                "Image File (*.jpg;*.png;*.bmp;*.tif;*.jpeg;*.tiff)|*.jpg;*.png;*.bmp;*.tif;*.jpeg;*.tiff",
            };
        }
    }
}
