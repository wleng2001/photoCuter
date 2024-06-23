using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace tools
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

        static object monitorObject0 = new object();
        static object monitorObject1 = new object();

        public Brightness(int multiplier) 
        { 
            this.multiplier = multiplier;
        }

        static private byte addNumber(int a, int b)
        {
            int c = a + b;
            if (c > 255)
            {
                c = 255;
            }
            if(c<0)
            {
                c = 0;
            }
            return (byte)c;
        }
        static public Bitmap SetBrightness(Bitmap image, float bright,int multiplier, int threads)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            int m = (int)(multiplier * bright);

            Thread[] threadTab = new Thread[threads];
            for (int i = 0; i < threads; i++)
            {
                int localI = i;
                void updatePixelX()
                {
                    updatePixel(image, newImage, m, threads, localI);
                }
                threadTab[i] = new Thread(updatePixelX);
                threadTab[i].Start();
            }

            foreach (Thread t in threadTab)
            {
                t.Join();
            }

            return newImage;

        }

        static private void updatePixel(Bitmap image, Bitmap newImage, int m, int whichPixel, int pixel)
        {

            int width;
            int height;
    
            try
            {
                Monitor.Enter(monitorObject0);
                width = image.Width;
                height = image.Height;
            }
            finally
            {
                Monitor.Exit(monitorObject0);
            }
            for (int i = 0; i < width; i++)
            {
                
                for (int j = pixel; j < height; j+=whichPixel)
                {
                    Color pxl;
                 
                    try
                    {
                        Monitor.Enter(monitorObject0);
                        pxl = image.GetPixel(i, j);
                    }
                    finally
                    {
                        Monitor.Exit(monitorObject0);
                    }

                    int R = addNumber(m, pxl.R);
                    int G = addNumber(m, pxl.G);
                    int B = addNumber(m, pxl.B);
                    pxl = Color.FromArgb(pxl.A, R, G, B);

                    
                    try
                    {
                        Monitor.Enter(monitorObject1);
                        newImage.SetPixel(i, j, pxl);
                    }
                    finally
                    {
                        Monitor.Exit(monitorObject1);
                    }

                }
            }
            
            
        }
    }


}
