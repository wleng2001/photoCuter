﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
        string openedFile;
        OpenedImageObject openedImageObject;
        byte quantityOfOperationOnImage = 3;
        CutRectangle CutRectangleWithCorner;
        MadeOperationsList madeOperationsList;

        Stopwatch sw1 = new Stopwatch();

        double stopWatchTimeInSec(Stopwatch sw)
        {
            return Math.Round((double)sw1.ElapsedTicks / (double)Stopwatch.Frequency, 3);
        }

        Bitmap tempBitmap;

        bool timeOfOperationsDebug = false;

        public byte thread = 4;
        byte previewResolution = 4;

        MenuItem[] threadingOptions;

        MenuItem[] previewResolutionOptions;

        public MainWindow()
        {
            InitializeComponent();
            threadingOptions = new MenuItem[]{
                MT1,MT2,MT4,MT6,MT8, MT16
            };

            previewResolutionOptions = new MenuItem[]
            {
                PS1,PS2,PS4
            };
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
            if (madeOperationsList.TempListLength >= quantityOfOperationOnImage - 1)
            {
                madeOperationsList.ClearTempOperations();
            }
            madeOperationsList.AddTempOpearation(new OperationOnImage
            {
                OperationType = OperationOnImage.Contrast,
                Value = cont,
            });

            return tools.Contrast.setContrast(bitmap, cont, thread);

        }

        void confirmSetContrast()
        {
            if (openedImageObject != null)
            {
                sw1.Restart();
                sw1.Start();
                Bitmap bitmap = setBrightness((float)brightnessSlider.Value, openedImageObject.OpenedBitmap);
                sw1.Stop();
                double time = stopWatchTimeInSec(sw1);
                sw1.Restart();
                sw1.Start();
                bitmap = setContrast((float)contrastSlider.Value, bitmap);
                sw1.Stop();
                if (timeOfOperationsDebug)
                    MessageBox.Show(stopWatchTimeInSec(sw1).ToString(), "Contrast");
                statusBarLabel.Content = "Contrast: " + stopWatchTimeInSec(sw1)+"s";
                sw1.Start();
                tempBitmap = bitmap;
                photoCanvas.Background = OpenedImageObject.convertToBrush(OpenedImageObject.convertToBitmapImage(bitmap));
                sw1.Stop();
                statusBarLabel.Content += "\t total: "+(stopWatchTimeInSec(sw1)+time).ToString()+"s";
            }
        }
        private void settingConstrastTextBox_ClickEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                setSliderWithTextBox(-10, 10, settingConstrastTextBox, contrastSlider);
                confirmSetContrast();
            }

        }

       private Bitmap setBrightness(float bright, Bitmap bitmap)
        {
            Bitmap image;
            if (bright != 0)
            {
                image = tools.Brightness.SetBrightness(bitmap, bright, 10, thread);
            }
            else
            {
                image = bitmap;
            }
            if (madeOperationsList.TempListLength >= quantityOfOperationOnImage - 1)
            {
                madeOperationsList.ClearTempOperations();
            }
            madeOperationsList.AddTempOpearation(new OperationOnImage
            {
                OperationType = OperationOnImage.Brightness,
                Value = bright,
            });

            return image;
        }

        void confirmSetBrightness()
        {
            if (openedImageObject != null)
            {
                sw1.Restart();
                sw1.Start();
                Bitmap bitmap = setContrast((float)contrastSlider.Value, openedImageObject.OpenedBitmap);
                sw1.Stop();
                double time = sw1.ElapsedMilliseconds / 1000;
                sw1.Restart();
                sw1.Start();
                bitmap = setBrightness((float)brightnessSlider.Value, bitmap);
                sw1.Stop();
                if (timeOfOperationsDebug)
                    MessageBox.Show(stopWatchTimeInSec(sw1).ToString() + "s", "Brightness");
                statusBarLabel.Content = "Brightness: " + stopWatchTimeInSec(sw1) + "s";
                tempBitmap = bitmap;
                photoCanvas.Background = OpenedImageObject.convertToBrush(OpenedImageObject.convertToBitmapImage(bitmap));
                statusBarLabel.Content += "\ttotal: " + (stopWatchTimeInSec(sw1)+time).ToString() + "s";
            }
        }
        private void settinBrightnessTextBox_ClickEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                setSliderWithTextBox(-10, 10, settingBrightnessTextBox, brightnessSlider);
                confirmSetBrightness();
                
            }
                
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        //Open click

        void loadPhoto(Bitmap bitmap)
        {
            openedImageObject = new OpenedImageObject();
            BitmapImage bI = OpenedImageObject.convertToBitmapImage(OpenedImageObject.changeResolution(bitmap, previewResolution));
            ImageBrush iB = openedImageObject.putImage(bI, photoCanvas.ActualWidth, photoCanvas.ActualHeight);
            photoCanvas.Background = iB;
            CutRectangleWithCorner = new CutRectangle(openedImageObject, photoCanvas, cutRectangle, cutRectangleThumbLeft, cutRectangleThumbRight);
            CutRectangleWithCorner.putRectangle();
            tempBitmap = openedImageObject.OpenedBitmap;
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
                sw1.Restart();
                sw1.Start();
                files = dialogForm.FileNames;
                openedFile = files[0];
                madeOperationsList = new MadeOperationsList(quantityOfOperationOnImage, operationsList, tempOpearationsList, files.Length);

                loadPhoto(new Bitmap(openedFile));

                madeOperationsList.ClearOperations();
                statusBarLabel.Content = "Opened: " + files[0];
                restartParameters();
                sw1.Stop();
                statusBarLabel.Content += "\tOpening time: " + stopWatchTimeInSec(sw1) + "s";
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
            if (openedImageObject != null)
            {

                openedImageObject.updateImageSize(photoCanvas.ActualWidth, photoCanvas.ActualHeight);
                Bitmap bitmap = tempBitmap;

                if ((bool)cutPhotCheckBox.IsChecked)
                {
                    tools.OperationOnImage cutImage = new tools.OperationOnImage();
                    cutImage.OperationType = tools.OperationOnImage.CutImage;
                    cutImage.X = CutRectangleWithCorner.XWithoutScale*previewResolution;
                    cutImage.Y = CutRectangleWithCorner.YWithoutScale*previewResolution;
                    cutImage.Width = CutRectangleWithCorner.WidthWithoutScale * previewResolution;
                    cutImage.Height = CutRectangleWithCorner.HeightWithoutScale * previewResolution;
                    madeOperationsList.AddTempOpearation(cutImage);
                    openedImageObject.updateImageSource(bitmap);

                    sw1.Restart();
                    sw1.Start();
                    bitmap = CutRectangle.CutImage(bitmap, cutImage.X/previewResolution, cutImage.Y / previewResolution, cutImage.Width / previewResolution, cutImage.Height / previewResolution);
                    sw1.Stop();
                    if (timeOfOperationsDebug)
                        MessageBox.Show(((double)sw1.ElapsedTicks / (double)Stopwatch.Frequency).ToString() + "s", "Cut");
                    statusBarLabel.Content = "Cut Photo: " + stopWatchTimeInSec(sw1) + "s";

                    tempBitmap = bitmap;

                    photoCanvas.Background = openedImageObject.putImage(OpenedImageObject.convertToBitmapImage(bitmap), photoCanvas.ActualWidth, photoCanvas.ActualHeight);

                }
                madeOperationsList.MigrateToMainImageOperation();
                madeOperationsList.RemoveTempOperations();
                restartParameters();
            }
        }

        void saveFiles(String dir)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs en)
        {
            void updateStatusBar(string text)
            {
                statusBarLabel.Content = text;
                statusBarProgressBar.Value++;
            }

            void saveFiles(Microsoft.Win32.SaveFileDialog dialog)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    Bitmap b = new Bitmap(files[i]);
                    String[] fileSplit = files[i].Split('\\');
                    String fileName = fileSplit[fileSplit.Length - 1];
                    String path = dialog.FileName + "\\" + fileName;
                    
                    Application.Current.Dispatcher.Invoke( () => updateStatusBar("Saved: " + path));

                    foreach (var o in madeOperationsList[i])
                    {
                        b = OperationOnImage.MakeOperation(b, o, thread);
                    }
                    b.Save(path, ImageFormat.Png);
                }
            }
            if (files != null)
            {
                sw1.Restart();
                sw1.Start();
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = "Folder"; // Default file name

                // Show save file dialog box
                bool? result = dialog.ShowDialog();
                Directory.CreateDirectory(dialog.FileName);
                
                statusBarProgressBar.Visibility = Visibility.Visible;
                statusBarProgressBar.Minimum = 0;
                statusBarProgressBar.Maximum = files.Length-1;
                                              
                saveFiles(dialog);
                               
                statusBarProgressBar.Visibility = Visibility.Hidden;
                sw1.Stop();
                statusBarLabel.Content += "\tSaving time: " + stopWatchTimeInSec(sw1) + "s";
            }
            
        }

        private void brightnessSlider_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
        }

        private void brightnessSlider_DragOver(object sender, DragEventArgs e)
        {
            MessageBox.Show("napis");
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            timeOfOperationsMenuItem.IsChecked = !timeOfOperationsMenuItem.IsChecked;
            timeOfOperationsDebug = !timeOfOperationsDebug;
        }

        private void brightnessSlider_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                confirmSetBrightness();
            }
        }

        private void contrastSlider_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                confirmSetContrast();
            }
        }

        void uncheckMultiThreading()
        {
            foreach(MenuItem m in threadingOptions)
            {
                m.IsChecked = false;
            }
        }

        void checkMultiThreadOption(byte number)
        {
            uncheckMultiThreading();
            threadingOptions[number].IsChecked = !threadingOptions[number].IsChecked;
        }
        private void MenuItem_Click_MT4(object sender, RoutedEventArgs e)
        {
            checkMultiThreadOption(2);
            thread = 4;
        }

        private void MT8_Click(object sender, RoutedEventArgs e)
        {
            checkMultiThreadOption(4);
            thread = 8;
        }

        private void MenuItem_Click_MT1(object sender, RoutedEventArgs e)
        {
            checkMultiThreadOption(0);
            thread = 1;
        }

        private void MenuItem_Click_MT2(object sender, RoutedEventArgs e)
        {
            checkMultiThreadOption(1);
            thread = 2;
        }

        private void MenuItem_Click_MT6(object sender, RoutedEventArgs e)
        {
            checkMultiThreadOption(3);
            thread = 6;
        }

        private void MenuItem_Click_MT16(object sender, RoutedEventArgs e)
        {
            checkMultiThreadOption(5);
            thread = 16;
        }

        void uncheckPreviewResolutionOptions()
        {
            foreach(MenuItem m in previewResolutionOptions)
            {
                m.IsChecked = false;
            }
        }
        private void PS1_Click(object sender, RoutedEventArgs e)
        {
            uncheckPreviewResolutionOptions();
            PS1.IsChecked = true;
            previewResolution = 1;
            if (openedFile != null)
            {
                loadPhoto(OpenedImageObject.changeResolution(new Bitmap(openedFile), previewResolution));
                madeOperationsList.ClearOperations();
                restartParameters();
            }
        }

        private void PS2_Click(object sender, RoutedEventArgs e)
        {
            uncheckPreviewResolutionOptions();
            PS2.IsChecked = true;
            previewResolution = 2;
            if (openedFile != null)
            {
                loadPhoto(OpenedImageObject.changeResolution(new Bitmap(openedFile), previewResolution));
                madeOperationsList.ClearOperations();
                restartParameters();
            }
        }

        private void PS4_Click(object sender, RoutedEventArgs e)
        {
            uncheckPreviewResolutionOptions();
            PS4.IsChecked = true;
            previewResolution = 4;
            if (openedFile != null)
            {
                loadPhoto(OpenedImageObject.changeResolution(new Bitmap(openedFile), previewResolution));
                madeOperationsList.ClearOperations();
                restartParameters();
            }
        }
    }
}
