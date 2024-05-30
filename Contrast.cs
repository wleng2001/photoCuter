using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tools
{
    internal class Contrast
    {

        static public Bitmap setContrast(Bitmap image, float contrast)
        {
            Bitmap newBitmap = new Bitmap(image.Width, image.Height);
            contrast = (10 * contrast) / 10;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);

                    float red = pixel.R / 255.0f;
                    float green = pixel.G / 255.0f;
                    float blue = pixel.B / 255.0f;

                    red = (((red - 0.5f) * contrast) + 0.5f) * 255.0f;
                    green = (((green - 0.5f) * contrast) + 0.5f) * 255.0f;
                    blue = (((blue - 0.5f) * contrast) + 0.5f) * 255.0f;

                    int r = Math.Max(0, Math.Min(255, (int)red));
                    int g = Math.Max(0, Math.Min(255, (int)green));
                    int b = Math.Max(0, Math.Min(255, (int)blue));

                    newBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }
    }
}
