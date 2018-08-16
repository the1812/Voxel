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
using static Voxel.Model.Settings;

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
                var fullScreen = GetBoolean(MakeKey(StartMenuKey, nameof(TileSize.Fullscreen)));
                TileSize.Fullscreen = fullScreen;
                var showMoreTiles = GetBoolean(MakeKey(StartMenuKey, nameof(TileSize.ShowMoreTiles)));
                TileSize.ShowMoreTiles = showMoreTiles;

                // new MainView().ShowDialog();
                new NonscalableTileView().ShowDialog();
            }
        }
    }
}
