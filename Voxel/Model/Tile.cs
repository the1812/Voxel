using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Ace;

namespace Voxel.Model
{
    public enum TextTheme
    {
        Light,
        Dark
    }
    public enum TargetType
    {
        File,
        Folder
    }
    abstract class Tile : NotificationObject
    {
        protected string StartMenu
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.StartMenu).Backslash() + "Programs".Backslash();
            }
        }
        public bool IsOnStartMenu => File.Exists(StartMenu + StartMenuTargetPath.GetFileName());
        public abstract string StartMenuTargetPath { get; }
        
        private Color background;
        public Color Background
        {
            get => background;
            set
            {
                background = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        private TextTheme theme;
        public TextTheme Theme
        {
            get => theme;
            set
            {
                theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        private bool showName;
        public bool ShowName
        {
            get => showName;
            set
            {
                showName = value;
                OnPropertyChanged(nameof(ShowName));
            }
        }

        //private string xmlPath;
        public string XmlPath
        {
            get
            {
                if (TargetType == TargetType.File)
                {
                    return TargetPath.RemoveExtension() + ".VisualElementsManifest.xml";
                }
                else
                {
                    return TargetPath.NoBackslash() + ".VisualElementsManifest.xml";
                }
            }
        }

        private string targetPath;
        public string TargetPath
        {
            get => targetPath;
            set
            {
                targetPath = value;
                OnPropertyChanged(nameof(TargetPath));
                OnPropertyChanged(nameof(XmlPath));
                OnPropertyChanged(nameof(StartMenuTargetPath));
            }
        }
        public bool TargetExists
        {
            get
            {
                if (TargetType == TargetType.File)
                {
                    return File.Exists(TargetPath);
                }
                else
                {
                    return Directory.Exists(TargetPath);
                }
            }
        }

        private string largeImagePath;
        public string LargeImagePath
        {
            get => largeImagePath;
            set
            {
                largeImagePath = value;
                OnPropertyChanged(nameof(LargeImagePath));
            }
        }

        private string smallImagePath;
        public string SmallImagePath
        {
            get => smallImagePath;
            set
            {
                smallImagePath = value;
                OnPropertyChanged(nameof(SmallImagePath));
            }
        }

        private TargetType targetType;
        public TargetType TargetType
        {
            get => targetType;
            set
            {
                targetType = value;
                OnPropertyChanged(nameof(TargetType));
            }
        }

        public const long MaxImageSize = 200 * 1024;
        public const int MaxImageDimensions = 1024;
    }
}
