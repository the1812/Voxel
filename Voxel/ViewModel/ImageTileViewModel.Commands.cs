using Ace;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace Voxel.ViewModel
{
    partial class ImageTileViewModel
    {
        public BindingCommand SelectBackcolorCommand => new BindingCommand
        {
            ExcuteAction = o =>
            {
                var selectedTile = from spliter in Spliters
                                   where spliter.SelectedRadioButton != null
                                   select spliter.SelectedRadioButton;
                var colorPicker = new ColorPickerView(DefaultColor)
                {
                    SelectedColor = DefaultColor,
                    Owner = View,
                };
                Color showDialog()
                {
                    if (colorPicker.ShowDialog() ?? false)
                    {
                        var color = colorPicker.SelectedColor;
#warning "Missing DWM Color Link"
                        return color;
                    }
                    else
                    {
                        return DefaultColor;
                    }
                }
                if (selectedTile.Count() > 0) //set for selected tile
                {
                    var radioButton = selectedTile.First();
                    Debug.Assert(radioButton != null);

                }
                else //set for all tiles
                {
                    DefaultColor = showDialog();
                }
            },
        };
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
                    createPreview();
                }
            },
        };
        public BindingCommand ImageMarginEnterCommand => new BindingCommand
        {
            ExcuteAction = o =>
            {
                if (o is TextBox textBox)
                {
                    ImageMarginText = textBox.Text;
                }
            },
        };
        public BindingCommand PreviewGridWidthEnterCommand => new BindingCommand
        {
            ExcuteAction = o =>
            {
                if (o is TextBox textBox)
                {
                    PreviewGridWidthText = textBox.Text;
                }
            }
        };
        public BindingCommand PreviewGridHeightEnterCommand => new BindingCommand
        {
            ExcuteAction = o =>
            {
                if (o is TextBox textBox)
                {
                    PreviewGridHeightText = textBox.Text;
                }
            }
        };
    }
}
