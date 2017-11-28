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
        private void updateProperties()
        {
            OnPropertyChanged(nameof(SelectedColor));
            OnPropertyChanged(nameof(Hex));
            OnPropertyChanged(nameof(Red));
            OnPropertyChanged(nameof(Green));
            OnPropertyChanged(nameof(Blue));
            OnPropertyChanged(nameof(Hue));
            OnPropertyChanged(nameof(Saturation));
            OnPropertyChanged(nameof(Brightness));
            OnPropertyChanged(nameof(SelectedBrush));
            OnPropertyChanged(nameof(RedValue));
            OnPropertyChanged(nameof(GreenValue));
            OnPropertyChanged(nameof(BlueValue));
            OnPropertyChanged(nameof(HueValue));
            OnPropertyChanged(nameof(SaturationValue));
            OnPropertyChanged(nameof(BrightnessValue));
        }
        private void updateHsbColor()
        {
            hsb = new HsbColor(selectedColor);
        }
        private void updateRgbColor()
        {
            selectedColor = hsb.ToRgbColor();
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
                updateProperties();
            }
        }
        public Brush SelectedBrush
        {
            get => new SolidColorBrush(SelectedColor);
        }
        public Color OldColor { get; set; }
        public Brush OldBrush => new SolidColorBrush(OldColor);
        public string Hex
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
                updateProperties();
            }
        }
        public string Red
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
                    updateHsbColor();
                }
                updateProperties();
            }
        }
        public string Blue
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
                    updateHsbColor();
                }
                updateProperties();
            }
        }
        public string Green
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
                    updateHsbColor();
                }
                updateProperties();
            }
        }
        public byte RedValue
        {
            get => selectedColor.R;
            set
            {
                selectedColor.R = value;
                updateHsbColor();
                updateProperties();
            }
        }
        public byte GreenValue
        {
            get => selectedColor.G;
            set
            {
                selectedColor.G = value;
                updateHsbColor();
                updateProperties();
            }
        }
        public byte BlueValue
        {
            get => selectedColor.B;
            set
            {
                selectedColor.B = value;
                updateHsbColor();
                updateProperties();
            }
        }

        private HsbColor hsb;
        public string Hue
        {
            get
            {
                return $"{hsb.Hue:0.0}";
            }
            set
            {
                if (value.IsMatch(@"[\d]{1,3}.[\d]{1}|[\d]{1,3}"))
                {
                    decimal h = value.ToDecimal();
                    while (h >= 360M)
                    {
                        h -= 360M;
                    }
                    while (h < 0M)
                    {
                        h += 360M;
                    }
                    hsb.Hue = h;
                    updateRgbColor();
                }
                updateProperties();

            }
        }
        public string Saturation
        {
            get
            {
                return $"{hsb.Saturation*100M:0.0}";
            }
            set
            {
                if (value.IsMatch(@"[\d]{1,3}.[\d]{1}|[\d]{1,3}"))
                {
                    decimal s = value.ToDecimal() / 100M;
                    if (s >= 0M && s <= 1M)
                    {
                        hsb.Saturation = s;
                        updateRgbColor();
                    }
                }
                updateProperties();

            }
        }
        public string Brightness
        {
            get
            {
                return $"{hsb.Brightness*100M:0.0}";
            }
            set
            {
                if (value.IsMatch(@"[\d]{1,3}.[\d]{1}|[\d]{1,3}"))
                {
                    decimal b = value.ToDecimal() / 100M;
                    if (b >= 0M && b <= 1M)
                    {
                        hsb.Brightness = b;
                        updateRgbColor();
                    }
                }
                updateProperties();

            }
        }
        public double HueValue
        {
            get => (double) hsb.Hue;
            set
            {
                hsb.Hue = (decimal) value;
                updateRgbColor();
                updateProperties();
            }
        }
        public double SaturationValue
        {
            get => (double) (hsb.Saturation * 100M);
            set
            {
                hsb.Saturation = (decimal) (value / 100.0);
                updateRgbColor();
                updateProperties();
            }
        }
        public double BrightnessValue
        {
            get => (double) (hsb.Brightness * 100M);
            set
            {
                hsb.Brightness = (decimal) (value / 100.0);
                updateRgbColor();
                updateProperties();
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
                        Hex = textBox.Text;
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
                        Red = textBox.Text;
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
                        Green = textBox.Text;
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
                        Blue = textBox.Text;
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
