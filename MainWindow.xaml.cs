using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

namespace photoCuter
{
    using Brushes = System.Windows.Media.Brushes;

    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 
    using OperationsOnImageList = List<OperationOnImage>;
    public partial class MainWindow : Window
    {
        string[] files;
        OpenedImageObject openedImageObject;
        OperationsOnImageList[] operationsOnImages;
        OperationsOnImageList tempOperationsList;
        byte quantityOfOperationOnImage = 3;
        CutRectangle CutRectangleWithCorner;
        Brightness brightness = new Brightness(10);

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
        private void settingConstrastTextBox_ClickEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                setSliderWithTextBox(-10, 10, settingConstrastTextBox, contrastSlider);
            }

        }

        private void settinBrightnessTextBox_ClickEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                setSliderWithTextBox(-10, 10, settingBrightnessTextBox, brightnessSlider);
                photoCanvas.Background = openedImageObject.putImage(OpenedImageObject.convertToBitmapImage(brightness.SetBrightness(OpenedImageObject.convertToBitmap(openedImageObject.OpenedImage), (float)brightnessSlider.Value)), photoCanvas.ActualWidth, photoCanvas.ActualHeight);
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
                openedImageObject = new OpenedImageObject();
                ImageBrush iB = openedImageObject.putImage(new BitmapImage(new Uri(files[0])), photoCanvas.ActualWidth, photoCanvas.ActualHeight);
                photoCanvas.Background = iB;
                CutRectangleWithCorner = new CutRectangle(openedImageObject, photoCanvas, cutRectangle, cutRectangleThumbLeft, cutRectangleThumbRight);
                CutRectangleWithCorner.putRectangle();
                tempOperationsList = new OperationsOnImageList(3);

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
            if ((bool)cutPhotCheckBox.IsChecked)
            {
                OperationOnImage cutImage = new OperationOnImage();
                cutImage.OperationType = OperationOnImage.CutImage;
                cutImage.X = CutRectangleWithCorner.XWithoutScale;
                cutImage.Y = CutRectangleWithCorner.YWithoutScale;
                cutImage.Width = CutRectangleWithCorner.WidthWithoutScale;
                cutImage.Height = CutRectangleWithCorner.HeightWithoutScale;
                tempOperationsList.Add(cutImage);
                CutRectangleWithCorner.CutImage();
            }
             restartParameters();
        }

        void saveFiles(String dir)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Folder"; // Default file name

            // Show save file dialog box
            bool? result = dialog.ShowDialog();
        }

        private void brightnessSlider_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            openedImageObject.putImage(OpenedImageObject.convertToBitmapImage(brightness.SetBrightness(OpenedImageObject.convertToBitmap(openedImageObject.OpenedImage), (float)brightnessSlider.Value)), photoCanvas.ActualWidth, photoCanvas.ActualHeight);
            MessageBox.Show(brightnessSlider.Value.ToString());
        }
    }
}
