using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Voxel.Model
{
    struct HsbColor //Color convertion: https://zh.wikipedia.org/wiki/HSL和HSV色彩空间
    {
        private decimal hue, saturation, brightness;

        public decimal Hue { get => hue; set => hue = value; }
        public decimal Saturation { get => saturation; set => saturation = value; }
        public decimal Brightness { get => brightness; set => brightness = value; }

        public HsbColor(decimal hue, decimal saturation, decimal brightness)
        {
            this.hue = hue;
            this.saturation = saturation;
            this.brightness = brightness;
        }
        public HsbColor(Color rgbColor)
        {
            const decimal RgbMax = 255M;
            decimal r = rgbColor.R / RgbMax;
            decimal g = rgbColor.G / RgbMax;
            decimal b = rgbColor.B / RgbMax;

            decimal max = Math.Max(Math.Max(r, g), b);
            decimal min = Math.Min(Math.Min(r, g), b);

            if (max == min)
            {
                hue = 0M;
            }
            else if (max == r)
            {
                hue = 60M * ((g - b) / (max - min));
            }
            else if (max == g)
            {
                hue = 60M * ((b - r) / (max - min)) + 120M;
            }
            else // max == b
            {
                hue = 60M * ((r - g) / (max - min)) + 240M;
            }

            if (max == 0M)
            {
                saturation = 0M;
            }
            else
            {
                saturation = 1M - min / max;
            }

            brightness = max;
        }

        public Color ToRgbColor()
        {
            decimal h = hue % 6M;
            decimal f = hue / 60M - h;
            decimal p = brightness * (1M - saturation);
            decimal q = brightness * (1M - f * saturation);
            decimal t = brightness * (1M - (1M - f) * saturation);

            byte r, g, b;
            switch (h)
            {
                default:
                case 0M:
                    r = decimal.ToByte(brightness);
                    g = decimal.ToByte(t);
                    b = decimal.ToByte(p);
                    break;
                case 1M:
                    r = decimal.ToByte(q);
                    g = decimal.ToByte(brightness);
                    b = decimal.ToByte(p);
                    break;
                case 2M:
                    r = decimal.ToByte(p);
                    g = decimal.ToByte(brightness);
                    b = decimal.ToByte(t);
                    break;
                case 3M:
                    r = decimal.ToByte(brightness);
                    g = decimal.ToByte(t);
                    b = decimal.ToByte(p);
                    break;
                case 4M:
                    r = decimal.ToByte(p);
                    g = decimal.ToByte(q);
                    b = decimal.ToByte(brightness);
                    break;
                case 5M:
                    r = decimal.ToByte(brightness);
                    g = decimal.ToByte(p);
                    b = decimal.ToByte(q);
                    break;
            }
            return Color.FromRgb(r, g, b);
        }

        public static implicit operator Color(HsbColor hsbColor)
        {
            return hsbColor.ToRgbColor();
        }
        public static implicit operator HsbColor(Color rgbColor)
        {
            return new HsbColor(rgbColor);
        }
    }
}
