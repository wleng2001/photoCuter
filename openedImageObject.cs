using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace photoCuter
{
    internal class openedImageObject
    {
        BitmapImage openedImage {
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
        double height;

        public double Height
        {
            get
            {
                return height;
            }
        }
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
        public float brightness
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
        public float contrast
        {
                   
            get
            {
                    return brightness;
            }
            set
            {
                if (value <= 10 && value >= -10)
                    contrast = value;
            }
        }

        openedImageObject()
        {
            contrast = 0;
            brightness = 0;
        }
        public ImageBrush putImage(BitmapImage image, double windowWidth, double windowHeight)
        {
            openedImage = image;

            updateImageSize(windowWidth, windowHeight);

            ImageBrush imageBrush = new ImageBrush(image);
            imageBrush.Stretch = Stretch.Uniform;
            return imageBrush;
        }

        public void updateImageSize(double windowWidth, double windowHeight)
        {
            if (openedImage.Width > openedImage.Height && windowWidth / windowHeight < openedImage.Width / bI.Height)
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
    }
}
