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
using System.Data;

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
            gmap_routes.MapProvider = GMap.NET.MapProviders.GMapProviders.OpenStreetMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gmap_routes.Markers.Clear();
        } 

        private void gmap_routes_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        /* Сделать выбор адреса из таблицы констант */
        public string Start_address = "Ульяновск Авиастроителей 7";
        private List<string> needded_addresses = new List<string>();
        public List<string> needded_adr_copy = new List<string>();
        public List<string> sorted_addresses = new List<string>();
        public List<GMap.NET.PointLatLng> sorted_points = new List<GMap.NET.PointLatLng>();

        /// <summary>
        /// Метод геокодирования
        /// </summary>
        /// <param name="address">Адрес</param>
        /// <returns>Широта,Долгота</returns>
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

        private async Task Sort_Addresses()
        {
            needded_addresses.Add(Start_address);
            needded_adr_copy.Add(Start_address);
            sorted_addresses.Add(Start_address);

            DataTable dtRouter = new DataTable();
            dtRouter.Columns.Add("Нач. точка (latitude)");
            dtRouter.Columns.Add("Нач. точка (longitude)");
            dtRouter.Columns.Add("Кон. точка (latitude)");
            dtRouter.Columns.Add("Кон. точка (longitude)");

            List<GMapRoute> routes_list = new List<GMapRoute>();
            LOrders = await oOrdersRepository.Select_All_Orders_Async();
            foreach (var c in LOrders)
            {
                if (c.status == "Готовится к отправке")
                {
                    needded_addresses.Add(c.address.ToString());
                    needded_adr_copy.Add(c.address.ToString());
                }
            }
            string Close_position = needded_addresses[0].ToString();

            for (int i = 0; i < needded_addresses.Count - 1; i++)
            {
                double min_Dist = 1000000.0;
                string Start_position = Close_position;
                int closest_id = 1;
                string Closest_Adr = "";
                List<double> Start_position_point = Geocoding(Start_position);
                sorted_points.Add(new GMap.NET.PointLatLng(Start_position_point[0], Start_position_point[1]));


                for (int j = closest_id; j < needded_adr_copy.Count; j++)
                {
                    if (needded_adr_copy[j] != Start_position)
                    {
                        double Distance = 0.0;
                        List<double> adr_latlng = Geocoding(needded_adr_copy[j].ToString());
                        Distance = gmap_routes.MapProvider.Projection.GetDistance(new GMap.NET.PointLatLng(Start_position_point[0], Start_position_point[1]), new GMap.NET.PointLatLng(adr_latlng[0], adr_latlng[1]));

                        if (Distance < min_Dist)
                        {
                            min_Dist = Distance;
                            Closest_Adr = needded_adr_copy[j].ToString();
                        }
                    }
                }
                sorted_addresses.Add(Closest_Adr.ToString());
                List<double> closest_address_points = Geocoding(Closest_Adr.ToString());
                sorted_points.Add(new GMap.NET.PointLatLng(closest_address_points[0], closest_address_points[1]));
                needded_adr_copy.Remove(Closest_Adr);
                Close_position = Closest_Adr;
            }

            GMap.NET.RoutingProvider rp = gmap_routes.MapProvider as GMap.NET.RoutingProvider;
            if (rp == null)
            {
                rp = GMapProviders.OpenStreetMap;
            }
            GMap.NET.MapRoute route;
            for (int i = 0; i < sorted_addresses.Count - 1; i++)
            {
                List<double> first = Geocoding(sorted_addresses[i]);
                List<double> last = Geocoding(sorted_addresses[i + 1]);
                route = rp.GetRoute(new GMap.NET.PointLatLng(first[0], first[1]), new GMap.NET.PointLatLng(last[0], last[1]), false, false, (int)gmap_routes.Zoom);
                if (route != null)
                {
                    GMapRoute mRoute = new GMapRoute(route.Points);
                    {
                        mRoute.ZIndex = -1;
                    }
                    //gmap_routes.Markers.Add(mRoute);
                    //mRoute.RegenerateShape(gmap_routes);
                    routes_list.Add(mRoute);
                }
            }

            foreach(var l in routes_list)
            {
                gmap_routes.Markers.Add(l);
                l.RegenerateShape(gmap_routes);
            }
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

            await Sort_Addresses();
        }
    }
}
