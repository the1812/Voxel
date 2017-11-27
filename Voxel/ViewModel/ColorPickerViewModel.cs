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
        private void updatePriperties()
        {
            OnPropertyChanged(nameof(SelectedColor));
            OnPropertyChanged(nameof(HexColor));
            OnPropertyChanged(nameof(RedColor));
            OnPropertyChanged(nameof(GreenColor));
            OnPropertyChanged(nameof(BlueColor));
            OnPropertyChanged(nameof(Hue));
            OnPropertyChanged(nameof(Saturation));
            OnPropertyChanged(nameof(Brightness));
            OnPropertyChanged(nameof(SelectedBrush));
        }

        private static Color dwmColor = Ace.Wpf.DwmEffect.ColorizationColor;
        private Color selectedColor = dwmColor;
        public Color SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                hsb = new HsbColor(value);
                updatePriperties();
            }
        }
        public Brush SelectedBrush
        {
            get => new SolidColorBrush(SelectedColor);
        }
        public Color OldColor { get; set; }
        public Brush OldBrush => new SolidColorBrush(OldColor);
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
                    hsb = new HsbColor(color);
                }
                updatePriperties();
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
                    hsb = new HsbColor(selectedColor);
                }
                updatePriperties();
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
                    hsb = new HsbColor(selectedColor);
                }
                updatePriperties();
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
                    hsb = new HsbColor(selectedColor);
                }
                updatePriperties();
            }
        }

        private HsbColor hsb;
        public string Hue
        {
            get
            {
                return $"{hsb.Hue:0}";
            }
            set
            {
                if (value.IsMatch(@"[\d]{1,3}"))
                {
                    decimal h = value.ToDecimal();
                    if (h <= 360M)
                    {
                        hsb.Hue = h;
                        selectedColor = hsb.ToRgbColor();
                    }
                }
                updatePriperties();

            }
        }
        public string Saturation
        {
            get
            {
                return $"{hsb.Saturation*100M:0}";
            }
            set
            {
                if (value.IsMatch(@"[\d]{1,3}"))
                {
                    decimal s = value.ToDecimal() / 100M;
                    if (s <= 1M)
                    {
                        hsb.Saturation = s;
                        selectedColor = hsb.ToRgbColor();
                    }
                }
                updatePriperties();

            }
        }
        public string Brightness
        {
            get
            {
                return $"{hsb.Brightness*100M:0}";
            }
            set
            {
                if (value.IsMatch(@"[\d]{1,3}"))
                {
                    decimal b = value.ToDecimal() / 100M;
                    if (b <= 1M)
                    {
                        hsb.Brightness = b;
                        selectedColor = hsb.ToRgbColor();
                    }
                }
                updatePriperties();

            }
        }


        private bool isHsbMode;
        public bool IsHsbMode
        {
            get => isHsbMode;
            set
            {
                isHsbMode = value;
                OnPropertyChanged(nameof(IsHsbMode));
            }
        }

        //private static JsonProperty showSampleTextValue = 
        //    Settings.Json[nameof(NonscalableTile)]
        //    .ObjectValue[nameof(ColorPickerView)]
        //    .ObjectValue
        //    .Where(p => p.Name == Settings.ShowSampleTextKey)
        //    .FirstOrDefault();
        //private bool showSampleText = showSampleTextValue.Value.BooleanValue ?? false;
        //public bool ShowSampleText
        //{
        //    get => showSampleText;
        //    set
        //    {
        //        showSampleText = value;
        //        showSampleTextValue.Value = value;
        //        OnPropertyChanged(nameof(ShowSampleText));
        //    }
        //}
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
        public BindingCommand HueEnterCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    if (o is TextBox textBox)
                    {
                        Hue = textBox.Text;
                    }
                },
            };
        public BindingCommand SaturationEnterCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    if (o is TextBox textBox)
                    {
                        Saturation = textBox.Text;
                    }
                },
            };
        public BindingCommand BrightnessEnterCommand
            => new BindingCommand
            {
                ExcuteAction = (o) =>
                {
                    if (o is TextBox textBox)
                    {
                        Brightness = textBox.Text;
                    }
                },
            };
        #endregion

    }
}
