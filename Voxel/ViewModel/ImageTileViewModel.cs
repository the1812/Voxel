using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model.Languages;

namespace Voxel.ViewModel
{
    sealed class ImageTileViewModel : ViewModel
    {
        public ImageTileViewModel() : base(new ImageTileLanguage())
        {
        }

        public string WindowTitle => language[nameof(WindowTitle)];

        public void ClearData()
        {
            
        }
    }
}
