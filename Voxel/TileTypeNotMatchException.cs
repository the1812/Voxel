using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model.Languages;

namespace Voxel
{
    sealed class TileTypeNotMatchException : Exception
    {
        public TileTypeNotMatchException(string message) : base(message) { }
        public TileTypeNotMatchException() : base(new GeneralLanguage().Dictionary[nameof(TileTypeNotMatchException)]) { }
    }
}
