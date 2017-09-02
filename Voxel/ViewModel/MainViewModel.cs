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
    sealed class MainViewModel : ViewModel
    {
        public MainViewModel() : base(new MainLanguage()) { }

        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonNonscalableTile => language[nameof(ButtonNonscalableTile)];
    }
}
