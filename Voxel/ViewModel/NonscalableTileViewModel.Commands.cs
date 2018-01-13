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
using static Voxel.Model.Settings;
using System.Windows.Media.Animation;
using Ace.Files.Icons;
using AceIcon = Ace.Files.Icons.Icon;

namespace Voxel.ViewModel
{
    partial class NonscalableTileViewModel
    {

        #region Commands

        public BindingCommand SelectFileCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    using (var busyController = new BusyStateController(this))
                    {
                        var dialog = new OpenFileDialog
                        {
                            Title = language["OpenFileDialogTitle"],
                            Multiselect = false,
                            DereferenceLinks = true,
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
                            var targetPath = dialog.FileName;
                            tileManager.Tile.TargetPath = targetPath;
                            tileManager.Tile.TargetType = TargetType.File;

                            if (tryLoadVoxel(targetPath) || tryLoadXml())
                            {
                                return;
                            }

                            loadIcon(targetPath);

                            OnPropertyChanged(nameof(TargetName));
                        }
                    }
                },
            };
        public BindingCommand SelectFolderCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    using (var busyController = new BusyStateController(this))
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
                            var targetPath = dialog.SelectedPath;
                            tileManager.Tile.TargetPath = targetPath;
                            tileManager.Tile.TargetType = TargetType.Folder;

                            if (tryLoadVoxel(targetPath) || tryLoadXml())
                            {
                                return;
                            }

                            loadIcon(targetPath);

                            OnPropertyChanged(nameof(TargetName));
                        }
                    }
                },
            };
        public BindingCommand SelectColorCommand
            => new BindingCommand
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
                                var color = colorPicker.SelectedColor;
                                if (color != dwmColor)
                                {
                                    isLinkedToDwmColor = false;
                                }
                                BackColor = color;
                            }
                        }

                        var rgbMode = GetBoolean(MakeKey(nameof(NonscalableTile), nameof(ColorPickerView), RgbModeKey));
                        var colorPickerViewModel = colorPicker.DataContext as ColorPickerViewModel;
                        colorPickerViewModel.IsHsbMode = !rgbMode;

                        var previewOnTile = GetBoolean(MakeKey(nameof(NonscalableTile), nameof(ColorPickerView), PreviewOnTileKey));
                        if (previewOnTile)
                        {
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

                        SetValue(
                            MakeKey(nameof(NonscalableTile), nameof(ColorPickerView), RgbModeKey),
                            !colorPickerViewModel.IsHsbMode);
                    }

                },
            };
        public BindingCommand DefaultColorCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    BackColor = dwmColor;
                    isLinkedToDwmColor = true;
                },
            };
        public BindingCommand SelectBackImageCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    using (var busyController = new BusyStateController(this))
                    {
                        bool verifyImage(string imagePath)
                        {
                            var info = new FileInfo(imagePath);
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
                                var imagePath = dialog.FileName;

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
                                var imagePath = dialog.FileName;

                                if (!verifyImage(imagePath))
                                {
                                    return;
                                }

                                tileManager.Tile.SmallImagePath = imagePath;
                                BackImageSmall = new BitmapImage(new Uri(imagePath));
                            }
                        }
                    }
                },
            };
        public BindingCommand ClearBackImageCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    //if (IsTileSizeToggleChecked)
                    //{
                    //    BackImageSmall = null;
                    //}
                    //else
                    //{
                    //    BackImage = null;
                    //}
                    BackImage = null;
                    BackImageSmall = null;
                },
            };
        public BindingCommand GenerateCommand
            => new BindingCommand
            {
                ExcuteAction = async (o) =>
                {
                    var startAnimation = View.TryFindResource("startWaiting") as Storyboard;
                    var stopAnimation = View.TryFindResource("stopWaiting") as Storyboard;

                    using (var busyController = new BusyStateController(this))
                    {
                        try
                        {
                            if (!tileManager.Tile.TargetExists)
                            {
                                View.ShowMessage(language["TargetMissing"], language["TargetMissingTitle"], false);
                            }
                            else
                            {
                                using (var waitingController = new WaitingStateController(this, () =>
                                {
                                    startAnimation.Begin();
                                },
                                () =>
                                {
                                    startAnimation.Stop();
                                    stopAnimation.Begin();
                                }))
                                {
                                    fillImagePath();
                                    await Task.Run(() =>
                                    {
                                        tileManager.Generate();
                                    });
                                    //await Task.Delay(2000);
                                    View.ShowMessage("", language["GenerateSuccessTitle"], false);
                                }
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
                    }
                },
            };
        public BindingCommand AddToStartCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    using (var busyController = new BusyStateController(this))
                    {
                        try
                        {
                            if (!tileManager.Tile.TargetExists)
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
                    }
                },
            };
        public BindingCommand ExportCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    using (var busyController = new BusyStateController(this))
                    {
                        if (!tileManager.Tile.TargetExists)
                        {
                            View.ShowMessage(language["TargetMissing"], language["TargetMissingTitle"], false);
                            return;
                        }

                        if (tileManager.Path == null)
                        {
                            tileManager.Path = getVoxelFileName(tileManager.Tile.TargetPath);
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
                                fillImagePath();
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
                    }
                },
            };
        public BindingCommand ImportCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    using (var busyController = new BusyStateController(this))
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

                            try
                            {
                                tileManager.LoadData();
                                updateFromTileManager();
                            }
                            catch (BadVoxelFileException ex)
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
                    }
                },
            };
        public BindingCommand ResetCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    var targetPath = tileManager.Tile.TargetPath;
                    var targetType = tileManager.Tile.TargetType;
                    reset();
                    tileManager.Tile.TargetPath = targetPath;
                    tileManager.Tile.TargetType = targetType;
                    OnPropertyChanged(nameof(TargetName));
                    OnPropertyChanged(nameof(TargetFileName));
                    OnPropertyChanged(nameof(TargetFolderName));
                },
            };
        #endregion

    }
}
