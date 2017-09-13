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
using System.Windows.Media.Imaging;
using System.IO;

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
        public string ButtonBackColorTip => language[nameof(ButtonBackColorTip)];
        public string ButtonBackImage => language[nameof(ButtonBackImage)];
        public string ButtonBackImageTip => language[nameof(ButtonBackImageTip)];
        public string CheckBoxDarkTheme => language[nameof(CheckBoxDarkTheme)];
        public string CheckBoxShowName => language[nameof(CheckBoxShowName)];
        public string ButtonGenerate => language[nameof(ButtonGenerate)];
        public string ButtonAddToStart => language[nameof(ButtonAddToStart)];
        public string ButtonImport => language[nameof(ButtonImport)];
        public string ButtonExport => language[nameof(ButtonExport)];
        public string ToggleTileSizeLeft => language[nameof(ToggleTileSizeLeft)];
        public string ToggleTileSizeRight => language[nameof(ToggleTileSizeRight)];

        #endregion
        #region Vars and properties
        private NonscalableTileManager tileManager = new NonscalableTileManager();
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
                string result;
                if (tileManager.Tile.TargetType == TargetType.File)
                {
                    result = TargetFileName;
                }
                else
                {
                    result = TargetFolderName;
                }

                var size = result.MeasureString();
                if (size.Width >= 100 && result.IndexOf(" ") == -1)
                {
                    while (size.Width >= 100)
                    {
                        result = result.Substring(0, result.Length - 1);
                        size = result.MeasureString();
                    }
                    ShowLongNameMark = true;
                }
                else
                {
                    ShowLongNameMark = false;
                }
                return result;
            }
        }

        private bool showLongNameMark;
        public bool ShowLongNameMark
        {
            get => showLongNameMark;
            set
            {
                showLongNameMark = value;
                OnPropertyChanged(nameof(ShowLongNameMark));
            }
        }


        private static Color dwmColor = Ace.Wpf.DwmEffect.ColorizationColor;
        private Color backColor = dwmColor;
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
                tileManager.Tile.Background = value;
                OnPropertyChanged(nameof(BackColor));
                OnPropertyChanged(nameof(Background));
            }
        }

        private ImageSource icon = null;
        public ImageSource Icon
        {
            get => icon;
            set
            {
                icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }


        private ImageSource backImage = null;
        public ImageSource BackImage
        {
            get => backImage ?? backImageSmall;
            set
            {
                backImage = value;
                OnPropertyChanged(nameof(BackImage));
                OnPropertyChanged(nameof(BackImageSmall));
                OnPropertyChanged(nameof(IconVisibility));
            }
        }
        public Visibility IconVisibility
        {
            get
            {
                if (backImage == null && backImageSmall == null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }


        private bool isDarkTheme = Ace.Wpf.DwmEffect.ForegroundColor == Colors.Black;
        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                isDarkTheme = value;
                tileManager.Tile.Theme = value ? TextTheme.Dark : TextTheme.Light;
                OnPropertyChanged(nameof(IsDarkTheme));
                OnPropertyChanged(nameof(NameForeground));
            }
        }
        public Brush NameForeground
        {
            get
            {
                if (IsDarkTheme)
                {
                    return new SolidColorBrush(Colors.Black);
                }
                else
                {
                    return new SolidColorBrush(Colors.White);
                }
            }
        }

        private bool showName = true;
        public bool ShowName
        {
            get => showName;
            set
            {
                showName = value;
                tileManager.Tile.ShowName = value;
                OnPropertyChanged(nameof(ShowName));
            }
        }

        private ImageSource backImageSmall;
        public ImageSource BackImageSmall
        {
            get
            {
                return backImageSmall ?? backImage;
            }
            set
            {
                backImageSmall = value;
                OnPropertyChanged(nameof(BackImage));
                OnPropertyChanged(nameof(BackImageSmall));
                OnPropertyChanged(nameof(IconVisibility));
            }
        }

        private bool isTileSizeToggleChecked;
        public bool IsTileSizeToggleChecked
        {
            get => isTileSizeToggleChecked;
            set
            {
                isTileSizeToggleChecked = value;
                OnPropertyChanged(nameof(IsTileSizeToggleChecked));
                OnPropertyChanged(nameof(LargePreviewVisibility));
                OnPropertyChanged(nameof(SmallPreviewVisibility));
            }
        }

        public Visibility LargePreviewVisibility
        {
            get
            {
                return IsTileSizeToggleChecked ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility SmallPreviewVisibility
        {
            get
            {
                return IsTileSizeToggleChecked ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void updateFromTileManager()
        {
            try
            {
                tileManager.LoadData();

                if (File.Exists(tileManager.Tile.TargetPath))
                {
                    var image = Ace.Win32.Api.GetIcon(tileManager.Tile.TargetPath);
                    Icon = image.ImageSource;
                }
                OnPropertyChanged(nameof(TargetName));

                if (File.Exists(tileManager.Tile.LargeImagePath))
                {
                    BackImage = new BitmapImage(new Uri(tileManager.Tile.LargeImagePath));
                }
                if (File.Exists(tileManager.Tile.SmallImagePath))
                {
                    BackImageSmall = new BitmapImage(new Uri(tileManager.Tile.SmallImagePath));
                }

                BackColor = tileManager.Tile.Background;
                IsDarkTheme = tileManager.Tile.Theme == TextTheme.Dark ? true : false;
                ShowName = tileManager.Tile.ShowName;
            }
            catch (TileTypeNotMatchException ex)
            {
                View.ShowMessage(ex.Message, language["TileTypeNotMatchTitle"], false);
            }
            catch (UnauthorizedAccessException)
            {
                View.ShowMessage(App.GeneralLanguage["AdminTip"], language["ImportFailedTitle"], false);
            }
            catch (IOException ex)
            {
                View.ShowMessage(ex.Message, language["ImportFailedTitle"], false);
            }
#if !DEBUG
            catch (Exception ex)
            {
                View.ShowMessage(ex.Message, language["ImportFailedTitle"], false);
            }
#endif
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
                        DereferenceLinks = true,
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

                        bool loadVoxel = Settings.Json[nameof(NonscalableTile)].ObjectValue["AutoLoadVoxelFile"].BooleanValue ?? false;
                        if (loadVoxel)
                        {
                            string voxelFileName = targetPath.RemoveExtension() + ".voxel";
                            if (File.Exists(voxelFileName))
                            {
                                tileManager.Path = voxelFileName;
                                updateFromTileManager();
                                return;
                            }
                        }

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

                        bool loadVoxel = Settings.Json[nameof(NonscalableTile)].ObjectValue["AutoLoadVoxelFile"].BooleanValue ?? false;
                        if (loadVoxel)
                        {
                            string voxelFileName = targetPath.NoBackslash() + ".voxel";
                            if (File.Exists(voxelFileName))
                            {
                                tileManager.Path = voxelFileName;
                                updateFromTileManager();
                                return;
                            }
                        }

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
                        void showDialog()
                        {
                            if (colorPicker.ShowDialog() ?? false)
                            {
                                BackColor = colorPicker.SelectedColor;
                            }
                        }

                        bool previewOnTile = Settings.Json[nameof(NonscalableTile)].ObjectValue[nameof(ColorPickerView)].ObjectValue["PreviewOnTile"].BooleanValue ?? false;
                        if (previewOnTile)
                        {
                            var colorPickerViewModel = colorPicker.DataContext as ColorPickerViewModel;
                            var originalColorBinding = gridPreview.GetBindingExpression(Control.BackgroundProperty).ParentBinding;
                            var colorPickerBinding = new Binding
                            {
                                Source = colorPickerViewModel,
                                Path = new PropertyPath(nameof(colorPickerViewModel.SelectedBrush)),

                            };

                            gridPreview.SetBinding(Control.BackgroundProperty, colorPickerBinding);
                            showDialog();
                            gridPreview.SetBinding(Control.BackgroundProperty, originalColorBinding);
                        }
                        else
                        {
                            showDialog();
                        }
                    }
                    
                },
            };
        public Command DefaultColorCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    BackColor = dwmColor;
                },
            };
        public Command SelectBackImageCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    bool verifyImage(string imagePath)
                    {
                        FileInfo info = new FileInfo(imagePath);
                        if (info.Length > Tile.MaxImageSize)
                        {
                            View.ShowMessage(language["ImageSizeTooBigContent"], language["ImageSizeTooBigTitle"], false);
                            return false;
                        }

                        var image = new BitmapImage(new Uri(imagePath));
                        if (image.PixelWidth > Tile.MaxImageDimensions ||
                            image.PixelHeight > Tile.MaxImageDimensions)
                        {
                            View.ShowMessage(language["ImageDimensionTooBigContent"], language["ImageDimensionTooBigTitle"], false);
                            return false;
                        }

                        return true;
                    }
                    var dialog = new OpenFileDialog
                    {
                        Title = language["OpenImageDialogTitle"],
                        DereferenceLinks = true,
                        Multiselect = false,
                        AddExtension = false,
                        CheckFileExists = true,
                        Filter = language["OpenImageDialogFilter"],
                    };
                    if (!IsTileSizeToggleChecked)
                    {
                        if (tileManager.Tile.LargeImagePath != null)
                        {
                            dialog.FileName = tileManager.Tile.LargeImagePath;
                        }
                        if (dialog.ShowDialog() ?? false)
                        {
                            string imagePath = dialog.FileName;

                            if (!verifyImage(imagePath))
                            {
                                return;
                            }

                            tileManager.Tile.LargeImagePath = imagePath;
                            BackImage = new BitmapImage(new Uri(imagePath));
                        }
                    }
                    else
                    {
                        if (tileManager.Tile.SmallImagePath != null)
                        {
                            dialog.FileName = tileManager.Tile.SmallImagePath;
                        }
                        if (dialog.ShowDialog() ?? false)
                        {
                            string imagePath = dialog.FileName;

                            if (!verifyImage(imagePath))
                            {
                                return;
                            }

                            tileManager.Tile.SmallImagePath = imagePath;
                            BackImageSmall = new BitmapImage(new Uri(imagePath));
                        }
                    }
                },
            };
        public Command ClearBackImageCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    if (IsTileSizeToggleChecked)
                    {
                        BackImageSmall = null;
                    }
                    else
                    {
                        BackImage = null;
                    }
                },
            };
        public Command GenerateCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    try
                    {
                        if (!File.Exists(tileManager.Tile.TargetPath))
                        {
                            View.ShowMessage(language["TargetMissing"], language["TargetMissingTitle"], false);
                        }
                        else
                        {
                            tileManager.Generate();
                            View.ShowMessage("", language["GenerateSuccessTitle"], false);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        View.ShowMessage(App.GeneralLanguage["AdminTip"], language["GenerateFailedTitle"], false);
                    }
#if !DEBUG
                    catch (Exception ex)
                    {
                        View.ShowMessage(ex.Message, language["GenerateFailedTitle"], false);
                    }
#endif
                },
            };
        public Command AddToStartCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    try
                    {
                        if (!File.Exists(tileManager.Tile.TargetPath))
                        {
                            View.ShowMessage(language["TargetMissing"], language["TargetMissingTitle"], false);
                            return;
                        }

                        if (tileManager.Tile.IsOnStartMenu
                        && !View.ShowMessage(language["OverwriteStartContent"], language["OverwriteStartTitle"], true))
                        {
                            return;
                        }
                        else
                        {
                            tileManager.AddToStart();
                            View.ShowMessage("", language["AddToStartSuccessTitle"], false);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        View.ShowMessage(App.GeneralLanguage["AdminTip"], language["AddToStartFailedTitle"], false);
                    }
#if !DEBUG
                        catch (Exception ex)
                        {
                            View.ShowMessage(ex.Message, language["AddToStartFailedTitle"], false);
                        }
#endif
                },
            };
        public Command ExportCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    if (!File.Exists(tileManager.Tile.TargetPath))
                    {
                        View.ShowMessage(language["TargetMissing"], language["TargetMissingTitle"], false);
                        return;
                    }

                    if (tileManager.Path == null)
                    {
                        tileManager.Path = tileManager.Tile.TargetPath.RemoveExtension() + ".voxel";
                    }
                    var dialog = new SaveFileDialog
                    {
                        Title = language["ExportDialogTitle"],
                        Filter = App.GeneralLanguage["VoxelDialogFilter"],
                        InitialDirectory = tileManager.Path.RemoveFileName(),
                        FileName = tileManager.Path.GetFileName(),
                        OverwritePrompt = true,
                        AddExtension = true,
                        DefaultExt = ".voxel",
                    };
                    if (dialog.ShowDialog() ?? false)
                    {
                        tileManager.Path = dialog.FileName;
                        try
                        {
                            tileManager.SaveData();
                        }
                        catch (UnauthorizedAccessException)
                        {
                            View.ShowMessage(App.GeneralLanguage["AdminTip"], language["ExportFailedTitle"], false);
                        }
                        catch (IOException ex)
                        {
                            View.ShowMessage(ex.Message, language["ExportFailedTitle"], false);
                        }
#if !DEBUG
                        catch (Exception ex)
                        {
                            View.ShowMessage(ex.Message, language["ExportFailedTitle"], false);
                        }
#endif
                    }
                },
            };
        public Command ImportCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    var dialog = new OpenFileDialog
                    {
                        Title = language["ImportDialogTitle"],
                        Filter = App.GeneralLanguage["VoxelDialogFilter"],
                        Multiselect = false,
                        DereferenceLinks = true,
                        CheckFileExists = true,
                        AddExtension = true,
                        DefaultExt = ".voxel",
                    };
                    if (dialog.ShowDialog() ?? false)
                    {
                        tileManager.Path = dialog.FileName;
                        updateFromTileManager();
                    }
                },
            };

        
        #endregion

    }
}