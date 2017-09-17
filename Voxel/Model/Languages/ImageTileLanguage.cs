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
            };
            simplifiedChinese = new List<string>
            {
                "图像磁贴",
            };
            americanEnglish = new List<string>
            {
                "ImageTile",
            };
        }
    }
}
