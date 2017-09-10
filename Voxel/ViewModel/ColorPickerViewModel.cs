using Ace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Voxel.Model.Languages;

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
            }
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
                    OnPropertyChanged(nameof(SelectedColor));
                    OnPropertyChanged(nameof(HexColor));
                }
                else
                {
                    selectedColor = dwmColor;
                    OnPropertyChanged(nameof(SelectedColor));
                    OnPropertyChanged(nameof(HexColor));
                }
            }
        }
        #endregion
        #region Commands

        #endregion






    }
}
