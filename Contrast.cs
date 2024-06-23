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
    internal class Contrast
    {
        static object monitorClass = new object();
        static public Bitmap setContrast(Bitmap image, float contrast, int threads)
        {
            Bitmap newBitmap = new Bitmap(image.Width, image.Height);
            contrast = (10 * contrast) / 10;

            Thread[] threadTab = new Thread[threads];
            for(int i = 0; i<threads; i++)
            {
                int localI = i;
                void updatePixelX()
                {
                    updatePixel(image, newBitmap, contrast, threads, localI);
                }
                threadTab[i] = new Thread(updatePixelX);
                threadTab[i].Start();
            }

            foreach(Thread t in threadTab)
            {
                t.Join();
            }

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
                        newBitmap.SetPixel(j, i, Color.FromArgb(pxl.A, r, g, b));
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
