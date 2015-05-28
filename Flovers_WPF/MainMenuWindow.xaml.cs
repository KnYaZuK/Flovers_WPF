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

using Flovers_WPF.DataAccess;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : MetroWindow
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection conn = new DBConnection();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void bt_settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();

            if ( settings.Set_DB_Path() )
            {
                DBConnection.Save_Connection(settings);
                System.Windows.Forms.MessageBox.Show("Путь успешно сохранён.");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Путь не был задан.");
            }
        }

        private void bt_Orders_Click(object sender, RoutedEventArgs e)
        {
            Order_Window ord_win = new Order_Window();
            ord_win.ShowDialog();
        }

        private void bt_records_Click(object sender, RoutedEventArgs e)
        {
            Components_Window rec_good = new Components_Window();
            rec_good.ShowDialog();
        }

        private void bt_flowers_Click(object sender, RoutedEventArgs e)
        {
            CreateFlowerWindow new_flav = new CreateFlowerWindow();
            new_flav.ShowDialog();
        }

        private void bt_deals_Click(object sender, RoutedEventArgs e)
        {
            //Special_Deal_Window spec_deal = new Special_Deal_Window();
            //spec_deal.ShowDialog();

            SpecialDealDiscountWindow oSpecialDealDiscountWindow = new SpecialDealDiscountWindow();
            oSpecialDealDiscountWindow.ShowDialog();
        }

        private void bt_route_Click(object sender, RoutedEventArgs e)
        {
            Routes_Window route = new Routes_Window();
            route.ShowDialog();
        }

        private void button_const_Click(object sender, RoutedEventArgs e)
        {
            Constants_Window const_win = new Constants_Window();
            const_win.ShowDialog();
        }

        private void button_Discounts_Click(object sender, RoutedEventArgs e)
        {
            Discounts_Window oDiscounts_Window = new Discounts_Window();
            oDiscounts_Window.ShowDialog();
        }
        

    }
}
