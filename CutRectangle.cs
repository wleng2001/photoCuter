﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace photoCuter
{
    internal class CutRectangle
    {
        private double x;
        public double X
        {
            get { return x-minX; }
            set
            {
                if (value >= 0 && value+minX<=MaxX)
                {
                    x = value+minX;
                    Canvas.SetLeft(Rect, x);
                    Canvas.SetLeft(LeftUpThumb, x);
                    Canvas.SetLeft(RightDownThumb, x+Width-RightDownThumb.Width);
                }
            }
        }
        public int XWithoutScale
        {
            get
            {
                int w = openedImageObject.WidthWithoutScale;
                int tempX = (int)((X * w) / openedImageObject.Width);
                return tempX;
            }
        }

        private double minX;
        public double MinX
        {
            get { return minX; }
            set
            {
                if (value >= 0)
                    minX = value;
            }
        }
        public double MaxX;

        private double y;
        public double Y
        {
            get
            {
                return y-minY;
            }
            set
            {
                if(value >= 0 && value+minY <= MaxY)
                {
                    y = value+minY;
                    Canvas.SetTop(Rect, y);
                    Canvas.SetTop(LeftUpThumb, y);
                    Canvas.SetTop(RightDownThumb, y + Height - RightDownThumb.Height);
                }
            }
        }

        public int YWithoutScale
        {
            get
            {
                int h = openedImageObject.HeightWithoutScale;
                int tempY = (int)((Y * h) / openedImageObject.Height);
                return tempY;
            }
        }

        private double minY;
        public double MinY
        {
            get { return minY; }
            set 
            { 
                if(value >= 0)
                    minY = value; 
            }
        }
        public double MaxY;

        private double width;
        public double Width
        {
            get { return width; }
            set
            {
                if (value>0)
                {
                    Rect.Width = value;
                    width = value;
                    Rect.Width = Width;
                    Canvas.SetLeft(RightDownThumb, x+width- RightDownThumb.Width);
                }

            }
        }

        public int WidthWithoutScale
        {
            get
            {
                int w = openedImageObject.WidthWithoutScale;
                int tempWidth = (int)((Width * w) / openedImageObject.Width);
                return tempWidth;
            }
        }

        private double height;
        public double Height
        {
            get { return height; }
            set
            {
                if (value > 0)
                {
                    Rect.Height = value;
                    height = value;
                    Canvas.SetTop(RightDownThumb, y+height- RightDownThumb.Height);
                }
            }
        }

        public int HeightWithoutScale
        {
            get
            {
                int h = openedImageObject.HeightWithoutScale;
                int tempWidth = (int)((Height * h) / openedImageObject.Height);
                return tempWidth;
            }
        }
        public Canvas Canva;
        public OpenedImageObject openedImageObject;
        public Rectangle Rect;
        public Thumb LeftUpThumb;
        public Thumb RightDownThumb;

        public CutRectangle(OpenedImageObject openedImageObject, Canvas Canva, Rectangle Rect, Thumb LeftUpThumb, Thumb RightDownThumb)
        {
            this.openedImageObject = openedImageObject;
            this.Rect = Rect;
            this.LeftUpThumb = LeftUpThumb;
            this.RightDownThumb = RightDownThumb;
            this.Canva = Canva;
            X = openedImageObject.X;
            Y = openedImageObject.Y;
            Width = openedImageObject.Width;
            Height = openedImageObject.Height;
            updateOpenedImage();
        }

        public void putRectangle()
        {
            if (Rect != null && openedImageObject != null && LeftUpThumb != null && RightDownThumb != null)
            {
                double windowWidth = Canva.ActualWidth;
                double windowHeight = Canva.ActualHeight;
                Rect.Visibility = Visibility.Hidden;
                LeftUpThumb.Visibility = Visibility.Hidden;
                RightDownThumb.Visibility = Visibility.Hidden;

                updateOpenedImage();
                Width = openedImageObject.Width;
                Height = openedImageObject.Height;
                X = 0;
                Y = 0;

                Rect.Visibility = Visibility.Visible;
                LeftUpThumb.Visibility = Visibility.Visible;
                RightDownThumb.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Non definied some cutRectangle subobject");
            }
        }

        public void updateRectangle()
        {
            if (Rect != null && openedImageObject != null && LeftUpThumb != null && RightDownThumb != null)
            {
                double windowWidth = Canva.ActualWidth;
                double windowHeight = Canva.ActualHeight;
                Rect.Visibility = Visibility.Hidden;
                LeftUpThumb.Visibility = Visibility.Hidden;
                RightDownThumb.Visibility = Visibility.Hidden;

                double tempPhotoWidth = openedImageObject.Width;
                double tempPhotoHeight = openedImageObject.Height;
                double tempMinX = minX;
                double tempMinY = minY;
                updateOpenedImage();


                X = ((x-tempMinX) * openedImageObject.Width) / tempPhotoWidth;
                Y = ((y-tempMinY) * openedImageObject.Height) / tempPhotoHeight;

                Width = (openedImageObject.Width * Rect.Width) / tempPhotoWidth;
                Height = (openedImageObject.Height * Rect.Height) / tempPhotoHeight;
                Rect.Visibility = Visibility.Visible;
                LeftUpThumb.Visibility = Visibility.Visible;
                RightDownThumb.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Non definied some cutRectangle subobject");
            }
        }
        public void SetLeftUpCorner(double XChange, double YChange)
        {
            double y = Y + YChange;
            double x = X + XChange;
            if ((x >= 0) && Width - XChange >= 0)
            {
                Width = Width - XChange;
                X = x;
                
            }

            if ((y >= 0) && Height - YChange >= 0)
            {
                Height = Height - YChange;
                Y = y;
                

            }
        }

        public void SetRightDownCorner(double XChange, double YChange) 
        {
            double yc = Y + Height + YChange;
            double xc = X + Width  + XChange;
            if (xc>0 && xc<=MaxX-minX && Width+XChange>0)
            {
                Width = Width + XChange;


            }

            if (yc>0 && yc<= MaxY-minY && Height + YChange > 0)
            {
                Height = Height + YChange;
            }
        }

        public void updateOpenedImage()
        {
            openedImageObject.updateImageSize(Canva.ActualWidth, Canva.ActualHeight);
            MinX = openedImageObject.X;
            MinY = openedImageObject.Y;
            MaxX = openedImageObject.Width + openedImageObject.X;
            MaxY = openedImageObject.Height + openedImageObject.Y;
        }

        public void CutImage()
        {
            Bitmap oldImage = OpenedImageObject.convertToBitmap(openedImageObject.OpenedImage);
            int w = oldImage.Width;
            int h = oldImage.Height;
            int tempX = (int)((X * w) / openedImageObject.Width);
            int tempY = (int)((Y * h) / openedImageObject.Height);
            int tempWidth = (int)((Width * w) / openedImageObject.Width);
            int tempHeight = (int)((Height * h) / openedImageObject.Height);

            Bitmap newImage = CutImage(oldImage, tempX, tempY, tempWidth, tempHeight);
            
            Canva.Background = openedImageObject.putImage(OpenedImageObject.convertToBitmapImage(newImage), Canva.ActualWidth, Canva.ActualHeight);
            updateOpenedImage();
            
        }

        static public Bitmap CutImage(Bitmap image, int tempX, int tempY, int tempWidth, int tempHeight)
        {
            Bitmap newImage = new Bitmap(tempWidth, tempHeight);

            for (int i = 0; i < tempWidth; i++)
            {
                for (int j = 0; j < tempHeight; j++)
                {
                    if(image.Width<tempX+i-1 || image.Height < tempY + j-1)
                    {
                        continue;
                    }
                    Color pxl = image.GetPixel(tempX + i, (tempY + j));
                    newImage.SetPixel(i, j, pxl);
                }
            }
            return newImage;
        }
        
    }
}
