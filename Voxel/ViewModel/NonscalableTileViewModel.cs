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
    sealed partial class NonscalableTileViewModel : ViewModel, IBusyState, IWaitingState
    {
        public NonscalableTileViewModel(NonscalableTileView view) : base(new NonscalableTileLanguage()) => View = view;
        
        private NonscalableTileManager tileManager = new NonscalableTileManager();
        public NonscalableTileView View { get; private set; }

        private string TargetFileName => tileManager.Tile.TargetPath?.GetFileName().RemoveExtension();
        private string TargetFolderName
        {
            get
            {
                var targetPath = tileManager.Tile.TargetPath?.NoQuotes().NoBackslash();
                if (targetPath == null)
                {
                    return "";
                }

                var slashIndex = targetPath.LastIndexOf('\\');
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
                if (result == null)
                {
                    return null;
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
            get => showLongNameMark && showName;
            set
            {
                showLongNameMark = value;
                OnPropertyChanged(nameof(ShowLongNameMark));
            }
        }

        public void DwmColorChanged(Color newColor)
        {
            dwmColor = newColor;
            if (isLinkedToDwmColor)
            {
                BackColor = newColor;
            }
        }
        private bool isLinkedToDwmColor = true;
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
            get => !isBusy && showName;
            set
            {
                showName = value;
                tileManager.Tile.ShowName = value;
                OnPropertyChanged(nameof(ShowName));
                OnPropertyChanged(nameof(ShowLongNameMark));
            }
        }

        private ImageSource backImageSmall = null;
        public ImageSource BackImageSmall
        {
            get => backImageSmall ?? backImage;
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
            => IsTileSizeToggleChecked ? Visibility.Collapsed : Visibility.Visible;
        public Visibility SmallPreviewVisibility 
            => IsTileSizeToggleChecked ? Visibility.Visible : Visibility.Collapsed;

        private void updateFromTileManager()
        {
            try
            {
                if (tileManager.Tile.TargetExists)
                {
                    loadIcon(tileManager.Tile.TargetPath);
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
        private void fillImagePath()
        {
            if (tileManager.Tile.LargeImagePath == null)
            {
                if (tileManager.Tile.SmallImagePath == null)
                {
                    return;
                }
                else
                {
                    tileManager.Tile.LargeImagePath = tileManager.Tile.SmallImagePath;
                }
            }
            else
            {
                if (tileManager.Tile.SmallImagePath == null)
                {
                    tileManager.Tile.SmallImagePath = tileManager.Tile.LargeImagePath;
                }
            }
        }
        private string getVoxelFileName(string targetPath)
        {
            var voxelFileName = tileManager.Tile.TargetType == TargetType.File ?
                                targetPath.RemoveExtension() + ".voxel" :
                                targetPath.NoBackslash() + ".voxel";
            return voxelFileName;
        }
        private bool tryLoadXml()
        {
            var loadXml = GetBoolean(MakeKey(nameof(NonscalableTile), AutoLoadXmlKey));
            if (loadXml && File.Exists(tileManager.Tile.XmlPath))
            {
                tileManager.LoadFromXml();
                updateFromTileManager();
                return true;
            }
            return false;
        }
        private bool tryLoadVoxel(string targetPath)
        {
            var loadVoxel = GetBoolean(MakeKey(nameof(NonscalableTile), AutoLoadVoxelFileKey));
            if (loadVoxel)
            {
                var voxelFileName = getVoxelFileName(targetPath);
                if (File.Exists(voxelFileName))
                {
                    try
                    {
                        tileManager.Path = voxelFileName;
                        tileManager.LoadData();
                        updateFromTileManager();
                        return true;
                    }
                    catch (BadVoxelFileException)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        private void loadIcon(string targetPath)
        {
            void loadByShellImageList()
            {
                var iconSize = Ace.Win32.Enumerations.IconSize.Large;
                if (MainView.Dpi.X > 1)
                {
                    iconSize = Ace.Win32.Enumerations.IconSize.Maximum;
                }
                Icon = Ace.Win32.Api.GetIcon(targetPath, iconSize);
            }
            AceIcon icon = null;
            try
            {
                if (tileManager.Tile.TargetType == TargetType.File)
                {
                    icon = new IconFile(targetPath).Load().Icon;
                }
                else
                {
                    icon = AceIcon.GetFolderIcon(targetPath);
                }
            }
            catch
            {
                loadByShellImageList();
                //View.ShowMessage(language["IconLoadFailed"], language["IconLoadFailedTitle"], false);
                return;
            }

            var size = TileSize.IconSize;
            size.Width *= MainView.Dpi.X;
            size.Height *= MainView.Dpi.Y;
            var images = from image in icon
                         where image.Width >= size.Width && image.Height >= size.Height
                         orderby image.Width
                         select image;
            var suitableImage = images.FirstOrDefault();
            if (suitableImage == null)
            {
                try
                {
                    Icon = icon[size.Width, false];
                }
                catch (BadIconFileException)
                {
                    loadByShellImageList();
                }
            }
            else
            {
                Icon = suitableImage;
            }
        }

        private void reset()
        {
            tileManager = new NonscalableTileManager();
            ClearBackImageCommand.Execute(null);
            DefaultColorCommand.Execute(null);
            IsDarkTheme = false;
            ShowName = true;
            IsTileSizeToggleChecked = false;
            OnPropertyChanged(nameof(TargetName));
            OnPropertyChanged(nameof(TargetFileName));
            OnPropertyChanged(nameof(TargetFolderName));
        }
        public void ClearData()
        {
            Icon = null;
            reset();
        }


        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(ShowName));
            }
        }

        private bool isWaiting;
        public bool IsWaiting
        {
            get => isWaiting;
            set
            {
                isWaiting = value;
                OnPropertyChanged(nameof(IsWaiting));
            }
        }
    }
}