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
using Voxel.View;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Voxel.ViewModel
{
    sealed class NonscalableTileViewModel : ViewModel
    {
        public NonscalableTileViewModel(NonscalableTileView view) : base(new NonscalableTileLanguage())
        {
            View = view;
        }

        #region Language
        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonTarget => language[nameof(ButtonTarget)];
        public string ButtonBackColor => language[nameof(ButtonBackColor)];
        public string ButtonTargetTip => language[nameof(ButtonTargetTip)];

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
        public NonscalableTileView View { get; private set; }

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
        public Brush Background
        {
            get => new SolidColorBrush(backColor);
            set
            {
                backColor = (value as SolidColorBrush)?.Color ?? throw new ArgumentException();
                OnPropertyChanged(nameof(BackColor));
                OnPropertyChanged(nameof(Background));
            }
        }
        public Color BackColor
        {
            get => backColor;
            set
            {
                backColor = value;
                OnPropertyChanged(nameof(BackColor));
                OnPropertyChanged(nameof(Background));
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
        public Command SelectColorCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    if (o is Grid gridPreview)
                    {
                        var colorPicker = new ColorPickerView(BackColor)
                        {
                            SelectedColor = BackColor,
                            Owner = View,
                        };
                        var colorPickerViewModel = colorPicker.DataContext as ColorPickerViewModel;

                        var originalColorBinding = gridPreview.GetBindingExpression(Control.BackgroundProperty).ParentBinding;
                        var colorPickerBinding = new Binding
                        {
                            Source = colorPickerViewModel,
                            Path = new PropertyPath("SelectedBrush"),

                        };

                        gridPreview.SetBinding(Control.BackgroundProperty, colorPickerBinding);
                        if (colorPicker.ShowDialog() ?? false)
                        {
                            BackColor = colorPicker.SelectedColor;
                        }
                        gridPreview.SetBinding(Control.BackgroundProperty, originalColorBinding);
                    }
                    
                },
            };
#endregion

    }
}