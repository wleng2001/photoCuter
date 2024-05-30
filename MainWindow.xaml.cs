﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tools;

namespace photoCuter
{
    using Brushes = System.Windows.Media.Brushes;

    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 
    using OperationsOnImageList = List<tools.OperationOnImage>;
    public partial class MainWindow : Window
    {
        string[] files;
        OpenedImageObject openedImageObject;
        OperationsOnImageList[] operationsOnImages;
        OperationsOnImageList tempOperationsList;
        byte quantityOfOperationOnImage = 3;
        CutRectangle CutRectangleWithCorner;

        Bitmap tempBitmap;

        public MainWindow()
        {
            InitializeComponent();
        }

        void restartParameters()
        {
            
            if (openedImageObject != null)
            {
                //brightness
                brightnessSlider.Value = 0;
                settingBrightnessTextBox.Text = 0.ToString();
                //contrast
                contrastSlider.Value = 0;
                settingConstrastTextBox.Text = 0.ToString();
                //cutRectangle
                CutRectangleWithCorner.putRectangle();
            }

            
        }
        private void brightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            settingBrightnessTextBox.Text = Math.Round(brightnessSlider.Value, 1).ToString();
        }

        private void contrastSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            settingConstrastTextBox.Text = Math.Round(contrastSlider.Value, 1).ToString();
        }

        void setSliderWithTextBox(float min, float max, TextBox textBox, Slider slider)
        {
            float value;
            if (float.TryParse(textBox.Text, out value))
                if (value >= min && value <= max)
                    slider.Value = value;
                else
                    MessageBox.Show("Out of the range");
            else
                MessageBox.Show("Wrong format");
        }

        private Bitmap setContrast(float cont, Bitmap bitmap)
        {
            if (cont == 0)
            {
                cont = 1;
                return bitmap;
            }
            else
            {
                if (cont < 0)
                {
                    cont = 1.0f + cont * 0.1f;
                }
                else
                {
                    cont = cont + 1;
                }
            }
            if (tempOperationsList.Count >= quantityOfOperationOnImage - 1)
            {
                tempOperationsList.Clear();
            }
            tempOperationsList.Add(
                new tools.OperationOnImage()
                {
                    OperationType = tools.OperationOnImage.Contrast,
                    Value = cont
                });
            return tools.Contrast.setContrast(bitmap, cont);

        }
        private void settingConstrastTextBox_ClickEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                setSliderWithTextBox(-10, 10, settingConstrastTextBox, contrastSlider);
                if(openedImageObject != null)
                {
                    Bitmap bitmap = setBrightness((float)brightnessSlider.Value, openedImageObject.OpenedBitmap);
                    bitmap = setContrast((float)contrastSlider.Value, bitmap);
                    tempBitmap = bitmap;
                    photoCanvas.Background = OpenedImageObject.convertToBrush(OpenedImageObject.convertToBitmapImage(bitmap));
                }
            }

        }

       private Bitmap setBrightness(float bright, Bitmap bitmap)
        {
            Bitmap image;
            if (bright != 0)
            {
                image = tools.Brightness.SetBrightness(bitmap, bright, 10);
            }
            else
            {
                image = bitmap;
            }
            if (tempOperationsList.Count >= quantityOfOperationOnImage - 1)
            {
                tempOperationsList.Clear();
            }
            tempOperationsList.Add(
                new tools.OperationOnImage()
                {
                    OperationType = tools.OperationOnImage.Brightness,
                    Value = bright
                });
            return image;
        }
        private void settinBrightnessTextBox_ClickEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                setSliderWithTextBox(-10, 10, settingBrightnessTextBox, brightnessSlider);
                if (openedImageObject != null)
                {
                    Bitmap bitmap = setContrast((float)contrastSlider.Value, openedImageObject.OpenedBitmap);
                    bitmap = setBrightness((float)brightnessSlider.Value, bitmap);
                    tempBitmap = bitmap;
                    photoCanvas.Background = OpenedImageObject.convertToBrush(OpenedImageObject.convertToBitmapImage(bitmap));
                }
                
            }
                
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialogForm = new Microsoft.Win32.OpenFileDialog();
            dialogForm.Filter = "Image files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg; *.png|All files (*.*)|*.*";
            dialogForm.Title = "Choose photos";
            dialogForm.DefaultExt = ".png";
            dialogForm.Multiselect = true;
            bool? result = dialogForm.ShowDialog();
            if(result == true)
            {
                files = dialogForm.FileNames;
                operationsOnImages = new OperationsOnImageList[files.Length];
                for(int i = 0; i< files.Length; i++)
                {
                    operationsOnImages[i] = new OperationsOnImageList();
                }
                openedImageObject = new OpenedImageObject();
                ImageBrush iB = openedImageObject.putImage(new BitmapImage(new Uri(files[0])), photoCanvas.ActualWidth, photoCanvas.ActualHeight);
                photoCanvas.Background = iB;
                CutRectangleWithCorner = new CutRectangle(openedImageObject, photoCanvas, cutRectangle, cutRectangleThumbLeft, cutRectangleThumbRight);
                CutRectangleWithCorner.putRectangle();
                tempOperationsList = new OperationsOnImageList(3);
                tempBitmap = openedImageObject.OpenedBitmap;

            }
        }

        private void photoCanvas_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(openedImageObject != null)
            {

                CutRectangleWithCorner.updateRectangle();
            }
        }
        private void cutRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void cutRectangleThumbLeft_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            CutRectangleWithCorner.SetLeftUpCorner(e.HorizontalChange, e.VerticalChange);
        }

        private void cutRectangleThumbLeft_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            cutRectangleThumbLeft.Background = Brushes.Black;
        }

        private void cutRectangleThumbLeft_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            cutRectangleThumbLeft.Background = Brushes.Gray;
        }

        private void cutRectangleThumbRight_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            CutRectangleWithCorner.SetRightDownCorner(e.HorizontalChange, e.VerticalChange);
        }

        private void cutRectangleThumbRight_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            cutRectangleThumbRight.Background = Brushes.Black;
        }

        private void cutRectangleThumbRight_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            cutRectangleThumbRight.Background = Brushes.Gray;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            openedImageObject.updateImageSize(photoCanvas.ActualWidth, photoCanvas.ActualHeight);
            Bitmap bitmap = tempBitmap;

            if ((bool)cutPhotCheckBox.IsChecked)
            {
                tools.OperationOnImage cutImage = new tools.OperationOnImage();
                cutImage.OperationType = tools.OperationOnImage.CutImage;
                cutImage.X = CutRectangleWithCorner.XWithoutScale;
                cutImage.Y = CutRectangleWithCorner.YWithoutScale;
                cutImage.Width = CutRectangleWithCorner.WidthWithoutScale;
                cutImage.Height = CutRectangleWithCorner.HeightWithoutScale;
                tempOperationsList.Add(cutImage);
                openedImageObject.updateImageSource(bitmap);
                bitmap = CutRectangle.CutImage(bitmap, cutImage.X, cutImage.Y, cutImage.Width, cutImage.Height);
                photoCanvas.Background = openedImageObject.putImage(OpenedImageObject.convertToBitmapImage(bitmap), photoCanvas.ActualWidth, photoCanvas.ActualHeight);
            }
            for (int i = 0; i < operationsOnImages.Length; i++)
            {
                for (int j = 0; j < tempOperationsList.Count; j++)
                {
                    operationsOnImages[i].Add(tempOperationsList[j]);
                }
            }
            tempOperationsList.Clear();
            restartParameters();
        }

        void saveFiles(String dir)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (files != null)
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = "Folder"; // Default file name

                // Show save file dialog box
                bool? result = dialog.ShowDialog();
                Directory.CreateDirectory(dialog.FileName);
                
                for(int i = 0; i< files.Length; i++)
                {
                    Bitmap b = new Bitmap(files[i]);
                    String[] fileSplit = files[i].Split('\\');
                    String fileName = fileSplit[fileSplit.Length - 1];
                    String path = dialog.FileName + "\\" + fileName;
                    MessageBox.Show(path);

                    foreach (var o in operationsOnImages[i])
                    {
                        b = OperationOnImage.MakeOperation(b, o);
                    }
                    b.Save(path, ImageFormat.Png);
                }
            }
            
        }

        private void brightnessSlider_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
        }
    }
}
