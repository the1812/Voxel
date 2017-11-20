using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.ViewModel;

namespace Voxel.Model
{
    sealed class BusyStateController : IDisposable
    {
        private IBusyState state;
        public BusyStateController(IBusyState state)
        {
            this.state = state;
            state.IsBusy = true;
        }

        public void Dispose()
        {
            state.IsBusy = false;
        }
    }
}
