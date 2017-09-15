using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model.Languages;

namespace Voxel
{
    sealed class BadVoxelFileException : Exception
    {
        public BadVoxelFileException() : base(new GeneralLanguage().Dictionary[nameof(BadVoxelFileException)]) { }
    }
}
