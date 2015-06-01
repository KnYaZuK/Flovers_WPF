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
using iTextSharp.text;
using iTextSharp;
using iTextSharp.text.pdf;
using Flovers_WPF.Repository;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;
using System.Net.Mail;
using System.Net;

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
            button_stopVideo.IsEnabled = false;
            button_SaveImage.IsEnabled = false;
            button_CaptureImage.IsEnabled = false;
        }

        Camera_Control webcam;
        public List<object> added_images = new List<object>();
        SaveFileDialog save_pdf;
        ClientsRepository oClients;
        ConstantsRepository oConstants;
        public List<Constants> mail_addresses;
        public List<Constants> mail_passwords;
        public string PDF_path;
        OpenFileDialog open_pdf;
        PDF_creater pdf = new PDF_creater();
        MailClass mail = new MailClass();

        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oClients = new ClientsRepository(oDBConnection);
            oConstants = new ConstantsRepository(oDBConnection);
        }
        /// <summary>
        /// инициализация камеры и БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void spec_deal_Loaded(object sender, RoutedEventArgs e)
        {
            webcam = new Camera_Control();
            webcam.Ini_Web_Camera(ref imgVideo);
            await Initialize_Database();
        }
        /// <summary>
        /// включение камеры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_startVideo_Click(object sender, RoutedEventArgs e)
        {
            webcam.Start();
            button_stopVideo.IsEnabled = true;
            button_startVideo.IsEnabled = false;
            button_CaptureImage.IsEnabled = true;
        }
        /// <summary>
        /// остановка камеры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stopVideo_Click(object sender, RoutedEventArgs e)
        {
            webcam.Stop();
            button_startVideo.IsEnabled = true;
            button_stopVideo.IsEnabled = false;
            button_CaptureImage.IsEnabled = false;
        }
        /// <summary>
        /// захват изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CaptureImage_Click(object sender, RoutedEventArgs e)
        {
            imgCaptured.Source = imgVideo.Source;
            button_SaveImage.IsEnabled = true;
        }
        /// <summary>
        /// сохранение изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            save_pdf = new SaveFileDialog();
            if(save_pdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pdf.Create_PDF_File(save_pdf.FileName,this);
                PDF_path = save_pdf.FileName;
            }
        }
        /// <summary>
        /// рассылка почты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SendMail_Click(object sender, RoutedEventArgs e)
        {
            mail.Send_Messages(this, textbox_theme.Text, textbox_text, PDF_path, added_images,oClients,oConstants);
        }

        private void button_add_pdf_Click(object sender, RoutedEventArgs e)
        {
            open_pdf = new OpenFileDialog();
            if(open_pdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PDF_path = open_pdf.FileName;
            }
        }
    }
}
