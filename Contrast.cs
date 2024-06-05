using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tools
{
    internal class Contrast
    {
        static object monitorClass = new object();
        static public Bitmap setContrast(Bitmap image, float contrast)
        {
            Bitmap newBitmap = new Bitmap(image.Width, image.Height);
            contrast = (10 * contrast) / 10;

            void updatePixel0()
            {
                updatePixel(image, newBitmap, contrast, 4, 0);
            }

            void updatePixel1()
            {
                updatePixel(image, newBitmap, contrast, 4, 1);
            }
            
            void updatePixel2()
            {
                updatePixel(image, newBitmap, contrast, 4, 2);
            }

            void updatePixel3()
            {
                updatePixel(image, newBitmap, contrast, 4, 3);
            }
            
            Thread t0 = new Thread(updatePixel0);
            t0.Start();
            Thread t1 = new Thread(updatePixel1);
            t1.Start();
            Thread t2 = new Thread(updatePixel2);
            t2.Start();
            Thread t3 = new Thread(updatePixel3);
            t3.Start();
            
            
            t0.Join();
            t1.Join();
            t2.Join();
            t3.Join(); 

            //updatePixel(image, newBitmap, contrast, 1, 0);

            return newBitmap;
        }

        static private void updatePixel(Bitmap image, Bitmap newBitmap, float contrast, int whichPixel, int pixel)
        {
            int width;
            int height;
            Color pxl;

            int combineValues(float value)
            {
                int v = (int)(contrast * (value - 127) + 127);
                if(v < 0)
                {
                    v = 0;
                }else if(v > 255)
                {
                    v = 255;
                }

                return v;
            }

            try
            {
                Monitor.Enter(image);
                width = image.Width;
                height = image.Height;
            }
            finally
            {
                Monitor.Exit(image);
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = pixel; j < width; j+=whichPixel)
                {
                   
                    try
                    {
                        Monitor.Enter(image);
                        pxl = image.GetPixel(j, i);
                    }
                    finally
                    {
                        Monitor.Exit(image);
                    }

                    int r = combineValues(pxl.R);
                    int g = combineValues(pxl.G);
                    int b = combineValues(pxl.B);

                    try
                    {
                        Monitor.Enter(monitorClass);
                        newBitmap.SetPixel(j, i, Color.FromArgb(r, g, b));
                    }
                    finally
                    {
                        Monitor.Exit(monitorClass);
                    }
                    
                }
            }

        }
    }
}
