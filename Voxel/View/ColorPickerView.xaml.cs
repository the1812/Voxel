using System;
using System.Collections.Generic;
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
    /// ColorPickerView.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPickerView : Window
    {
        public ColorPickerView()
        {
            InitializeComponent();
            DataContext = new ColorPickerViewModel();
            buttonOK.Click += (s, e) =>
            {
                DialogResult = true;
            };
        }
        public Color SelectedColor
        {
            get => (DataContext as ColorPickerViewModel).SelectedColor;
            set => (DataContext as ColorPickerViewModel).SelectedColor = value;
        }
    }

}
