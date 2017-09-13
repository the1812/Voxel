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

        
        public static Task ClearTileCacheAsync()
        {
            void clearTileCacheInFolder(DirectoryInfo directoryInfo, ProcessStartInfo info)
            {
                var subDirs = directoryInfo.EnumerateDirectories();
                if (subDirs.Count() != 0)
                {
                    foreach (var subDir in subDirs)
                    {
#if DEBUG
                        Debug.WriteLine(subDir.FullName);
#endif
                        clearTileCacheInFolder(subDir, info);
                    }
                }

                info.WorkingDirectory = directoryInfo.FullName;
                Process.Start(info).WaitForExit();
            }

            string command = @"for %f in (*.*) do copy /b ""%f"" +,,";
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = @"/c " + command
            };
            DirectoryInfo commonStart = new DirectoryInfo(Environment.GetEnvironmentVariable("ProgramData") + @"\Microsoft\Windows\Start Menu\Programs");
            DirectoryInfo userStart = new DirectoryInfo(Environment.GetEnvironmentVariable("UserProfile") + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs");

            return Task.Run(() =>
            {
                clearTileCacheInFolder(commonStart, startInfo);
                clearTileCacheInFolder(userStart, startInfo);
            });
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
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
                IsBusy = true;
                try
                {
                    await ClearTileCacheAsync();
                    View.ShowMessage("", language["ClearSuccessTitle"], false);
                }
                catch (UnauthorizedAccessException)
                {
                    View.ShowMessage(App.GeneralLanguage["AdminTip"], language["ClearFailedTitle"], false);
                }
                finally
                {
                    IsBusy = false;
                }
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
