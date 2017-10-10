using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model
{
    enum ActionType
    {
        None,
        File,
        Folder,
        Url,
    }
    class ImageTileAction : NotificationObject
    {
        public ImageTileAction(ActionType type)
        {
            Type = type;
        }

        private ActionType type;
        public ActionType Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }


        private string command;
        public string Command
        {
            get => command;
            set
            {
                command = value;
                OnPropertyChanged(nameof(Command));
            }
        }

    }
}
