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
            Maps m = new Maps();
            DB_address = m.Find_Address(gmap_adress, this);
        }
        /// <summary>
        /// событие нажатия на карту (двойной клик)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void map_MouseClick(object sender,MouseEventArgs e)
        {
            Maps m = new Maps();
            gmap_adress.Markers.Clear();
            double lat = 0.0;
            double lng = 0.0;

            if(e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(gmap_adress);
                lat = gmap_adress.FromLocalToLatLng((int)pos.X, (int)pos.Y).Lat;
                lng = gmap_adress.FromLocalToLatLng((int)pos.X, (int)pos.Y).Lng;

                m.Reverse_Geocoding(lat, lng,gmap_adress,this);
                DB_address = textbox_adres.Text;
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
