using MahApps.Metro.Controls;
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
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for Special_Deal_Window.xaml
    /// </summary>
    public partial class Special_Deal_Window : MetroWindow
    {
        public Special_Deal_Window()
        {
            InitializeComponent();
        }

        Camera_Control webcam;
        List<object> added_images = new List<object>();

        private void spec_deal_Loaded(object sender, RoutedEventArgs e)
        {
            webcam = new Camera_Control();
            webcam.Ini_Web_Camera(ref imgVideo);
        }

        private void button_startVideo_Click(object sender, RoutedEventArgs e)
        {
            webcam.Start();
        }

        private void button_stopVideo_Click(object sender, RoutedEventArgs e)
        {
            webcam.Stop();
        }

        private void button_CaptureImage_Click(object sender, RoutedEventArgs e)
        {
            imgCaptured.Source = imgVideo.Source;
        }

        private void button_SaveImage_Click(object sender, RoutedEventArgs e)
        {
            Camera_HelperClass.SaveImageCapture((BitmapSource)imgCaptured.Source);
        }

        private void add_photo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog select_pic = new OpenFileDialog();
            if(select_pic.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                added_images.Add(select_pic.FileName);
                pictures.Items.Add(select_pic.FileName);
            }
        }

        private void delete_photo_Click(object sender, RoutedEventArgs e)
        {
            if (pictures.SelectedIndex != -1)
            {
                pictures.Items.RemoveAt(pictures.SelectedIndex);
                added_images.Remove(pictures.SelectedItem);
            }
            else
            {
                System.Windows.MessageBox.Show("выберите изображение из списка");
            }
        }

        private void button_SaveAsPDF_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
