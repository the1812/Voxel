using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Voxel.Model;

namespace Voxel.Controls
{
    sealed class ColorPickerHueSlider : Slider, INotifyPropertyChanged
    {
        static ColorPickerHueSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPickerHueSlider), new FrameworkPropertyMetadata(typeof(ColorPickerHueSlider)));
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(Color),
                typeof(ColorPickerHueSlider),
                new PropertyMetadata(Colors.Transparent));
        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set
            {
                SetValue(ColorProperty, value);
                //var color = new HsbColor(value);
                //var brush = new LinearGradientBrush()
                //{
                //    StartPoint = new Point(0, 0),
                //    EndPoint = new Point(360, 0),
                //};
                //for (decimal hue = 0M; hue <= 360M; hue += 60M)
                //{
                //    color.Hue = hue;
                //    var stop = new GradientStop(color, (double) hue);
                //    brush.GradientStops.Add(stop);
                //}
                //HueBrush = brush;
                onPropertyChanged(nameof(Color));
            }
        }

        //private static readonly DependencyPropertyKey HueBrushPropertyKey = DependencyProperty.RegisterReadOnly(
        //   nameof(HueBrush),
        //   typeof(Brush),
        //   typeof(ColorPickerHueSlider),
        //   new PropertyMetadata(null));
        //public static readonly DependencyProperty HueBrushProperty = HueBrushPropertyKey.DependencyProperty;
        //public Brush HueBrush
        //{
        //    get => (Brush) GetValue(HueBrushProperty);
        //    private set
        //    {
        //        SetValue(HueBrushPropertyKey, value);
        //        onPropertyChanged(nameof(Brush));
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
