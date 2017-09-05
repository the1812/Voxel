using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Voxel.Model
{
    static class TileSize
    {
        private static bool fullScreen, showMoreTiles;
        private static Size largeSize, smallSize;
        private static void calculateSize()
        {
            if (Fullscreen)
            {
                if (ShowMoreTiles)
                {

                }
                else
                {
                    largeSize = new Size(124.0, 124.0);
                    smallSize = new Size(60.0, 60.0);
                }
            }
            else
            {
                if (ShowMoreTiles)
                {

                }
                else
                {

                }
            }
        }
        public static bool Fullscreen
        {
            get => fullScreen;
            set
            {
                fullScreen = value;
                calculateSize();
            }
        }
        public static bool ShowMoreTiles
        {
            get => showMoreTiles;
            set
            {
                showMoreTiles = value;
                calculateSize();
            }
        }
        public static Size LargeSize => largeSize;
        public static Size SmallSize => smallSize;
    }
}
