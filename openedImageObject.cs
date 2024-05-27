﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

namespace photoCuter
{
    internal class OpenedImageObject
    {
        BitmapImage openedImage;
        public BitmapImage OpenedImage {
            get 
            {
                return openedImage;
            }
            set
            {
                openedImage = value;
            } 
        }
        double width;
        public double Width
        {
            get
            {
                return width;
            }
        }
        public int WidthWithoutScale;
        double height;

        public double Height
        {
            get
            {
                return height;
            }
        }

        public int HeightWithoutScale;
        double x;
        public double X
        {
            get { return x; }
        }
        double y;
        public double Y
        {
            get { return y; }
        }

        float brightness = 0;
        public float Brightness
        {
            get
            {
                return brightness;
            }
            set
            {
                if(value<=10 && value>=-10)
                    brightness = value;
            }
        }

        float contrast = 0;
        public float Contrast
        {
                   
            get
            {
                    return contrast;
            }
            set
            {
                if (value <= 10 && value >= -10)
                    contrast = value;
            }
        }

        public OpenedImageObject()
        {
            contrast = 0;
            brightness = 0;
        }
        public ImageBrush putImage(BitmapImage image, double windowWidth, double windowHeight)
        {
            openedImage = image;
            Bitmap tempImage = OpenedImageObject.convertToBitmap(openedImage);
            WidthWithoutScale = tempImage.Width;
            HeightWithoutScale = tempImage.Height;

            updateImageSize(windowWidth, windowHeight);

            ImageBrush imageBrush = new ImageBrush(openedImage);
            imageBrush.Stretch = Stretch.Uniform;
            return imageBrush;
        }

        public void updateImageSize(double windowWidth, double windowHeight)
        {
            if (openedImage.Width > openedImage.Height && windowWidth / windowHeight < openedImage.Width / openedImage.Height)
            {
                width = windowWidth;
                x = 0;
                height = (openedImage.Height * windowWidth) / openedImage.Width;
                y = (windowHeight - height) / 2;
            }
            else
            {
                height = windowHeight;
                y = 0;
                width = (openedImage.Width * windowHeight) / openedImage.Height;
                x = (windowWidth - width) / 2;
            }
        }

        static public Bitmap convertToBitmap(BitmapImage Image)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(Image));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        static public BitmapImage convertToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
