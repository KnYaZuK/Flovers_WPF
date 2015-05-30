#region библиотеки
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
using iTextSharp.text;
using iTextSharp;
using iTextSharp.text.pdf;

using Flovers_WPF.DataModel;
using Flovers_WPF.DataAccess;
using Flovers_WPF.Repository;
using System.Data;
using System.Windows.Forms;
#endregion

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for Routes_Window.xaml
    /// </summary>
    /// 
    public partial class Routes_Window : MetroWindow
    {
        OrdersRepository oOrdersRepository;
        ConstantsRepository oConstantsRepository;
        public List<Orders> LOrders;
        SaveFileDialog sf;
        public string Start_address;
        private List<string> needded_addresses = new List<string>();
        public List<string> needded_adr_copy = new List<string>();
        public List<string> sorted_addresses = new List<string>();
        public List<GMap.NET.PointLatLng> sorted_points = new List<GMap.NET.PointLatLng>();

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

        #region Работа с картами 
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
        /// <summary>
        /// Метод сортировки адресов по мере удаленности и построения маршрута
        /// </summary>
        /// <returns></returns>
        private async Task Sort_Addresses()
        {
            List<Constants> start_list = await oConstantsRepository.Select_Constants_Async("select value from Constants where name=\'Address\'");
            Start_address = start_list.First().value.ToString();

            needded_addresses.Add(Start_address);
            needded_adr_copy.Add(Start_address);
            sorted_addresses.Add(Start_address);

            List<GMapRoute> routes_list = new List<GMapRoute>();
            LOrders = await oOrdersRepository.Select_All_Orders_Async();

            #region выборка всех нужных адресов из БД

            foreach (var c in LOrders)
            {
                if (c.status == "Готовится к отправке")
                {
                    needded_addresses.Add(c.address.ToString());
                    needded_adr_copy.Add(c.address.ToString());
                }
            }

            #endregion

            string Close_position = needded_addresses[0].ToString();

            #region Сортировка адресов по мере удаленности от адреса компании

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

            #endregion

            GMap.NET.RoutingProvider rp = gmap_routes.MapProvider as GMap.NET.RoutingProvider;
            if (rp == null)
            {
                rp = GMapProviders.OpenStreetMap;
            }
            GMap.NET.MapRoute route;

            #region построение маршрута по отсортированным адресам

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
                    routes_list.Add(mRoute);
                }
            }

            foreach(var l in routes_list)
            {
                gmap_routes.Markers.Add(l);
                l.RegenerateShape(gmap_routes);
            }

            #endregion
        }
        #endregion

        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oOrdersRepository = new OrdersRepository(oDBConnection);
            oConstantsRepository = new ConstantsRepository(oDBConnection);
        }

        private async void Route_Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();

            await Sort_Addresses();
        }

        #region Работа с PDF

        public void Create_PDF_File(string path)
        {
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            BaseFont base_font = BaseFont.CreateFont("arialn.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Phrase head = new Phrase("Маршрут", new iTextSharp.text.Font(base_font, 18, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Red)));
            iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph(head);
            header.Alignment = Element.ALIGN_CENTER;
            doc.Add(header);

            for (int i = 0; i < sorted_addresses.Count; i++)
            {
                string row = i.ToString() + ")" + sorted_addresses[i].ToString();
                iTextSharp.text.Phrase text = new Phrase(row, new iTextSharp.text.Font(base_font, 14, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black)));
                iTextSharp.text.Paragraph main_text = new iTextSharp.text.Paragraph(text);
                doc.Add(main_text);
            }

            doc.Close();
        }

        private void button_loadPDF_Click(object sender, RoutedEventArgs e)
        {
            sf = new SaveFileDialog();
            sf.FileName = "Route_List";
            sf.DefaultExt = ".pdf";
            sf.Filter = "(.pdf) | *.pdf";
            if(sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Create_PDF_File(sf.FileName);
                System.Windows.MessageBox.Show("Файл успешно создан");
            }
        }

        #endregion
    }
}
