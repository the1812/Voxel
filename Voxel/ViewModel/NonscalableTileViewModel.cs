using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model;
using Voxel.Model.Languages;

namespace Voxel.ViewModel
{
    sealed class NonscalableTileViewModel : ViewModel
    {
        public NonscalableTileViewModel() : base(new NonscalableTileLanguage()) { }

        public string WindowTitle => language[nameof(WindowTitle)];


        private NonscalableTileManager tileManager;
        public NonscalableTileManager TileManager
        {
            get => tileManager;
            set
            {
                tileManager = value;
                OnPropertyChanged(nameof(TileManager));
            }
        }


    }
}