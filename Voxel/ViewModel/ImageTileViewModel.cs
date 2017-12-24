using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Voxel.Controls;
using Voxel.Model;
using Voxel.Model.Languages;
using Voxel.View;

namespace Voxel.ViewModel
{
    sealed class ImageTileViewModel : ViewModel
    {
        public ImageTileViewModel(ImageTileView view) : base(new ImageTileLanguage()) => View = view;
        #region Language
        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonGenerate => language[nameof(ButtonGenerate)];
        public string ButtonAddToStart => language[nameof(ButtonAddToStart)];
        public string ButtonExport => language[nameof(ButtonExport)];
        public string ButtonImport => language[nameof(ButtonImport)];
        public string ButtonSelectImage => language[nameof(ButtonSelectImage)];
        public string CheckBoxKeepRatio => language[nameof(CheckBoxKeepRatio)];
        public string ButtonSetAction => language[nameof(ButtonSetAction)];
        public string RadioButtonActionNone => language[nameof(RadioButtonActionNone)];
        public string RadioButtonActionFile => language[nameof(RadioButtonActionFile)];
        public string RadioButtonActionFolder => language[nameof(RadioButtonActionFolder)];
        public string RadioButtonActionUrl => language[nameof(RadioButtonActionUrl)];
        public string ButtonBackColor => language[nameof(ButtonBackColor)];
        #endregion
        #region Vars and properties

        private ImageTileAction action = new ImageTileAction(ActionType.None);
        private ImageTileManager manager = new ImageTileManager();


        public ImageTileView View { get; private set; }
        public void ClearData()
        {
            
        }
        public Stretch ImageStretch
        {
            get
            {
                if (KeepAspectRatio)
                {
                    return Stretch.Fill;
                }
                else
                {
                    return Stretch.Uniform;
                }
            }
        }

        private bool keepAspectRatio = true;
        public bool KeepAspectRatio
        {
            get => keepAspectRatio;
            set
            {
                keepAspectRatio = value;
                OnPropertyChanged(nameof(KeepAspectRatio));
            }
        }

        private string originalImagePath = null;
        private BitmapSource originalImage = null;
        public BitmapSource OriginalImage
        {
            get => originalImage;
            set
            {
                originalImage = value;
                OnPropertyChanged(nameof(OriginalImage));
            }
        }

        private ObservableCollection<ImageSpliter> spliters = new ObservableCollection<ImageSpliter>();
        public ObservableCollection<ImageSpliter> Spliters => spliters;

        private double previewWidth = 0.0;
        public double PreviewWidth
        {
            get => previewWidth;
            set
            {
                previewWidth = value;
                OnPropertyChanged(nameof(PreviewWidth));
            }
        }

        private double previewHeight = 0.0;
        public double PreviewHeight
        {
            get => previewHeight;
            set
            {
                previewHeight = value;
                OnPropertyChanged(nameof(PreviewHeight));
            }
        }

        private BitmapSource testImage;
        public BitmapSource TestImage
        {
            get => testImage;
            set
            {
                testImage = value;
                OnPropertyChanged(nameof(TestImage));
            }
        }
        private void createPreview(Size gridSize)
        {
            //Calculate preview size
            var previewSize = new Size(
                gridSize.Width * TileSize.LargeWidthAndHeight,
                gridSize.Height * TileSize.LargeWidthAndHeight);
            previewSize.Width += (gridSize.Width - 1) * TileSize.Gap;
            previewSize.Height += (gridSize.Height - 1) * TileSize.Gap;
            PreviewWidth = previewSize.Width;
            PreviewHeight = previewSize.Height;

            //Create ImageSpliters
            var dictionary = OriginalImage.SmartSplit(gridSize);
            Spliters.Clear();
            for (var row = 0; row < gridSize.Height; row++)
            {
                for (var column = 0; column < gridSize.Width; column++)
                {
                    var spliter = new ImageSpliter
                    {
                        BitmapSource = dictionary[new Point(column, row)],
                        Margin = new Thickness
                        {
                            Left = column * (TileSize.LargeWidthAndHeight + TileSize.Gap),
                            Top = row * (TileSize.LargeWidthAndHeight + TileSize.Gap),
                            Right = 0,
                            Bottom = 0,
                        },
                        IsSplit = false,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Width = TileSize.LargeWidthAndHeight,
                        Height = TileSize.LargeWidthAndHeight,
                    };
                    Spliters.Add(spliter);
                }
            }
        }

        #endregion
        #region Commands

        public BindingCommand ChangeActionCommand => new BindingCommand
        {
            ExcuteAction = (o) =>
            {
                if (o is RadioButton radioButton)
                {
                    switch (radioButton.Content as string)
                    {
                        case nameof(ActionType.None):
                            action.Type = ActionType.None;
                            break;
                        case nameof(ActionType.File):
                            action.Type = ActionType.File;
                            break;
                        case nameof(ActionType.Folder):
                            action.Type = ActionType.Folder;
                            break;
                        case nameof(ActionType.Url):
                            action.Type = ActionType.Url;
                            break;
                        default:
                            break;
                    }
                }
            },
        };
        public BindingCommand SelectImageCommand => new BindingCommand
        {
            ExcuteAction = (o) =>
            {
                var dialog = new OpenFileDialog
                {
                    Title = language["OpenImageDialogTitle"],
                    Multiselect = false,
                    DereferenceLinks = true,
                    CheckFileExists = true,
                    Filter = language["OpenImageDialogFilter"],
                };
                if (File.Exists(originalImagePath))
                {
                    dialog.FileName = originalImagePath;
                }
                if (dialog.ShowDialog() ?? false)
                {
                    originalImagePath = dialog.FileName;
                    OriginalImage = new BitmapImage(new Uri(originalImagePath));
#warning "Test data: Size(3,2)"
                    var gridSize = new Size(3, 2);
                    createPreview(gridSize);
                }
            },
        };
        #endregion
    }
}
