using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flovers_WPF.Custom_Markers
{
    /// <summary>
    /// Interaction logic for Route_Orange_Marker.xaml
    /// </summary>
    public partial class Route_Orange_Marker
    {
        Popup Popup;
        Label Label;
        GMapMarker Marker;
        Routes_Window rt_win;
        public Route_Orange_Marker(Routes_Window window, GMapMarker marker, string title)
        {
            InitializeComponent();
            this.InitializeComponent();
            this.rt_win = window;
            this.Marker = marker;
            Popup = new Popup();
            Label = new Label();

            this.Loaded += new RoutedEventHandler(Route_Orange_Marker_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(Route_Orange_Marker_SizeChanged);
            this.MouseEnter += new MouseEventHandler(Route_Orange_Marker_MouseEnter);
            this.MouseLeave += new MouseEventHandler(Route_Orange_Marker_MouseLeave);

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

        void Route_Orange_Marker_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void Route_Orange_Marker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        void Route_Orange_Marker_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 10000;
            Popup.IsOpen = true;
        }

        void Route_Orange_Marker_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 10000;
            Popup.IsOpen = false;
        }
    }
}
