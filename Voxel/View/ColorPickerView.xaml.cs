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
    /// ColorPickerView.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPickerView : Window
    {
        public ColorPickerView(Color oldColor)
        {
            InitializeComponent();
            this.oldColor = oldColor;
            DataContext = new ColorPickerViewModel();
            buttonOK.Click += (s, e) =>
            {
                DialogResult = true;
            };
            buttonCancel.Click += (s, e) =>
            {
                DialogResult = false;
            };
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == false)
            {
                SelectedColor = oldColor;
            }
            base.OnClosing(e);
        }
        private Color oldColor;
        public Color SelectedColor
        {
            get => (DataContext as ColorPickerViewModel).SelectedColor;
            set => (DataContext as ColorPickerViewModel).SelectedColor = value;
        }
    }

}
