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
using System.Windows.Controls;

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

        
        public static void ClearTileCache(Func<FileInfo, bool> fileFilter = null)
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
                        if (fileFilter?.Invoke(file) ?? true)
                        {
                            file.LastWriteTime = DateTime.Now;
                        }
                    }
                    catch (IOException)
                    {
                        continue;
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

        private bool isClearTileCacheBusy = false;
        public bool IsClearTileCacheBusy
        {
            get => isClearTileCacheBusy;
            set
            {
                isClearTileCacheBusy = value;
                View.Cursor = value ? Cursors.AppStarting : Cursors.Arrow;
                OnPropertyChanged(nameof(IsClearTileCacheBusy));
            }
        }


        //private bool isNonscalableTileBusy;
        //public bool IsNonscalableTileBusy
        //{
        //    get => isNonscalableTileBusy;
        //    set
        //    {
        //        isNonscalableTileBusy = value;
        //        View.Cursor = value ? Cursors.AppStarting : Cursors.Arrow;
        //        OnPropertyChanged(nameof(IsNonscalableTileBusy));
        //    }
        //}



        public BindingCommand NonscalableTileCommand => new BindingCommand
        {
            ExcuteAction = (o) =>
            {
                View.NonscalableTileView.Owner = View;
                View.NonscalableTileView.ShowDialog();
            },
        };
        public BindingCommand ImageTileCommand => new BindingCommand
        {
            ExcuteAction = (o) =>
            {
                //View.ImageTileView.Owner = View;
                //View.ImageTileView.ShowDialog();
            },
        };
        public BindingCommand ClearTileCacheCommand => new BindingCommand
        {
            ExcuteAction = async (o) =>
            {
                if (!Ace.Utils.IsAdministratorProcess)
                {
                    View.ShowMessage(App.GeneralLanguage["AdminTip"], language["ClearFailedTitle"], false);
                    return;
                }
                IsClearTileCacheBusy = true;
                await Task.Run(() =>
                {
                    ClearTileCache();
                    View.Dispatcher.Invoke(() =>
                    {
                        View.ShowMessage("", language["ClearSuccessTitle"], false);
                    });
                });
                IsClearTileCacheBusy = false;
            },
        };

        public BindingCommand AboutCommand => new BindingCommand
        {
            ExcuteAction = (o) =>
            {
                //var window = new AboutView
                //{
                //    Owner = View
                //};
                //window.ShowDialog();
            },
        };
    }
}
