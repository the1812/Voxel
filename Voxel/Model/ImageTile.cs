using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;
using System.Windows.Media;

namespace Voxel.Model
{
    sealed class ImageTile : Tile
    {
        //public ImageTile(string groupName, string name)
        //{
        //    GroupName = groupName;
        //    Name = name;
        //    TargetType = TargetType.File;
        //    TargetPath = GroupPath + Name.Backslash() + ActionExecutorName;

        //}
        public ImageTile(string groupName)
        {
            GroupName = groupName;
            TargetType = TargetType.File;
        }

        public const string ActionExecutorName = "action.exe";
        
        private string GroupName { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public ImageSource Image { get; set; }

        public override string StartMenuTargetPath => StartMenu + $"Voxel\\{GroupName}\\{Name}.lnk";
    }
}
