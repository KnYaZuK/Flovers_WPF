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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET.WindowsPresentation;
using System.Windows.Controls.Primitives;
using Flovers_WPF;

namespace Flovers_WPF.Custom_Markers
{
    /// <summary>
    /// Interaction logic for Red_marker.xaml
    /// </summary>
    public partial class Red_marker
    {
        Popup Popup;
        Label Label;
        GMapMarker Marker;
        New_Adress_Window win;
        Maps m = new Maps();
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="window">Родительское окно</param>
        /// <param name="marker">Маркер</param>
        /// <param name="title">Описание маркера</param>
        public Red_marker(New_Adress_Window window,GMapMarker marker, string title)
        {
            this.InitializeComponent();
            this.win = window;
            this.Marker = marker;
            Popup = new Popup();
            Label = new Label();
            
            this.Loaded += new RoutedEventHandler(Red_marker_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(Red_marker_SizeChanged);
            this.MouseEnter += new MouseEventHandler(Red_marker_MouseEnter);
            this.MouseLeave += new MouseEventHandler(Red_marker_MouseLeave);
            this.MouseMove += new MouseEventHandler(Red_marker_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Red_marker_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Red_marker_MouseLeftButtonDown);

            Popup.Placement = PlacementMode.Mouse;
            {
                Label.Background = Brushes.Blue;
                Label.Foreground = Brushes.White;
                Label.BorderBrush = Brushes.WhiteSmoke;
                Label.BorderThickness = new Thickness(2);
                Label.Padding = new Thickness(5);
                Label.FontSize = 22;
                Label.Content = title;
            }
            Popup.Child = Label;
        }

        void Red_marker_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void Red_marker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        void Red_marker_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 10000;
            Popup.IsOpen = true;
        }

        void Red_marker_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 10000;
            Popup.IsOpen = false;
        }

        void Red_marker_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
            {
                var pos = e.GetPosition(win.gmap_adress);
                Marker.Position = win.gmap_adress.FromLocalToLatLng((int)pos.X,(int)pos.Y);
                double lat = win.gmap_adress.FromLocalToLatLng((int)pos.X,(int)pos.Y).Lat;
                double lng = win.gmap_adress.FromLocalToLatLng((int)pos.X,(int)pos.Y).Lng;
                win.gmap_adress.Markers.Clear();
                m.Reverse_Geocoding(lat, lng, win.gmap_adress, win);
                win.DB_address = win.textbox_adres.Text;
            }
        }

        void Red_marker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
            }
        }

        void Red_marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                Mouse.Capture(this);
            }
        }
    }
}
