using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

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
            double y = Y + YChange;
            double x = X + XChange;
            if (x>0 && x<=MaxX-minX && Width+XChange>0)
            {
                Width = Width + XChange;


            }

            if (y>0 && y<= MaxY-minY && Height + YChange > 0)
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
    }
}
