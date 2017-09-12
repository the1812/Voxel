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
            get => backImage;
            set
            {
                backImage = value;
                OnPropertyChanged(nameof(BackImage));
                OnPropertyChanged(nameof(IconVisibility));
            }
        }
        public Visibility IconVisibility
        {
            get
            {
                if (BackImage == null)
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
                        void showDialog()
                        {
                            if (colorPicker.ShowDialog() ?? false)
                            {
                                BackColor = colorPicker.SelectedColor;
                            }
                        }
                        

                        //bool showSampleText = Settings.Json[nameof(NonscalableTile)].ObjectValue[nameof(ColorPickerView)].ObjectValue["ShowSampleText"].BooleanValue ?? false;

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
                    var dialog = new OpenFileDialog
                    {
                        Title = language["OpenImageDialogTitle"],
                        Multiselect = false,
                        AddExtension = false,
                        CheckFileExists = true,
                        Filter = language["OpenImageDialogFilter"],
                    };
                    if (tileManager.Tile.LargeImagePath != null)
                    {
                        dialog.FileName = tileManager.Tile.LargeImagePath;
                    }
                    if (dialog.ShowDialog() ?? false)
                    {
                        string imagePath = dialog.FileName;
                        tileManager.Tile.LargeImagePath = imagePath;
                        BackImage = new BitmapImage(new Uri(imagePath));
                    }
                },
            };
        public Command ClearBackImageCommand
            => new Command
            {
                ExcuteAction = (o) =>
                {
                    BackImage = null;
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
                            tileManager.Path = tileManager.Tile.TargetPath.RemoveFileName();
                            tileManager.Generate();
                            View.ShowMessage("", language["GenerateSuccessTitle"], false);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        View.ShowMessage(language["AdminTip"], language["GenerateFailedTitle"], false);
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
                        View.ShowMessage(language["AdminTip"], language["AddToStartFailedTitle"], false);
                    }
#if !DEBUG
                        catch (Exception ex)
                        {
                            View.ShowMessage(ex.Message, language["AddToStartFailedTitle"], false);
                        }
#endif
                },
            };

#endregion

    }
}