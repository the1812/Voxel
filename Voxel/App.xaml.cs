using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Voxel.Model;
using Voxel.Model.Languages;
using Voxel.View;
using Voxel.ViewModel;

namespace Voxel
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static Dictionary<string, string> GeneralLanguage { get; private set; }
        static App()
        {
            GeneralLanguage = new GeneralLanguage().Dictionary;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Settings.Save();
            base.OnExit(e);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            Settings.Load();

            // Select the text in a TextBox when it receives focus.
            // See https://stackoverflow.com/questions/660554/how-to-automatically-select-all-text-on-focus-in-wpf-textbox
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(selectivelyIgnoreMouseButton));
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent,
                new RoutedEventHandler(selectAllText));
            EventManager.RegisterClassHandler(typeof(TextBox), Control.MouseDoubleClickEvent,
                new RoutedEventHandler(selectAllText));
            base.OnStartup(e);

            if (e.Args != null && e.Args.Length > 0)
            {
                if (e.Args[0].ToLower() == "--clear")
                {
                    MainViewModel.ClearTileCache();
                    var language = new MainLanguage().Dictionary;
                    var dialog = new MessageView()
                    {
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };
                    var viewModel = dialog.DataContext as MessageViewModel;
                    viewModel.Content = "";
                    viewModel.Title = language["ClearSuccessTitle"];
                    viewModel.ShowCancelButton = false;
                    dialog.ShowDialog();
                }
            }
            else
            {
                bool fullScreen = Settings.Json["StartMenu"].ObjectValue[nameof(TileSize.Fullscreen)].BooleanValue ?? false;
                TileSize.Fullscreen = fullScreen;
                bool showMoreTiles = Settings.Json["StartMenu"].ObjectValue[nameof(TileSize.ShowMoreTiles)].BooleanValue ?? false;
                TileSize.ShowMoreTiles = showMoreTiles;

                new MainView().ShowDialog();
            }
            Shutdown();
        }

        void selectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox) parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focused, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        void selectAllText(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TextBox textBox)
                textBox.SelectAll();
        }
    }
}
