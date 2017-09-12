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
                "CheckBoxDarkTheme",
                "CheckBoxShowName",
                "ButtonGenerate",
                "ButtonAddToStart",
                "ButtonImport",
                "ButtonExport",
                "GenerateSuccessTitle",
                "GenerateFailedTitle",
                "AdminTip",
                "TargetMissing",
                "TargetMissingTitle",
                "ToggleTileSizeLeft",
                "ToggleTileSizeRight",
                "OverwriteStartTitle",
                "OverwriteStartContent",
                "AddToStartFailedTitle",
                "AddToStartSuccessTitle",
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
                "使用暗色主题",
                "显示名称",
                "生成",
                "添加到开始",
                "导入",
                "导出",
                "生成成功",
                "生成失败",
                "权限不足，请以管理员身份运行。",
                "目标文件/文件夹未选择或不存在。",
                "目标无效",
                "中等",
                "小",
                "是否覆盖",
                "开始里已经存在快捷方式，是否要覆盖？",
                "添加失败",
                "添加成功",
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
                "Use Dark Theme",
                "Show Name",
                "Generate",
                "Add to Start",
                "Import",
                "Export",
                "Generate Success",
                "Failed to Generate",
                "Access denied. Please run as admin.",
                "The target file/folder is not selected or doesn't exist.",
                "Target Missing",
                "Medium",
                "Small",
                "Overwrite?",
                "Shortcut already exists, do you want to overwrite?",
                "Failed to Add",
                "Add Success",
            };
        }
    }
}
