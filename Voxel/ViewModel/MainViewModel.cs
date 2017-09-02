using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model;
using Voxel.Model.Languages;

namespace Voxel.ViewModel
{
    sealed class MainViewModel
    {
        public MainViewModel()
        {
            language = new MainLanguage(CultureInfo.CurrentUICulture).Dictionary;
        }
        private Dictionary<string, string> language;

        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonNonScalableTile => language[nameof(ButtonNonScalableTile)];
    }
}
