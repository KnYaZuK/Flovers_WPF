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
using Flovers_WPF.DataModel;
using Flovers_WPF.DataAccess;
using Flovers_WPF.Repository;
using System.Data;
using System.Windows.Forms;
using System.Windows.Controls.DataVisualization.Charting;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for Reply_Window.xaml
    /// </summary>
    public partial class Reply_Window : MetroWindow
    {
        OrdersRepository oOrdersRepository;
        CartsRepository oCartsRepository;
        BouquetsRepository oBouquetsRepository;
        List<KeyValuePair<string, int>> values;
        public struct Data
        {
            public string time { get; set; }
            public int count { get; set; }
            public int bouqet_id { get; set; }
        }
        
        SQLite.SQLiteAsyncConnection conn;
        public Reply_Window()
        {
            InitializeComponent();
            grid_day.Visibility = System.Windows.Visibility.Collapsed;
            grid_month.Visibility = System.Windows.Visibility.Collapsed;
            grid_week.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oOrdersRepository = new OrdersRepository(oDBConnection);
            oCartsRepository = new CartsRepository(oDBConnection);
            oBouquetsRepository = new BouquetsRepository(oDBConnection);

            conn = oDBConnection.GetAsyncConnection();
        }

        private void menu_lvl2_day_Click(object sender, RoutedEventArgs e)
        {
            grid_day.Visibility = System.Windows.Visibility.Visible;
            grid_month.Visibility = System.Windows.Visibility.Collapsed;
            grid_week.Visibility = System.Windows.Visibility.Collapsed;
        }
        /// <summary>
        /// построение графика - отчет за день
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_build_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < day_chart.Series.Count; i++)
            {
                day_chart.Series.Remove(day_chart.Series[i]);
            }
            values = new List<KeyValuePair<string, int>>();
            List<Orders> list_orders = await oOrdersRepository.Select_All_Orders_Async();
            List<Data> list_data = new List<Data>();
            for(int i = 0; i < list_orders.Count; i++)
            {
                Data d = new Data();
                if(list_orders[i].datetime.Date == datepicker.SelectedDate)
                {
                    List<Carts> list_carts = await oCartsRepository.Select_Carts_Async("select count,bouquets_id from Carts where orders_id=" + list_orders[i].orders_id);
                    d.time = list_orders[i].datetime.TimeOfDay.ToString();
                    foreach(var c in list_carts)
                    {
                        d.count = (int)c.count;
                        d.bouqet_id = c.bouquets_id;
                        list_data.Add(d);
                    }
                }
            }

            List<Bouquets> list_bouq = await oBouquetsRepository.Select_All_Bouquets_Async();
            for(int i = 0; i < list_bouq.Count; i++)
            {
                values = new List<KeyValuePair<string, int>>();
                for (int j = 0; j < list_data.Count; j++)
                {
                    if(list_data[j].bouqet_id == list_bouq[i].bouquets_id)
                    {
                        values.Add(new KeyValuePair<string, int>(list_data[j].time, list_data[j].count));
                    }
                }
                createLineSeries_Day(values, list_bouq[i].name);
            }
        }

        #region методы отрисовки данных графика
        public void createLineSeries_Day(List<KeyValuePair<string,int>> values,string title)
        {
            ColumnSeries ls = new ColumnSeries();
            ls.Title = title;
            ls.DependentValuePath = "Value";
            ls.IndependentValuePath = "Key";
            ls.IsSelectionEnabled = true;
            ls.ItemsSource = values;
            day_chart.Series.Add(ls);
        }

        public void createColSeries_Month(List<KeyValuePair<string,int>> values,string title)
        {
            ColumnSeries ls = new ColumnSeries();
            ls.Title = title;
            ls.DependentValuePath = "Value";
            ls.IndependentValuePath = "Key";
            ls.IsSelectionEnabled = true;
            ls.ItemsSource = values;
            month_chart.Series.Add(ls);
        }

        public void createColSeries_Week(List<KeyValuePair<string, int>> values, string title)
        {
            ColumnSeries ls = new ColumnSeries();
            ls.Title = title;
            ls.DependentValuePath = "Value";
            ls.IndependentValuePath = "Key";
            ls.IsSelectionEnabled = true;
            ls.ItemsSource = values;
            week_chart.Series.Add(ls);
        }
        #endregion

        private async void Reply_Win_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();
        }

        private void menu_lvl2_month_Click(object sender, RoutedEventArgs e)
        {
            grid_day.Visibility = System.Windows.Visibility.Collapsed;
            grid_month.Visibility = System.Windows.Visibility.Visible;
            grid_week.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// отчет за месяц
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_create_month_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < month_chart.Series.Count; i++)
            {
                month_chart.Series.Remove(month_chart.Series[i]);
            }
            values = null;
            List<Orders> list_orders = await oOrdersRepository.Select_All_Orders_Async();
            List<Data> list_data = new List<Data>();
            for (int i = 0; i < list_orders.Count; i++)
            {
                Data d = new Data();
                if (list_orders[i].datetime.Month == DateTime.Now.Month)
                {
                    List<Carts> list_carts = await oCartsRepository.Select_Carts_Async("select count,bouquets_id from Carts where orders_id=" + list_orders[i].orders_id);
                    d.time = list_orders[i].datetime.Date.Day.ToString()+ "/" + list_orders[i].datetime.Month.ToString() + "/" + list_orders[i].datetime.Year.ToString();
                    foreach (var c in list_carts)
                    {
                        d.count = (int)c.count;
                        d.bouqet_id = c.bouquets_id;
                        list_data.Add(d);
                    }
                }
            }

            List<Bouquets> list_bouq = await oBouquetsRepository.Select_All_Bouquets_Async();
            for (int i = 0; i < list_bouq.Count; i++)
            {
                values = new List<KeyValuePair<string, int>>();
                for (int j = 0; j < list_data.Count; j++)
                {
                    if (list_data[j].bouqet_id == list_bouq[i].bouquets_id)
                    {
                        values.Add(new KeyValuePair<string, int>(list_data[j].time, list_data[j].count));
                    }
                }
                createColSeries_Month(values, list_bouq[i].name);
            }
            button_create_month.IsEnabled = false;
        }

        private void menu_lvl2_range_Click(object sender, RoutedEventArgs e)
        {
            grid_week.Visibility = System.Windows.Visibility.Visible;
            grid_day.Visibility = System.Windows.Visibility.Collapsed;
            grid_month.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// отчет за неделю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_create_week_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < month_chart.Series.Count; i++)
            {
                month_chart.Series.Remove(month_chart.Series[i]);
            }
            values = null;
            List<Orders> list_orders = await oOrdersRepository.Select_All_Orders_Async();
            List<Data> list_data = new List<Data>();
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            for (int i = 0; i < list_orders.Count; i++)
            {
                Data d = new Data();
                if (cal.GetWeekOfYear(list_orders[i].datetime, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday) == cal.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday))
                {
                    List<Carts> list_carts = await oCartsRepository.Select_Carts_Async("select count,bouquets_id from Carts where orders_id=" + list_orders[i].orders_id);
                    d.time = list_orders[i].datetime.Day.ToString() + "," + list_orders[i].datetime.DayOfWeek.ToString();
                    foreach (var c in list_carts)
                    {
                        d.count = (int)c.count;
                        d.bouqet_id = c.bouquets_id;
                        list_data.Add(d);
                    }
                }
            }

            List<Bouquets> list_bouq = await oBouquetsRepository.Select_All_Bouquets_Async();
            for (int i = 0; i < list_bouq.Count; i++)
            {
                values = new List<KeyValuePair<string, int>>();
                for (int j = 0; j < list_data.Count; j++)
                {
                    if (list_data[j].bouqet_id == list_bouq[i].bouquets_id)
                    {
                        values.Add(new KeyValuePair<string, int>(list_data[j].time, list_data[j].count));
                    }
                }
                createColSeries_Week(values, list_bouq[i].name);
            }
            button_create_week.IsEnabled = false;
        }

        
    }
}
