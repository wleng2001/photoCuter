using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace tools
{
    internal class OperationOnImage
    {
        public String OperationType;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public double Value;
        public string StrValue
        {
            get
            {
                if(OperationType == CutImage)
                {
                    return Width + "X" + Height;
                }
                else
                {
                    float v = (float)Math.Round(Value, 1);
                    return v.ToString();
                }
            }
        }
        static public String CutImage { get; } = "CutImage";
        static public String Brightness { get; } = "Brightness";
        static public String Contrast { get; } = "Contrast";

        static public Bitmap MakeOperation(Bitmap image, OperationOnImage operation)
        {
            Bitmap newImage = image;

            switch (operation.OperationType)
            {
                case "Brightness":
                    newImage = tools.Brightness.SetBrightness(image, (float)operation.Value, 10);
                    break;
                case "Contrast":
                    newImage = tools.Contrast.setContrast(image, (float)operation.Value);
                    break;
                case "CutImage":
                    newImage = photoCuter.CutRectangle.CutImage(image, operation.X, operation.Y, operation.Width, operation.Height);
                    break;
            }
            return newImage;
        }
    }
}
