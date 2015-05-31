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
using Excel = Microsoft.Office.Interop.Excel;

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
        public List<KeyValuePair<string, int>> values;
        public List<KeyValuePair<string, int>> pie_values;
        public List<PieData> list_pie;
        public List<Data> list_day_data;
        public List<Data> list_week_data;
        public List<Data> list_month_data;
        public List<Data> list_quater_data;
        Replies r = new Replies();


        public struct Data
        {
            public string time { get; set; }
            public int count { get; set; }
            public int bouqet_id { get; set; }
        }

        public struct PieData
        {
            public int count { get; set; }
            public int bouqet_id { get; set; }
            public string name { get; set; }
        }
        
        SQLite.SQLiteAsyncConnection conn;

        public Reply_Window()
        {
            InitializeComponent();
            grid_day.Visibility = System.Windows.Visibility.Collapsed;
            grid_month.Visibility = System.Windows.Visibility.Collapsed;
            grid_week.Visibility = System.Windows.Visibility.Collapsed;
            grid_quater.Visibility = System.Windows.Visibility.Collapsed;
            grid_stat.Visibility = System.Windows.Visibility.Collapsed;
            button_createExcel.IsEnabled = false;
            button_excel_month.IsEnabled = false;
            button_excel_week.IsEnabled = false;
            button_excel_quater.IsEnabled = false;
            button_excel_stat.IsEnabled = false;
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
            grid_quater.Visibility = System.Windows.Visibility.Collapsed;
            grid_stat.Visibility = System.Windows.Visibility.Collapsed;
        }
        /// <summary>
        /// построение графика - отчет за день
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_build_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < day_chart.Series.Count; i++)
            {
                day_chart.Series.Remove(day_chart.Series[i]);
            }
            values = new List<KeyValuePair<string, int>>();
            List<Orders> list_orders = await oOrdersRepository.Select_All_Orders_Async();
            List<Data> list_data = new List<Data>();
            for (int i = 0; i < list_orders.Count; i++)
            {
                Data d = new Data();
                if (list_orders[i].datetime.Date == datepicker.SelectedDate)
                {
                    List<Carts> list_carts = await oCartsRepository.Select_Carts_Async("select count,bouquets_id from Carts where orders_id=" + list_orders[i].orders_id);
                    d.time = list_orders[i].datetime.TimeOfDay.ToString();
                    foreach (var c in list_carts)
                    {
                        d.count = (int)c.count;
                        d.bouqet_id = c.bouquets_id;
                        list_data.Add(d);
                    }
                }
            }
            list_day_data = list_data;
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
                createLineSeries_Day(values, list_bouq[i].name);
                button_createExcel.IsEnabled = true;
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

        public void createColSeries_Quater(List<KeyValuePair<string, int>> values, string title)
        {
            ColumnSeries ls = new ColumnSeries();
            ls.Title = title;
            ls.DependentValuePath = "Value";
            ls.IndependentValuePath = "Key";
            ls.IsSelectionEnabled = true;
            ls.ItemsSource = values;
            quater_chart.Series.Add(ls);
        }

        public void createPieSeries_Stat(List<KeyValuePair<string, int>> values)
        {
            PieSeries ls = new PieSeries();
            ls.DependentValuePath = "Value";
            ls.IndependentValuePath = "Key";
            ls.IsSelectionEnabled = true;
            ls.ItemsSource = values;
            stat_chart.Series.Add(ls);
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
            grid_quater.Visibility = System.Windows.Visibility.Collapsed;
            grid_stat.Visibility = System.Windows.Visibility.Collapsed;
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
            list_month_data = list_data;
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
            button_excel_month.IsEnabled = true;
        }

        private void menu_lvl2_range_Click(object sender, RoutedEventArgs e)
        {
            grid_week.Visibility = System.Windows.Visibility.Visible;
            grid_day.Visibility = System.Windows.Visibility.Collapsed;
            grid_month.Visibility = System.Windows.Visibility.Collapsed;
            grid_quater.Visibility = System.Windows.Visibility.Collapsed;
            grid_stat.Visibility = System.Windows.Visibility.Collapsed;
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
                if (cal.GetWeekOfYear(list_orders[i].datetime, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Monday) == cal.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Monday))
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
            list_week_data = list_data;
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
            button_excel_week.IsEnabled = true;
        }

        private void menu_lvl2_quter_Click(object sender, RoutedEventArgs e)
        {
            grid_quater.Visibility = System.Windows.Visibility.Visible;
            grid_day.Visibility = System.Windows.Visibility.Collapsed;
            grid_month.Visibility = System.Windows.Visibility.Collapsed;
            grid_week.Visibility = System.Windows.Visibility.Collapsed;
            grid_stat.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void button_create_quater_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < quater_chart.Series.Count; i++)
            {
                quater_chart.Series.Remove(quater_chart.Series[i]);
            }
            values = null;
            List<Orders> list_orders = await oOrdersRepository.Select_All_Orders_Async();
            List<Data> list_data = new List<Data>();
            for (int i = 0; i < list_orders.Count; i++)
            {
                Data d = new Data();
                if ((list_orders[i].datetime.Month > DateTime.Now.Month - 2) && (list_orders[i].datetime.Month < DateTime.Now.Month + 2))
                {
                    List<Carts> list_carts = await oCartsRepository.Select_Carts_Async("select count,bouquets_id from Carts where orders_id=" + list_orders[i].orders_id);
                    d.time = list_orders[i].datetime.Date.Day.ToString() + "/" + list_orders[i].datetime.Month.ToString() + "/" + list_orders[i].datetime.Year.ToString();
                    foreach (var c in list_carts)
                    {
                        d.count = (int)c.count;
                        d.bouqet_id = c.bouquets_id;
                        list_data.Add(d);
                    }
                }
            }
            list_quater_data = list_data;
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
                createColSeries_Quater(values, list_bouq[i].name);
            }
            button_create_quater.IsEnabled = false;
            button_excel_quater.IsEnabled = true;
        }

        private void menu_lvl1_reply_Click(object sender, RoutedEventArgs e)
        {
            grid_stat.Visibility = System.Windows.Visibility.Visible;
            grid_day.Visibility = System.Windows.Visibility.Collapsed;
            grid_month.Visibility = System.Windows.Visibility.Collapsed;
            grid_week.Visibility = System.Windows.Visibility.Collapsed;
            grid_quater.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void button_create_stat_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < stat_chart.Series.Count; i++)
            {
                stat_chart.Series.Remove(stat_chart.Series[i]);
            }
            values = null;
            list_pie = new List<PieData>();
            List<Orders> list_orders = await oOrdersRepository.Select_All_Orders_Async();
            for(int i = 0; i < list_orders.Count; i++)
            {
                PieData pd = new PieData();
                List<Carts> list_carts = await oCartsRepository.Select_Carts_Async("select count,bouquets_id from Carts where orders_id=" + list_orders[i].orders_id);
                foreach(var n in list_carts)
                {
                    pd.count = (int)n.count;
                    pd.bouqet_id = n.bouquets_id;
                    Bouquets b = await conn.GetAsync<Bouquets>(n.bouquets_id);
                    pd.name = b.name;
                    list_pie.Add(pd);
                }
            }

            int min_id = 0;
            int max = 0;
            int min = 1000000;
            int max_id = 0;
            for (int i = 0; i < list_pie.Count; i++)
            {
                if(list_pie[i].count > max)
                {
                    max = list_pie[i].count;
                    max_id = i;
                }
            }
            for (int i = 0; i < list_pie.Count; i++)
            {
                if (list_pie[i].count < min)
                {
                    min = list_pie[i].count;
                    min_id = i;
                }
            }
            
            List<Bouquets> list_bouq = await oBouquetsRepository.Select_All_Bouquets_Async();

                pie_values = new List<KeyValuePair<string,int>>();
                string min_name = "";
                string max_name = "";
                for (int j = 0; j < list_pie.Count; j++)
                {
                    if (j == min_id || j == max_id)
                    {
                        pie_values.Add(new KeyValuePair<string, int>(list_pie[j].name,list_pie[j].count));
                        if(j == min_id)
                        {
                            min_name = list_pie[j].name;
                        }
                        if(j == max_id)
                        {
                            max_name = list_pie[j].name;
                        }
                    }
                }
                createPieSeries_Stat(pie_values);
            button_create_stat.IsEnabled = false;
            button_excel_stat.IsEnabled = true;
        }

        private void button_createExcel_Click(object sender, RoutedEventArgs e)
        {
            r.Create_Excel_Doc(list_day_data, this, conn, list_day_data, list_week_data, list_month_data, list_quater_data);
        }

        private void button_excel_month_Click(object sender, RoutedEventArgs e)
        {
            r.Create_Excel_Doc(list_month_data, this, conn, list_day_data, list_week_data, list_month_data, list_quater_data);
        }

        private void button_excel_week_Click(object sender, RoutedEventArgs e)
        {
            r.Create_Excel_Doc(list_week_data, this, conn, list_day_data, list_week_data, list_month_data, list_quater_data);
        }

        private void button_excel_quater_Click(object sender, RoutedEventArgs e)
        {
            r.Create_Excel_Doc(list_quater_data, this, conn, list_day_data, list_week_data, list_month_data, list_quater_data);
        }

        private void button_excel_stat_Click(object sender, RoutedEventArgs e)
        {
            r.Create_Excel_Doc(pie_values);
        }
    }
}
