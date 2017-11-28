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
        private ColorPickerViewModel dataContext;
        public ColorPickerView(Color oldColor)
        {
            InitializeComponent();
            dataContext = new ColorPickerViewModel
            {
                OldColor = oldColor
            };
            DataContext = dataContext;
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
                SelectedColor = dataContext.OldColor;
            }
            base.OnClosing(e);
        }
        public Color SelectedColor
        {
            get => dataContext.SelectedColor;
            set => dataContext.SelectedColor = value;
        }
    }

}
