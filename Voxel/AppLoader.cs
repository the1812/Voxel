using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Voxel.Model;
using Voxel.Model.Languages;
using Voxel.View;
using Voxel.ViewModel;

namespace Voxel
{
    static class AppLoader
    {
        public static void Load(StartupEventArgs e)
        {
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
        }
    }
}
