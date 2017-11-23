using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.ViewModel
{
    interface IWaitingState
    {
        bool IsWaiting { get; set; }
    }
}
