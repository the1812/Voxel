using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model
{
    sealed class ScalableTile : Tile
    {
        public string TargetPath { get; set; }
        public string SubFolderName { get; set; }
        public string ImageName { get; set; }

        public override string StartMenuTargetPath => StartMenu + TargetPath.GetFileName() + ".lnk";
    }
}
