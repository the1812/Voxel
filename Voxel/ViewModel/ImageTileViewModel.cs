using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Voxel.Model;
using Voxel.Model.Languages;

namespace Voxel.ViewModel
{
    sealed class ImageTileViewModel : ViewModel
    {
        public ImageTileViewModel() : base(new ImageTileLanguage())
        {
        }
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


        private BitmapSource backImage;
        public BitmapSource BackImage
        {
            get => backImage;
            set
            {
                backImage = value;
                OnPropertyChanged(nameof(BackImage));
            }
        }



        #endregion
        #region Commands

        public Command ChangeActionCommand => new Command
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
        public Command SelectImageCommand => new Command
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

            },
        };

        #endregion
    }
}
