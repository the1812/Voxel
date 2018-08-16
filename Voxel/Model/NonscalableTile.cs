using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;

namespace Voxel.Model
{
    sealed class NonscalableTile : Tile
    {
        public override string StartMenuTargetPath
        {
            get
            {
                if (TargetType == TargetType.File)
                {
                    return StartMenu + TargetPath.GetParentFolder().RemoveExtension() + ".lnk";
                }
                else
                {
                    return StartMenu + TargetPath.GetParentFolder().NoBackslash() + ".lnk";
                }
            }
        }
    }
}