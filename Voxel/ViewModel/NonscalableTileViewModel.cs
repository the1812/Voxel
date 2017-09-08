using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model;
using Voxel.Model.Languages;
using Ace;
using Microsoft.Win32;
using System.Windows.Media;

namespace Voxel.ViewModel
{
    sealed class NonscalableTileViewModel : ViewModel
    {
        public NonscalableTileViewModel() : base(new NonscalableTileLanguage()) { }

        #region Language
        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonTarget => language[nameof(ButtonTarget)];
        public string ButtonBackColor => language[nameof(ButtonBackColor)];

        #endregion
        #region Vars and properties
        private NonscalableTileManager tileManager = new NonscalableTileManager();
        //public NonscalableTileManager TileManager
        //{
        //    get => tileManager;
        //    set
        //    {
        //        tileManager = value;
        //        OnPropertyChanged(nameof(TileManager));
        //    }
        //}

        private string TargetFileName
        {
            get
            {
                return tileManager.Tile.TargetPath?.GetFileName().RemoveExtension();
            }
        }
        private string TargetFolderName
        {
            get
            {
                string targetPath = tileManager.Tile.TargetPath?.NoQuotes().NoBackslash();
                if (targetPath == null) return "";
                int slashIndex = targetPath.LastIndexOf('\\');
                return targetPath.Substring(slashIndex + 1);
            }
        }
        public string TargetName
        {
            get
            {
                if (tileManager.Tile.TargetType == TargetType.File)
                {
                    return TargetFileName;
                }
                else
                {
                    return TargetFolderName;
                }
            }
        }
        

        private Color backColor = Ace.Wpf.DwmEffect.ColorizationColor;
        public Brush BackColor
        {
            get => new SolidColorBrush(backColor);
            set
            {
                backColor = (value as SolidColorBrush)?.Color ?? throw new ArgumentException();
                OnPropertyChanged(nameof(BackColor));
            }
        }

        private ImageSource icon;
        public ImageSource Icon
        {
            get => icon;
            set
            {
                icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }


        #endregion
        #region Commands

        public Command SelectFileCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    var dialog = new OpenFileDialog
                    {
                        Title = language["OpenFileDialogTitle"],
                        Multiselect = false,
                        AddExtension = true,
                        DefaultExt = ".exe",
                        CheckFileExists = true,
                        Filter = language["OpenFileDialogFilter"],
                    };
                    if (tileManager.Tile.TargetType == TargetType.File
                    && TargetFileName != null)
                    {
                        dialog.FileName = tileManager.Tile.TargetPath;
                    }
                    if (dialog.ShowDialog() ?? false)
                    {
                        string targetPath = dialog.FileName;
                        tileManager.Tile.TargetPath = targetPath;
                        tileManager.Tile.TargetType = TargetType.File;
                        var image = Ace.Win32.Api.GetIcon(targetPath);
                        Icon = image.ImageSource;
                        OnPropertyChanged(nameof(TargetName));
                    }
                },
            };
        public Command SelectFolderCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    var dialog = new System.Windows.Forms.FolderBrowserDialog
                    {
                        Description = language["OpenFolderDialogTitle"],
                        ShowNewFolderButton = false,
                    };
                    if (tileManager.Tile.TargetType == TargetType.Folder
                    && TargetFolderName != null)
                    {
                        dialog.SelectedPath = tileManager.Tile.TargetPath;
                    }
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string targetPath = dialog.SelectedPath;
                        tileManager.Tile.TargetPath = targetPath;
                        tileManager.Tile.TargetType = TargetType.Folder;
                        var image = Ace.Win32.Api.GetIcon(targetPath);
                        Icon = image.ImageSource;
                        OnPropertyChanged(nameof(TargetName));
                    }
                },
            };

#endregion

    }
}