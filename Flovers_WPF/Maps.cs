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
    class Maps
    {
        string DB_address = "";
        /// <summary>
        /// метод поиска адреса по текстовой строке
        /// </summary>
        /// <param name="gmap_adress">карта</param>
        /// <param name="win">окно с картой</param>
        /// <returns>возвращает отформатированный адрес</returns>
        public string Find_Address(GMapControl gmap_adress,New_Adress_Window win)
        {
            gmap_adress.Markers.Clear();
            string url = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=true_or_false&language=ru", Uri.EscapeDataString(win.textbox_adres.Text));
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
            Stream filestream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(filestream);
            string responsereader = sreader.ReadToEnd();
            response.Close();

            XmlDocument document = new XmlDocument();
            document.LoadXml(responsereader);
            if (document.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                XmlNodeList nodes = document.SelectNodes("//location");

                double latitude = 0.0;
                double longitude = 0.0;

                foreach (XmlNode node in nodes)
                {
                    latitude = XmlConvert.ToDouble(node.SelectSingleNode("lat").InnerText.ToString());
                    longitude = XmlConvert.ToDouble(node.SelectSingleNode("lng").InnerText.ToString());
                }

                string formatted_address = document.SelectNodes("//formatted_address").Item(0).InnerText.ToString();
                string[] words = formatted_address.Split(',');
                string dataMarker = string.Empty;
                foreach (string word in words)
                {
                    dataMarker += word + ";" + Environment.NewLine;
                }

                var address = new GMapMarker(new GMap.NET.PointLatLng(latitude, longitude));
                {
                    address.Shape = new Custom_Markers.Red_marker(win, address, dataMarker);
                    address.Offset = new Point(-15, -15);
                }
                gmap_adress.Markers.Add(address);
                gmap_adress.Position = new GMap.NET.PointLatLng(latitude, longitude);
                gmap_adress.Zoom = 17;
                DB_address = win.textbox_adres.Text;
            }
            return DB_address;
        }


        /// <summary>
        /// Метод обратного геокодирования
        /// </summary>
        /// <param name="lat">Широта</param>
        /// <param name="lng">Долгота</param>
        /// <param name="gmap_adress">Карта</param>
        /// <param name="win">окно с картой</param>
        public void Reverse_Geocoding(double lat, double lng,GMapControl gmap_adress,New_Adress_Window win)
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

                win.textbox_adres.Text = City + " " + Street + " " + house;

                foreach (string word in components)
                {
                    MarkerData += word + ";" + Environment.NewLine;
                }

                var address = new GMapMarker(new GMap.NET.PointLatLng(lat, lng));
                {
                    address.Shape = new Custom_Markers.Red_marker(win, address, MarkerData);
                    address.Offset = new Point(-15, -15);
                }
                gmap_adress.Markers.Add(address);
                gmap_adress.Position = new GMap.NET.PointLatLng(lat, lng);
                gmap_adress.Zoom = 17;
            }
        }

        /// <summary>
        /// метод геокодирования
        /// </summary>
        /// <param name="address">адрес в формате строки</param>
        /// <returns>Возвращает широту и долготу указанного адреса</returns>
        public List<double> Geocoding(string address)
        {
            string url = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=true_or_false&language=ru", Uri.EscapeDataString(address));
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
            Stream filestream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(filestream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            double latitude = 0.0;
            double longitude = 0.0;
            XmlDocument document = new XmlDocument();
            document.LoadXml(responsereader);
            if (document.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                XmlNodeList nodes = document.SelectNodes("//location");

                foreach (XmlNode node in nodes)
                {
                    latitude = XmlConvert.ToDouble(node.SelectSingleNode("lat").InnerText.ToString());
                    longitude = XmlConvert.ToDouble(node.SelectSingleNode("lng").InnerText.ToString());
                }
            }
            List<double> latlng = new List<double>();
            latlng.Add(latitude);
            latlng.Add(longitude);
            return latlng;
        }
    }
}
