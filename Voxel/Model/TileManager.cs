using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.Files.Json;
using Ace.Files;
using Ace;
using System.IO;
using Voxel.ViewModel;

namespace Voxel.Model
{
    abstract class TileManager : NotificationObject
    {

        private string path;
        public string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
        protected JsonObject data;
        public virtual void SaveData()
        {
            JsonFile file = new JsonFile(Path)
            {
                Content = data,
            };
            file.Save();
        }
        public virtual void LoadData()
        {
            JsonFile file = new JsonFile(Path);
            file.Load();
            data = file.Content;
        }
        public abstract void AddToStart();
        public abstract void Generate();
        public abstract void RefreshShortcut();

        protected const string TypeKey = "Type";
        public const string DarkThemeString = "dark";
        public const string LightThemeString = "light";
        public const string FileTargetString = nameof(TargetType.File);
        public const string FolderTargetString = nameof(TargetType.Folder);
    }
}
