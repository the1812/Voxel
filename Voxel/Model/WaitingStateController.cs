using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.ViewModel;

namespace Voxel.Model
{
    sealed class WaitingStateController : IDisposable
    {
        private IWaitingState state;
        private Action end;
        public WaitingStateController(IWaitingState state, Action startAction, Action endAction)
        {
            this.state = state;
            state.IsWaiting = true;
            startAction?.Invoke();
        }

        public void Dispose()
        {
            state.IsWaiting = false;
            end?.Invoke();
        }
    }
}
