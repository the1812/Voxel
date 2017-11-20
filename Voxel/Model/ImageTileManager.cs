using Ace;
using Ace.Files;
using Ace.Files.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Voxel.Model
{
    sealed class ImageTileManager : TileManager, IEnumerable<ImageTile>, ICollection<ImageTile>
    {
        private List<ImageTile> list;
        public ImageTileManager(IEnumerable<ImageTile> imageTiles)
        {
            list = new List<ImageTile>(imageTiles);
        }
        public ImageTileManager(params ImageTile[] imageTiles) : this(imageTiles as IEnumerable<ImageTile>) { }
        public List<ImageTile> TileList => list;

        private string GroupPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Backslash() + @"Voxel\" + GroupName.Backslash();
        private const string ImageName = "image.png";

        private string groupName;
        public string GroupName
        {
            get => groupName;
            set
            {
                groupName = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }
        private void copyExecutorTo(string path, string action)
        {
            var uri = new Uri(ImageTile.ActionExecutorName, UriKind.Relative);
            var reader = new BinaryReader(Application.GetResourceStream(uri).Stream);
            byte[] bytesData = reader.ReadBytes((int) reader.BaseStream.Length);
            using (var writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                writer.Write(bytesData);
            }
            JsonFile settings = new JsonFile(path)
            {
                Content = new JsonObject
                {
                    new JsonProperty(nameof(ImageTile.Action), action),
                }
            };
            settings.Save();
        }

        public override void Generate()
        {
            foreach (var tile in list)
            {
                string subFolder = GroupPath + tile.Name.Backslash();

                string executorPath = subFolder + ImageTile.ActionExecutorName;
                copyExecutorTo(executorPath, tile.Action);

                string imagePath = subFolder + tile.Name + ".png";
                var image = new UniversalImage(tile.Image);
                image.SaveImageSource(imagePath);

                XmlManager xml = new XmlManager();
                xml.FillFrom(tile);
                xml.Save(tile.XmlPath);
            }
        }
        public override void AddToStart()
        {
            foreach (var tile in list)
            {
                if (!tile.IsOnStartMenu)
                {
                    ShortcutFile file = new ShortcutFile(tile.StartMenuTargetPath)
                    {
                        TargetPath = tile.TargetPath,
                    };
                    file.Save();
                }
            }
        }

        public override void LoadData()
        {
            base.LoadData();
            if (data[TypeKey].StringValue != nameof(ImageTile))
            {
                throw new TileTypeNotMatchException();
            }
            try
            {
                GroupName = data[nameof(GroupName)].StringValue ?? throw new BadVoxelFileException();
                var tileArray = data[nameof(TileList)].ArrayValue;
                if (tileArray != null)
                {
                    var tileObjects = tileArray.Select(element => element.ObjectValue);
                    foreach (var tileObject in tileObjects)
                    {
                        var tile = new ImageTile(GroupName)
                        {
                            Action = tileObject[nameof(ImageTile.Action)].StringValue,
                            Background = ((int) data[nameof(ImageTile.Background)].NumberValue).ToColor(),
                            TargetPath = data[nameof(ImageTile.TargetPath)].StringValue,
                            Theme = (data[nameof(ImageTile.Theme)].StringValue ?? LightThemeString)
                                == DarkThemeString
                                ? TextTheme.Dark 
                                : TextTheme.Light,
                            TargetType = (data[nameof(ImageTile.TargetType)].StringValue ?? FileTargetString) 
                                == FolderTargetString 
                                ? TargetType.Folder 
                                : TargetType.File,
                            ShowName = data[nameof(ImageTile.ShowName)].BooleanValue.Value,
                            Name = data[nameof(ImageTile.Name)].StringValue,
                        };
                        list.Add(tile);
                    }
                }
                else
                {
                    throw new BadVoxelFileException();
                }
            }
            catch (KeyNotFoundException)
            {
                throw new BadVoxelFileException();
            }
        }
        public override void SaveData()
        {
            data[TypeKey] = nameof(ImageTile);
            data[nameof(GroupName)] = GroupName;
            List<JsonValue> tiles = new List<JsonValue>();
            foreach (var tile in list)
            {
                tiles.Add(new JsonObject
                {
                    [nameof(ImageTile.Action)] = tile.Action,
                    [nameof(ImageTile.Background)] = tile.Background.ToInt32(),
                    [nameof(ImageTile.TargetPath)] = tile.TargetPath,
                    [nameof(ImageTile.Theme)] = 
                        tile.Theme == TextTheme.Dark 
                        ? DarkThemeString 
                        : LightThemeString,
                    [nameof(ImageTile.TargetType)] = 
                        tile.TargetType == TargetType.Folder 
                        ? FolderTargetString 
                        : FileTargetString,
                    [nameof(ImageTile.ShowName)] = tile.ShowName,
                    [nameof(ImageTile.Name)] = tile.Name,
                });
            }
            data[nameof(TileList)] = new JsonArray(tiles);
            base.SaveData();
        }

        #region 接口
        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(ImageTile item) => list.Add(item);

        public void Clear() => list.Clear();

        public bool Contains(ImageTile item) => list.Contains(item);

        public void CopyTo(ImageTile[] array, int arrayIndex)
        => list.CopyTo(array, arrayIndex);

        public IEnumerator<ImageTile> GetEnumerator() => list.GetEnumerator();

        public bool Remove(ImageTile item) => list.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endregion

    }
}
