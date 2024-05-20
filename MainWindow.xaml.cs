using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace photoCuter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] files;
        openedImageObject openedImageObject;

        public MainWindow()
        {
            InitializeComponent();
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
                setSliderWithTextBox(-10, 10, settingBrightnessTextBox, brightnessSlider);
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
                ImageBrush iB = openedImageObject.putImage(new BitmapImage(new Uri(files[0])), photoCanvas.ActualWidth, photoCanvas.ActualHeight);
                photoCanvas.Background = iB;
                putCutRectangle(photoCanvas.ActualWidth, photoCanvas.ActualHeight);

            }
        }

        void putCutRectangle(double windowWidth, double windowHeight)
        {

            
            cutRectangle.Visibility = Visibility.Hidden;
            openedImageObject.updateImageSize(windowWidth, windowHeight);
            cutRectangle.Width = openedImageObject.Width;
            cutRectangle.Height = openedImageObject.Height;
            Canvas.SetLeft(cutRectangle, openedImageObject.X);
            Canvas.SetTop(cutRectangle, openedImageObject.Y);
            cutRectangle.Visibility = Visibility.Visible;
        }

        private void photoCanvas_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(openedImageObject != null)
            {
                putCutRectangle(photoCanvas.ActualWidth, photoCanvas.ActualHeight);
            }
        }
        private void cutRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void cutRectangleThumbLeft_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

        }

        private void cutRectangleThumbLeft_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }

        private void cutRectangleThumbLeft_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

        }

        private void cutRectangleThumbRight_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

        }

        private void cutRectangleThumbRight_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }

        private void cutRectangleThumbRight_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

        }
    }
}
