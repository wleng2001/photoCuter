using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace photoCuter
{
    internal class OperationOnImage
    {
        public String OperationType;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public double Value;
        static public String CutImage { get; } = "CutImage";
        static public String Brightness { get; } = "Brightness";
        static public String Contrast { get; } = "Contrast";

        static public Bitmap MakeOperation(Bitmap image, OperationOnImage operation)
        {
            Bitmap newImage = image;

            switch (operation.OperationType)
            {
                case "Brightness":
                    break;
                case "Contrast":
                    break;
                case "CutImage":
                    newImage = CutRectangle.CutImage(image, operation.X, operation.Y, operation.Width, operation.Height);
                    break;
            }
            return newImage;
        }
    }
}
