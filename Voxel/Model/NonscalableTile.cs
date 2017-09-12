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

        private string largeImagePath;
        public string LargeImagePath
        {
            get => largeImagePath;
            set
            {
                largeImagePath = value;
                OnPropertyChanged(nameof(LargeImagePath));
            }
        }

        private string smallImagePath;
        public string SmallImagePath
        {
            get => smallImagePath;
            set
            {
                smallImagePath = value;
                OnPropertyChanged(nameof(SmallImagePath));
            }
        }


        public override string StartMenuTargetPath => StartMenu + TargetPath.GetFileName().RemoveExtension() + ".lnk";
    }
}