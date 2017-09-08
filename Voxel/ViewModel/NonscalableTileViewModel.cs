using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model;
using Voxel.Model.Languages;
using Ace;
using Microsoft.Win32;

namespace Voxel.ViewModel
{
    sealed class NonscalableTileViewModel : ViewModel
    {
        public NonscalableTileViewModel() : base(new NonscalableTileLanguage()) { }

#region Language
        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonTarget => language[nameof(ButtonTarget)];

        #endregion
#region Vars and properties
        private NonscalableTileManager tileManager = new NonscalableTileManager();
        //public NonscalableTileManager TileManager
        //{
        //    get => tileManager;
        //    set
        //    {
        //        tileManager = value;
        //        OnPropertyChanged(nameof(TileManager));
        //    }
        //}

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
        #endregion
        #region Commands

        public Command SelectTargetCommand
            => new Command
            {
                CanExecuteAction = (o) => true,
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
                    if (dialog.ShowDialog() ?? false)
                    {
                        tileManager.Tile.TargetPath = dialog.FileName;
                        tileManager.Tile.TargetType = TargetType.File;
                        OnPropertyChanged(nameof(TargetName));
                    }
                },
            };

#endregion

    }
}