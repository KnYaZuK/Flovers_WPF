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
using System.Windows.Forms;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for Main_menu.xaml
    /// </summary>
    public partial class Main_menu : MetroWindow
    {
        public Main_menu()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        public string DB_Path;
        
        private void bt_settings_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DB_Path = sfd.FileName;
            }
        }

        private void bt_Orders_Click(object sender, RoutedEventArgs e)
        {
            Order_Window ord_win = new Order_Window();
            ord_win.Show();
        }
        
    }
}
