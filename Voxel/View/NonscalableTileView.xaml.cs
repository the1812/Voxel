using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Voxel.ViewModel;
using Ace;
using Ace.Win32;
using Ace.Wpf;

namespace Voxel.View
{
    /// <summary>
    /// NonscalableTileView.xaml 的交互逻辑
    /// </summary>
    public partial class NonscalableTileView : Window
    {
        private NonscalableTileViewModel dataContext;
        public NonscalableTileView()
        {
            InitializeComponent();
            DataContext = dataContext = new NonscalableTileViewModel(this);
            Loaded += (s, e) =>
            {
                this.ReceiveMessage((IntPtr handle, int messgae, IntPtr wParam, IntPtr lParam, ref bool handled) =>
                {
                    if (messgae == 0x0320) // WindowsMessage.DwmColorizationColorChanged 
                    {
                        long colorValue = (long) wParam;
                        dataContext.DwmColorChanged(colorValue.ToColor());
                    }
                    return IntPtr.Zero;
                });
            };
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            dataContext.ClearData();
            e.Cancel = true;
            base.OnClosing(e);
        }
        
    }
}
