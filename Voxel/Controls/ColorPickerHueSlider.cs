using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Voxel.Model;

namespace Voxel.Controls
{
    sealed class ColorPickerHueSlider : Slider, INotifyPropertyChanged
    {
        static ColorPickerHueSlider() 
            => DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPickerHueSlider), new FrameworkPropertyMetadata(typeof(ColorPickerHueSlider)));
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(Color),
                typeof(ColorPickerHueSlider),
                new PropertyMetadata(Colors.Transparent));
        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set
            {
                SetValue(ColorProperty, value);
                onPropertyChanged(nameof(Color));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            var position = e.GetPosition(this);
            position.X -= BorderThickness.Left + Padding.Left;
            var width = ActualWidth - BorderThickness.Left - BorderThickness.Right
                                    - Padding.Left - Padding.Right;
            var value = Maximum * (position.X / width);
            if (value > Maximum)
            {
                value = Maximum;
            }
            if (value < Minimum)
            {
                value = Minimum;
            }
            if (Math.Abs(Value - value) >= 100.0 * 15.0 / 2.0 / 233.0) // outside of thumb button
            {
                Value = value;
                e.Handled = true;
            }
        }
    }
}
