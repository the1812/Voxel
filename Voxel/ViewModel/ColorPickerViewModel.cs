using Ace;
using Ace.Files.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Voxel.Model;
using Voxel.Model.Languages;
using Voxel.View;

namespace Voxel.ViewModel
{
    sealed class ColorPickerViewModel : ViewModel
    {
        public ColorPickerViewModel() : base(new ColorPickerLanguage()) { }

        #region Lanugage
        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonOK => language[nameof(ButtonOK)];
        public string ButtonCancel => language[nameof(ButtonCancel)];
        public string TextSample => language[nameof(TextSample)];
        public string CheckBoxShowSample => language[nameof(CheckBoxShowSample)];
        #endregion
        #region Vars and properties
        private static Color dwmColor = Ace.Wpf.DwmEffect.ColorizationColor;
        private Color selectedColor = dwmColor;
        public Color SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                OnPropertyChanged(nameof(SelectedColor));
                OnPropertyChanged(nameof(HexColor));
                OnPropertyChanged(nameof(RedColor));
                OnPropertyChanged(nameof(GreenColor));
                OnPropertyChanged(nameof(BlueColor));
                OnPropertyChanged(nameof(SelectedBrush));
            }
        }
        public Brush SelectedBrush
        {
            get => new SolidColorBrush(SelectedColor);
        }
        public string HexColor
        {
            get
            {
                return $"{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
            }
            set
            {
                string hex = value;
                if (hex.Length == 3)
                {
                    hex = $"{hex[0]}{hex[0]}{hex[1]}{hex[1]}{hex[2]}{hex[2]}";
                }

                if (hex.Length == 6 && hex.IsMatch("[A-Fa-f0-9]*"))
                {
                    var color = Color.FromArgb(
                        255,
                        Convert.ToByte(hex.Substring(0, 2), 16),
                        Convert.ToByte(hex.Substring(2, 2), 16),
                        Convert.ToByte(hex.Substring(4, 2), 16)
                        );
                    selectedColor = color;
                }
                OnPropertyChanged(nameof(SelectedColor));
                OnPropertyChanged(nameof(HexColor));
                OnPropertyChanged(nameof(RedColor));
                OnPropertyChanged(nameof(GreenColor));
                OnPropertyChanged(nameof(BlueColor));
                OnPropertyChanged(nameof(SelectedBrush));
            }
        }
        public string RedColor
        {
            get
            {
                return selectedColor.R.ToString();
            }
            set
            {
                if (value.IsMatch("[0-9]{1,3}")
                    && Convert.ToInt32(value) <= 255)
                {
                    selectedColor.R = Convert.ToByte(value);
                }
                OnPropertyChanged(nameof(SelectedColor));
                OnPropertyChanged(nameof(HexColor));
                OnPropertyChanged(nameof(RedColor));
                OnPropertyChanged(nameof(GreenColor));
                OnPropertyChanged(nameof(BlueColor));
                OnPropertyChanged(nameof(SelectedBrush));
            }
        }
        public string BlueColor
        {
            get
            {
                return selectedColor.B.ToString();
            }
            set
            {
                if (value.IsMatch("[0-9]{1,3}")
                    && Convert.ToInt32(value) <= 255)
                {
                    selectedColor.B = Convert.ToByte(value);
                }
                OnPropertyChanged(nameof(SelectedColor));
                OnPropertyChanged(nameof(HexColor));
                OnPropertyChanged(nameof(RedColor));
                OnPropertyChanged(nameof(GreenColor));
                OnPropertyChanged(nameof(BlueColor));
                OnPropertyChanged(nameof(SelectedBrush));
            }
        }
        public string GreenColor
        {
            get
            {
                return selectedColor.G.ToString();
            }
            set
            {
                if (value.IsMatch("[0-9]{1,3}")
                    && Convert.ToInt32(value) <= 255)
                {
                    selectedColor.G = Convert.ToByte(value);
                }
                OnPropertyChanged(nameof(SelectedColor));
                OnPropertyChanged(nameof(HexColor));
                OnPropertyChanged(nameof(RedColor));
                OnPropertyChanged(nameof(GreenColor));
                OnPropertyChanged(nameof(BlueColor));
                OnPropertyChanged(nameof(SelectedBrush));
            }
        }
        private static JsonProperty showSampleTextValue = 
            Settings.Json[nameof(NonscalableTile)]
            .ObjectValue[nameof(ColorPickerView)]
            .ObjectValue
            .Where(p => p.Name == Settings.ShowSampleTextKey)
            .FirstOrDefault();
        private bool showSampleText = showSampleTextValue.Value.BooleanValue ?? false;
        public bool ShowSampleText
        {
            get => showSampleText;
            set
            {
                showSampleText = value;
                showSampleTextValue.Value = value;
                OnPropertyChanged(nameof(ShowSampleText));
            }
        }
        #endregion
        #region Commands
        public BindingCommand HexEnterCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    if (o is TextBox textBox)
                    {
                        HexColor = textBox.Text;
                    }
                },
            };
        public BindingCommand RedEnterCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    if (o is TextBox textBox)
                    {
                        RedColor = textBox.Text;
                    }
                },
            };
        public BindingCommand GreenEnterCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    if (o is TextBox textBox)
                    {
                        GreenColor = textBox.Text;
                    }
                },
            };
        public BindingCommand BlueEnterCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    if (o is TextBox textBox)
                    {
                        BlueColor = textBox.Text;
                    }
                },
            };
        #endregion

    }
}
