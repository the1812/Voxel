using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model.Languages;

namespace Voxel.ViewModel
{
    abstract class ViewModel : NotificationObject
    {
        public ViewModel(Language languageSource) => language = languageSource.Dictionary;
        protected Dictionary<string, string> language;
        public string this[string key] => language[key];
    }
}
