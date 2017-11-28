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
        private const decimal RgbMax = 255M;

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
            decimal r = rgbColor.R / RgbMax;
            decimal g = rgbColor.G / RgbMax;
            decimal b = rgbColor.B / RgbMax;

            decimal max = Math.Max(Math.Max(r, g), b);
            decimal min = Math.Min(Math.Min(r, g), b);
            decimal delta = max - min;

            if (delta == 0M)
            {
                hue = 0M;
            }
            else if (max == r)
            {
                hue = 60M * (((g - b) / delta) % 6M);
            }
            else if (max == g)
            {
                hue = 60M * (((b - r) / delta) + 2M);
            }
            else // max == b
            {
                hue = 60M * (((r - g) / delta) + 4M);
            }
            while (hue >= 360M)
            {
                hue -= 360M;
            }
            while (hue < 0M)
            {
                hue += 360M;
            }

            if (max == 0M)
            {
                saturation = 0M;
            }
            else
            {
                saturation = delta / max;
            }

            brightness = max;
        }

        public Color ToRgbColor()
        {
            decimal c = brightness * saturation;
            decimal h = hue / 60M;
            decimal x = c * (1M - Math.Abs((h % 2M) - 1M));

            decimal r, g, b;
            if (0M <= h && h <= 1M)
            {
                r = c;
                g = x;
                b = 0M;
            }
            else if (h <= 2M)
            {
                r = x;
                g = c;
                b = 0M;
            }
            else if (h <= 3M)
            {
                r = 0M;
                g = c;
                b = x;
            }
            else if (h <= 4M)
            {
                r = 0M;
                g = x;
                b = c;
            }
            else if (h <= 5M)
            {
                r = x;
                g = 0M;
                b = c;
            }
            else if (h < 6M)
            {
                r = c;
                g = 0M;
                b = x;
            }
            else
            {
                r = g = b = 0M;
            }
            decimal m = brightness - c;
            r += m;
            g += m;
            b += m;
            
            return Color.FromRgb(Convert.ToByte(r * RgbMax), Convert.ToByte(g * RgbMax), Convert.ToByte(b * RgbMax));
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
