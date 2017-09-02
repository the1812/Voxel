using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Voxel.ViewModel
{
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private void onCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public Func<object, bool> CanExecuteAction { get; set; }
        public Action<object> ExcuteAction { get; set; }
        public bool CanExecute(object parameter)
        {
            return CanExecuteAction?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            ExcuteAction?.Invoke(parameter);
        }
    }
}
