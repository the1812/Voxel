using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.Files.Json;

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
            file.Flush();
        }
        public virtual void LoadData()
        {
            JsonFile file = new JsonFile(Path);
            file.Read();
            data = file.Content;
        }
        protected const string TypeKey = "Type";
        protected const string DarkThemeString = nameof(Tile.TextTheme.Dark);
        protected const string LightThemeString = nameof(Tile.TextTheme.Light);
    }
}
