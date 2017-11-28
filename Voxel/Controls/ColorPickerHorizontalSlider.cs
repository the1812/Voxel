using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Voxel.Controls
{
    sealed class ColorPickerHorizontalSlider : Slider, INotifyPropertyChanged
    {
        static ColorPickerHorizontalSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPickerHorizontalSlider), new FrameworkPropertyMetadata(typeof(ColorPickerHorizontalSlider)));
        }

        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register(
                nameof(StartColor),
                typeof(Color),
                typeof(ColorPickerHorizontalSlider),
                new PropertyMetadata(Colors.Transparent));
        public Color StartColor
        {
            get { return (Color) GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); onPropertyChanged(nameof(EndColor)); }
        }
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register(
                nameof(EndColor),
                typeof(Color),
                typeof(ColorPickerHorizontalSlider),
                new PropertyMetadata(Colors.Transparent));
        public Color EndColor
        {
            get { return (Color) GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); onPropertyChanged(nameof(EndColor)); }
        }
        //public static readonly DependencyProperty MiddleColorProperty =
        //    DependencyProperty.Register(
        //        nameof(MiddleColor),
        //        typeof(Color),
        //        typeof(ColorPickerHorizontalSlider),
        //        new PropertyMetadata(Colors.Transparent));
        //public Color MiddleColor
        //{
        //    get { return (Color) GetValue(MiddleColorProperty); }
        //    set { SetValue(MiddleColorProperty, value); onPropertyChanged(nameof(MiddleColor)); }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
