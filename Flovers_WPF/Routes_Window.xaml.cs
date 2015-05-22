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
    /// Interaction logic for Routes_Window.xaml
    /// </summary>
    public partial class Routes_Window : MetroWindow
    {
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
    }
}
