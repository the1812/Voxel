using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;

namespace Voxel.Model
{
    sealed class ImageTile : Tile
    {
        public static string GroupPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Backslash() + @"Voxel\";
        public string Name { get; set; }
        public string Action { get; set; }
        public string GroupName { get; set; }

        public void Save()
        {

        }

        public override string StartMenuTargetPath => StartMenu + $"Voxel\\{GroupName}\\{Name}.lnk";
    }
}
