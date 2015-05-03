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
    /// Interaction logic for New_Order_Window.xaml
    /// </summary>
    public partial class New_Order_Window : MetroWindow
    {
        public New_Order_Window()
        {
            InitializeComponent();
        }

        private void bt_new_client_Click(object sender, RoutedEventArgs e)
        {
            NewClientWindow new_Client = new NewClientWindow();
            new_Client.Show();
        }
    }
}
