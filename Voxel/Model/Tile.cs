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
                return Environment.GetFolderPath(Environment.SpecialFolder.StartMenu).Backslash();
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

        private string xmlPath;
        public string XmlPath
        {
            get
            {
                return TargetPath + ".VisualElementsManifest.xml";
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

    }
}
