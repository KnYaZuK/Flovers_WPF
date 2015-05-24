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

using Flovers_WPF.DataModel;
using Flovers_WPF.DataAccess;
using Flovers_WPF.Repository;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for Routes_Window.xaml
    /// </summary>
    /// 
    public partial class Routes_Window : MetroWindow
    {
        OrdersRepository oOrdersRepository;
        public List<Orders> LOrders;
        public Routes_Window()
        {
            InitializeComponent();
            gmap_routes.Bearing = 0;
            gmap_routes.CanDragMap = true;
            gmap_routes.DragButton = MouseButton.Right;
            gmap_routes.MaxZoom = 18;
            gmap_routes.MinZoom = 2;
            gmap_routes.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            gmap_routes.ShowTileGridLines = false;
            gmap_routes.Zoom = 5;
            gmap_routes.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
        } 

        private void gmap_routes_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        //public double Dist = 0.0;
        //public void GetDistanse()
        //{
        //    Dist = gmap_routes.MapProvider.Projection.GetDistance(new GMap.NET.PointLatLng(54.385834, 48.585122), new GMap.NET.PointLatLng(54.369156, 48.587579));
        //    MessageBox.Show(Dist.ToString());
        //}
        public GMap.NET.PointLatLng start = new GMap.NET.PointLatLng(54.388271, 48.611187);
        private List<string> needded_addresses = new List<string>();
        public List<string> sorted_addresses = new List<string>();

        public List<double> Geocoding(string address)
        {
            string url = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=true_or_false&language=ru",Uri.EscapeDataString(address));
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

        private async Task getOrders()
        {
            double Distance = 0.0;
            double min_Dist_address = 10000000.0;
            LOrders = await oOrdersRepository.Select_All_Orders_Async();
            foreach (var c in LOrders)
            {
                if(c.status == "Оплачен, готовится к отправке")
                {
                    needded_addresses.Add(c.address.ToString());
                    //MessageBox.Show(c.address.ToString());
                }
            }

            foreach(var adr in needded_addresses)
            {
                List<double> latlng_address = Geocoding(adr);
                Distance = gmap_routes.MapProvider.Projection.GetDistance(start, new GMap.NET.PointLatLng(latlng_address[0], latlng_address[1]));
                if(Distance < min_Dist_address)
                {
                    min_Dist_address = Distance;
                }
            }

            MessageBox.Show(min_Dist_address.ToString());
        }
        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oOrdersRepository = new OrdersRepository(oDBConnection);
        }
        private async void Route_Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();

            await getOrders();
     
        }
    }
}
