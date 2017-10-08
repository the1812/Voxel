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
                "WindowTitle",
                "ButtonNonscalableTile",
                "ButtonScalableTile",
                "ButtonImageTile",
                "ButtonClearTileCache",
                "ClearSuccessTitle",
                "ClearFailedTitle",
            };
            simplifiedChinese = new List<string>
            {
                "VOXEL",
                "无缩放磁贴",
                "可缩放磁贴(未完成)",
                "图像磁贴(未完成)",
                "清除磁贴缓存",
                "清除缓存成功",
                "清除缓存失败",
            };
            americanEnglish = new List<string>
            {
                "VOXEL",
                "Non-scalable Tile",
                "Scalable Tile (Coming Soon)",
                "Image Tile (Coming Soon)",
                "Clear Tile Cache",
                "Clear Success",
                "Failed to clear",
            };
        }
    }
}
