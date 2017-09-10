using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model.Languages;

namespace Voxel.ViewModel
{
    sealed class AboutViewModel : ViewModel
    {
        public AboutViewModel() : base(new AboutLanguage())
        {
        }

        public string WindowTitle => language[nameof(WindowTitle)];
    }
}
