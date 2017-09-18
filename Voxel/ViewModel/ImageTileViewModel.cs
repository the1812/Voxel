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
#region Language
        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonGenerate => language[nameof(ButtonGenerate)];
        public string ButtonAddToStart => language[nameof(ButtonAddToStart)];
        public string ButtonExport => language[nameof(ButtonExport)];
        public string ButtonImport => language[nameof(ButtonImport)];
        public string ButtonSelectImage => language[nameof(ButtonSelectImage)];
        #endregion
        #region Vars and properties
        public void ClearData()
        {
            
        }
        #endregion
        #region Commands

#endregion
    }
}
