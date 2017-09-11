using Ace.Win32;
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
    /// MessageView.xaml 的交互逻辑
    /// </summary>
    public partial class MessageView : Window
    {
        public MessageView()
        {
            InitializeComponent();
            DataContext = new MessageViewModel();
            buttonOK.Click += (s, e) =>
            {
                DialogResult = true;
            };
            buttonCancel.Click += (s, e) =>
            {
                DialogResult = false;
            };
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            this.RemoveTitleIcon();
            base.OnSourceInitialized(e);
        }
        //public static bool ShowDialog(string content, string title, bool showCancelButton)
        //{
        //    var dialog = new MessageView();
        //    var viewModel = dialog.DataContext as MessageViewModel;
        //    viewModel.Content = content;
        //    viewModel.Title = title;
        //    viewModel.ShowCancelButton = showCancelButton;
        //    return dialog.ShowDialog() ?? false;
        //}

    }
}
