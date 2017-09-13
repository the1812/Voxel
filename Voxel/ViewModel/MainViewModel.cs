using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.View;
using Voxel.Model.Languages;
using System.IO;
using System.Diagnostics;
using System.Windows.Input;

namespace Voxel.ViewModel
{
    sealed class MainViewModel : ViewModel
    {
        public MainViewModel(MainView mainView) : base(new MainLanguage())
        {
            View = mainView;
        }

        public MainView View { get; private set; }


        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonNonscalableTile => language[nameof(ButtonNonscalableTile)];
        public string ButtonScalableTile => language[nameof(ButtonScalableTile)];
        public string ButtonImageTile => language[nameof(ButtonImageTile)];
        public string ButtonClearTileCache => language[nameof(ButtonClearTileCache)];

        
        public static void ClearTileCacheAsync()
        {
            void clearTileCacheInFolder(DirectoryInfo directoryInfo)
            {
                var subDirs = directoryInfo.EnumerateDirectories();
                if (subDirs.Count() != 0)
                {
                    foreach (var subDir in subDirs)
                    {
                        clearTileCacheInFolder(subDir);
                    }
                }

                foreach (var file in directoryInfo.EnumerateFiles())
                {
                    try
                    {
                        file.LastWriteTime = DateTime.Now;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                }
            }

            DirectoryInfo commonStart = new DirectoryInfo(Environment.GetEnvironmentVariable("ProgramData") + @"\Microsoft\Windows\Start Menu\Programs");
            DirectoryInfo userStart = new DirectoryInfo(Environment.GetEnvironmentVariable("UserProfile") + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs");
            
            clearTileCacheInFolder(commonStart);
            clearTileCacheInFolder(userStart);
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                View.Cursor = value ? Cursors.Wait : Cursors.Arrow;
                OnPropertyChanged(nameof(IsBusy));
            }
        }



        public Command NonscalableTileCommand => new Command
        {
            ExcuteAction = (o) =>
            {
                var window = new NonscalableTileView
                {
                    Owner = View
                };
                window.ShowDialog();
            },
        };
        public Command ClearTileCacheCommand => new Command
        {
            ExcuteAction = async (o) =>
            {
                if (!Ace.Utils.IsAdministratorProcess)
                {
                    View.ShowMessage(App.GeneralLanguage["AdminTip"], language["ClearFailedTitle"], false);
                    return;
                }
                IsBusy = true;
                await Task.Run(() =>
                {
                    ClearTileCacheAsync();
                    View.Dispatcher.Invoke(() =>
                    {
                        View.ShowMessage("", language["ClearSuccessTitle"], false);
                    });
                });
                IsBusy = false;
            },
        };

        public Command AboutCommand => new Command
        {
            ExcuteAction = (o) =>
            {
                var window = new AboutView
                {
                    Owner = View
                };
                window.ShowDialog();
            },
        };
    }
}
