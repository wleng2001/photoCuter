using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace photoCuter
{
    internal class Brightness
    {
        private int multiplier = 10;
        public int Multiplier
        {
            get
            {
                return multiplier;
            }
            set
            {
                if (value > 0)
                {
                    multiplier = value;
                }
            }
        }

        public Brightness(int multiplier) 
        { 
            this.multiplier = multiplier;
        }

        private byte addNumber(int a, int b)
        {
            int c = a + b;
            if (c > 255)
            {
                c = 255;
            }
            return (byte)c;
        }
        public Bitmap SetBrightness(Bitmap image, float bright)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            for(int i = 0; i < image.Width; i++)
            {
                for(int j = 0; j < image.Height; j++)
                {
                    Color pxl = image.GetPixel(i, j);
                    int m = (int)(multiplier*bright);
                    int R = addNumber( m, pxl.R);
                    int G = addNumber(m, pxl.G);
                    int B = addNumber(m, pxl.B);
                    pxl = Color.FromArgb(pxl.A, R, G, B);
                    newImage.SetPixel(i,j,pxl);
                }
            }
            return newImage;
        }
    }
}
