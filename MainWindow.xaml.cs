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

        private void settingConstrastTextBox_ClickEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                float value;
                if (float.TryParse(settingConstrastTextBox.Text, out value))
                    if (value >= -10 && value <= 10)
                        contrastSlider.Value = value;
                    else
                        MessageBox.Show("Out of the range");
                else
                    MessageBox.Show("Wrong format");
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialogForm = new Microsoft.Win32.OpenFileDialog();
            dialogForm.Filter = "Image files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg; *.png|All files (*.*)|*.*";
            dialogForm.Title = "Choose photos";
            dialogForm.DefaultExt = ".png";
            dialogForm.Multiselect = true;
            dialogForm.ShowDialog();
        }
    }
}
