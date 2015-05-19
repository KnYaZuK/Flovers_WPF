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
using GMap;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using System.IO;
using System.Xml;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for New_Adress_Window.xaml
    /// </summary>
    public partial class New_Adress_Window : MetroWindow
    {
        public New_Adress_Window()
        {
            InitializeComponent();
            gmap_adress.Bearing = 0;
            gmap_adress.CanDragMap = true;
            gmap_adress.DragButton = MouseButton.Right;
            gmap_adress.MaxZoom = 18;
            gmap_adress.MinZoom = 2;
            gmap_adress.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            gmap_adress.ShowTileGridLines = false;
            gmap_adress.Zoom = 5;
            gmap_adress.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            button_find.IsEnabled = false;
        }

        private void button_find_Click(object sender, RoutedEventArgs e)
        {
            string url = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=true_or_false&language=ru",Uri.EscapeDataString(textbox_adres.Text));
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
            Stream filestream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(filestream);
            string responsereader = sreader.ReadToEnd();
            response.Close();

            XmlDocument document = new XmlDocument();
            document.LoadXml(responsereader);
            if(document.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                XmlNodeList nodes = document.SelectNodes("//location");

                double latitude = 0.0;
                double longitude = 0.0;

                foreach(XmlNode node in nodes)
                {
                    latitude = XmlConvert.ToDouble(node.SelectSingleNode("lat").InnerText.ToString());
                    longitude = XmlConvert.ToDouble(node.SelectSingleNode("lng").InnerText.ToString());
                }

                string formatted_address = document.SelectNodes("//formatted_address").Item(0).InnerText.ToString();
                string[] words = formatted_address.Split(',');
                string dataMarker = string.Empty;
                foreach (string word in words)
                {
                    dataMarker += word +";"+ Environment.NewLine;
                }
                GMap.NET.WindowsPresentation.GMapMarker address = new GMapMarker(new GMap.NET.PointLatLng(latitude, longitude));
                
            }
        }

        
    }
}
