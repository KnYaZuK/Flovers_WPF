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
using System.Net;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for New_Adress_Window.xaml
    /// </summary>
    public partial class New_Adress_Window : MetroWindow
    {
        public string DB_address = "";
        public string NewAddress = "";
        /// <summary>
        /// конструктор(включает в себя настройку карты)
        /// </summary>
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
            gmap_adress.MouseDoubleClick += new MouseButtonEventHandler(map_MouseClick);
        }
        /// <summary>
        /// Метод геокодирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_find_Click(object sender, RoutedEventArgs e)
        {
            gmap_adress.Markers.Clear();
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
       
                var address = new GMapMarker(new GMap.NET.PointLatLng(latitude, longitude));
                {
                    address.Shape = new Custom_Markers.Red_marker(this, address, dataMarker);
                    address.Offset = new Point(-15, -15);
                }
                gmap_adress.Markers.Add(address);
                gmap_adress.Position = new GMap.NET.PointLatLng(latitude, longitude);
                gmap_adress.Zoom = 17;
                DB_address = textbox_adres.Text;
            }
        }
        /// <summary>
        /// событие нажатия на карту (двойной клик)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void map_MouseClick(object sender,MouseEventArgs e)
        {
            gmap_adress.Markers.Clear();
            double lat = 0.0;
            double lng = 0.0;

            if(e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(gmap_adress);
                lat = gmap_adress.FromLocalToLatLng((int)pos.X, (int)pos.Y).Lat;
                lng = gmap_adress.FromLocalToLatLng((int)pos.X, (int)pos.Y).Lng;

                Reverse_Geocoding(lat, lng);
                DB_address = textbox_adres.Text;
            }
        }
        /// <summary>
        /// метод обратного геокодирования
        /// </summary>
        /// <param name="lat">Широта</param>
        /// <param name="lng">Долгота</param>
        public void Reverse_Geocoding(double lat, double lng)
        {
            string url = string.Format("http://maps.google.com/maps/api/geocode/xml?latlng={0},{1}&sensor=true_or_false&language=ru", lat.ToString().Replace(",", "."), lng.ToString().Replace(",", "."));
            HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            Stream data_stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(data_stream);
            string response_reader = reader.ReadToEnd();
            response.Close();

            XmlDocument document = new XmlDocument();
            document.LoadXml(response_reader);

            if (document.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                string formatted_address = document.SelectNodes("//formatted_address").Item(0).InnerText.ToString();
                string[] components = formatted_address.Split(',');
                string MarkerData = "";

                string City = components[2].Trim();
                string Street = components[0].Trim();
                string house = components[1].Trim();

                textbox_adres.Text = City + " " + Street + " " + house;

                foreach (string word in components)
                {
                    MarkerData += word + ";" + Environment.NewLine;
                }

                var address = new GMapMarker(new GMap.NET.PointLatLng(lat, lng));
                {
                    address.Shape = new Custom_Markers.Red_marker(this, address, MarkerData);
                    address.Offset = new Point(-15, -15);
                }
                gmap_adress.Markers.Add(address);
                gmap_adress.Position = new GMap.NET.PointLatLng(lat, lng);
                gmap_adress.Zoom = 17;
            }
        }
        /// <summary>
        /// Метод выбора окнчательного адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_choose_Click(object sender, RoutedEventArgs e)
        {
            NewAddress = DB_address;
            this.Close();
        }
    }
}
