using Ace;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Voxel.Controls;
using Voxel.Model;
using Voxel.Model.Languages;
using Voxel.View;
using GdiPlus = System.Drawing;


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

        private int previewGridWidth;
        public int PreviewGridWidth
        {
            get => previewGridWidth;
            set
            {
                previewGridWidth = value;
                OnPropertyChanged(nameof(PreviewGridWidth));
                OnPropertyChanged(nameof(PreviewGridSize));
            }
        }

        private int previewGridHeight;
        public int PreviewGridHeight
        {
            get => previewGridHeight;
            set
            {
                previewGridHeight = value;
                OnPropertyChanged(nameof(PreviewGridHeight));
                OnPropertyChanged(nameof(PreviewGridSize));
            }
        }
        public Size PreviewGridSize => new Size(PreviewGridWidth, PreviewGridHeight);

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


        private static string marginRegex = @"([\d\.]+),([\d\.]+),([\d\.]+),([\d\.]+)|([\d\.]+),([\d\.]+)|([\d\.]+)";
        private bool tryParseMargin(string text, out Thickness margin)
        {
            var match = text.Match(@"([\d\.]+),([\d\.]+),([\d\.]+),([\d\.]+)");
            if (match.Success)
            {
                var left = match.Groups[1].Value.ToDouble();
                var top = match.Groups[2].Value.ToDouble();
                var right = match.Groups[3].Value.ToDouble();
                var bottom = match.Groups[4].Value.ToDouble();
                margin = new Thickness(left, top, right, bottom);
                return true;
            }
            match = text.Match(@"([\d\.]+),([\d\.]+)");
            if (match.Success)
            {
                var leftAndRight = match.Groups[1].Value.ToDouble();
                var topAndBottom = match.Groups[2].Value.ToDouble();
                margin = new Thickness(leftAndRight, topAndBottom, leftAndRight, topAndBottom);
                return true;
            }
            match = text.Match(@"([\d\.]+)");
            if (match.Success)
            {
                var uniformLength = match.Groups[0].Value.ToDouble();
                margin = new Thickness(uniformLength);
                return true;
            }
            margin = new Thickness(0, 0, 0, 0);
            return false;
        }
        private Thickness imageMargin;
        public Thickness ImageMargin
        {
            get => imageMargin;
            set
            {
                imageMargin = value;
                OnPropertyChanged(nameof(ImageMargin));
            }
        }
        public string ImageMarginText
        {
            get => ImageMargin.ToString();
            set
            {
                if (tryParseMargin(value, out var margin))
                {
                    ImageMargin = margin;
                }
                OnPropertyChanged(nameof(ImageMarginText));
            }
        }


        private BitmapSource fitGridSize(Size panelSize)
        {
            var panelRatio = panelSize.Ratio();
            var imageRatio = OriginalImage.Width / OriginalImage.Height;
            if (panelRatio >= imageRatio) //height equals
            {
                return OriginalImage.Zoom((panelSize.Height - ImageMargin.Top - ImageMargin.Bottom) / OriginalImage.Height);
            }
            else //width equals
            {
                return OriginalImage.Zoom((panelSize.Width - ImageMargin.Left - ImageMargin.Right) / OriginalImage.Width);
            }
        }
        private BitmapSource extend(BitmapSource image, Size newSize)
        {
            //newSize.Width *= MainView.Dpi.X;
            //newSize.Height *= MainView.Dpi.Y;
            var newImage = new GdiPlus.Bitmap(
                (int) (newSize.Width),
                (int) (newSize.Height));
            var gdiNewImage = GdiPlus.Graphics.FromImage(newImage);
            gdiNewImage.FillRectangle(new GdiPlus.SolidBrush(GdiPlus.Color.Transparent),
                0, 0, newImage.Width, newImage.Height);

            var marginedWidth = newImage.Width - ImageMargin.Left - ImageMargin.Right;
            var marginedHeight = newImage.Height - ImageMargin.Top - ImageMargin.Bottom;

            var x = ImageMargin.Left + marginedWidth / 2;
            x -= image.Width / 2;
            var y = ImageMargin.Top + marginedHeight / 2;
            y -= image.Height / 2;

            gdiNewImage.DrawImage(image.ToImage(), (float) x, (float) y, (float) image.Width, (float) image.Height);
            return newImage.ToImageSource() as BitmapSource;
        }


        private Dictionary<Point, CroppedBitmap> split(BitmapSource bitmap, Size gridSize)
        {
            var dpiX = MainView.Dpi.X;
            var dpiY = MainView.Dpi.Y;
            var dictionary = new Dictionary<Point, CroppedBitmap>();
            int x = 0, y = 0;
            for (var row = 0; row < gridSize.Height; row++)
            {
                for (var column = 0; column < gridSize.Width; column++)
                {
                    var croppintRect = new Int32Rect
                    {
                        X = x,
                        Y = y,
                        Width = (int) (TileSize.LargeWidthAndHeight * dpiX),
                        Height = (int) (TileSize.LargeWidthAndHeight * dpiY),
                    };
                    dictionary.Add(new Point(column, row), new CroppedBitmap
                    (
                        bitmap,
                        croppintRect
                    ));
                    x += (int) ((TileSize.LargeWidthAndHeight + TileSize.Gap) * dpiX);
                }
                x = 0;
                y += (int) ((TileSize.LargeWidthAndHeight + TileSize.Gap) * dpiX);
            }
            return dictionary;
        }
        private Dictionary<Point, CroppedBitmap> smartSplit(Size gridSize)
        {
            var desiredSize = new Size(
                        gridSize.Width * TileSize.LargeWidthAndHeight * MainView.Dpi.X,
                        gridSize.Height * TileSize.LargeWidthAndHeight * MainView.Dpi.Y);
            desiredSize.Width += (gridSize.Width - 1) * TileSize.Gap * MainView.Dpi.X;
            desiredSize.Height += (gridSize.Height - 1) * TileSize.Gap * MainView.Dpi.Y;

            var image = extend(fitGridSize(desiredSize), desiredSize);
            return split(image, gridSize);
        }


        private void createPreview()
        {
            var gridSize = PreviewGridSize;
            //Calculate preview size
            var previewSize = new Size(
                gridSize.Width * TileSize.LargeWidthAndHeight,
                gridSize.Height * TileSize.LargeWidthAndHeight);
            previewSize.Width += (gridSize.Width - 1) * TileSize.Gap;
            previewSize.Height += (gridSize.Height - 1) * TileSize.Gap;
            PreviewWidth = previewSize.Width;
            PreviewHeight = previewSize.Height;

            //Create ImageSpliters
            var dictionary = smartSplit(gridSize);
            Spliters.Clear();
            var shadow = new DropShadowEffect
            {
                ShadowDepth = 0,
                BlurRadius = 15,
                Opacity = 0.4,
            };
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
                        Effect = shadow,
                        Background = new SolidColorBrush(Ace.Wpf.DwmEffect.ColorizationColor),
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

#warning "Test data: Size(3,2) Margin=0"
                    PreviewGridWidth = 3;
                    PreviewGridHeight = 2;
                    ImageMargin = new Thickness(0);
                    
                    createPreview();
                }
            },
        };
        #endregion
    }
}
