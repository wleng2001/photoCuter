using System;
using System.Collections.Generic;
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
            return (byte)c;
        }
        static public Bitmap SetBrightness(Bitmap image, float bright,int multiplier)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            int m = (int)(multiplier * bright);

            void updatePixel0()
            {
                updatePixel(image, newImage, m, 4, 0);
            }

            void updatePixel1()
            {
                updatePixel(image, newImage, m, 4, 1);
            }

            void updatePixel2()
            {
                updatePixel(image, newImage, m, 4, 2);
            }

            void updatePixel3()
            {
                updatePixel(image, newImage, m, 4, 3);
            }
            
            Thread t0 = new Thread(updatePixel0);
            t0.Name = "t0";
            
            Thread t1 = new Thread(updatePixel1);
            t1.Name = "t1";
            
            Thread t2 = new Thread(updatePixel2);
            t2.Name = "t2";
            
            Thread t3 = new Thread(updatePixel3);
            t3.Name = "t3";
            
            t0.Start();
            t1.Start();
            t2.Start();
            t3.Start();

            
            t0.Join();
            t1.Join();
            t2.Join();
            t3.Join();

            //updatePixel(image, newImage, m, 1, 0);

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
