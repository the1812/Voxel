using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Win32.Enumerations
{
    /// <summary>
    /// Flags controlling how the Image List item is 
    /// drawn
    /// </summary>
    [Flags]
    public enum ImageListDrawItem : int
    {
        /// <summary>
        /// Draw item normally.
        /// </summary>
        Normal = 0x0,
        /// <summary>
        /// Draw item transparently.
        /// </summary>
        Transparent = 0x1,
        /// <summary>
        /// Draw item blended with 25% of the specified foreground colour
        /// or the Highlight colour if no foreground colour specified.
        /// </summary>
        Hover = 0x2,
        /// <summary>
        /// Draw item blended with 50% of the specified foreground colour
        /// or the Highlight colour if no foreground colour specified.
        /// </summary>
        Selected = 0x4,
        /// <summary>
        /// Draw the icon's mask
        /// </summary>
        Mask = 0x10,
        /// <summary>
        /// Draw the icon image without using the mask
        /// </summary>
        Image = 0x20,
        /// <summary>
        /// Draw the icon using the ROP specified.
        /// </summary>
        Rop = 0x40,
        /// <summary>
        /// Preserves the alpha channel in dest. XP only.
        /// </summary>
        PreservesAlpha = 0x1000,
        /// <summary>
        /// Scale the image to cx, cy instead of clipping it.  XP only.
        /// </summary>
        Scale = 0x2000,
        /// <summary>
        /// Scale the image to the current DPI of the display. XP only.
        /// </summary>
        DpiScale = 0x4000
    }
}
