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

namespace Voxel.View
{
    /// <summary>
    /// ImageTileView.xaml 的交互逻辑
    /// </summary>
    public partial class ImageTileView : Window
    {
        public ImageTileView()
        {
            InitializeComponent();
            DataContext = new ImageTileViewModel(this);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            (DataContext as ImageTileViewModel).ClearData();
            e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
