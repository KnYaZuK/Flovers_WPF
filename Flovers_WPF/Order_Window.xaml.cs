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

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for Order_Window.xaml
    /// </summary>
    public partial class Order_Window : MetroWindow
    {
        public Order_Window()
        {
            InitializeComponent();
        }

        private void bt_new_Click(object sender, RoutedEventArgs e)
        {
            New_Order_Window NOW = new New_Order_Window();
            NOW.Show();
        }
    }
}
