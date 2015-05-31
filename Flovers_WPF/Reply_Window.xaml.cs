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
        List<KeyValuePair<string, int>> values;
        List<KeyValuePair<string, int>> pie_values;
        List<PieData> list_pie;
        List<Data> list_day_data;
        List<Data> list_week_data;
        List<Data> list_month_data;
        List<Data> list_quater_data;

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
            list_day_data = list_data;
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

        /// <summary>
        /// Создание Excel файла для отчетов
        /// </summary>
        /// <param name="values"></param>
        /// 
        public async void Create_Excel_Doc(List<Data> values)
        {
            Excel.Application exapp = new Excel.Application();
            exapp.SheetsInNewWorkbook = 1;
            exapp.Workbooks.Add(Type.Missing);
            exapp.DisplayAlerts = true;
            exapp.Visible = true;
            Excel.Workbooks exappworkbooks = exapp.Workbooks;
            Excel.Workbook exappworkbook = exappworkbooks[1];
            exappworkbook.Saved = false;
            Excel.Sheets excellsheets = exappworkbook.Worksheets;
            Excel.Worksheet excellworksheet = (Excel.Worksheet)excellsheets.get_Item(1);
            excellworksheet.Activate();
            Excel.Range excelcells;
            for (int j = 1; j < 4; j++)
            {
                if (j == 1)
                {
                    excelcells = (Excel.Range)excellworksheet.Cells[1, j];
                    excelcells.Value2 = "время/дата";
                    excelcells.Font.Size = 12;
                    excelcells.Font.Italic = true;
                    excelcells.Font.Bold = true;
                    excelcells.HorizontalAlignment = Excel.Constants.xlCenter;
                    excelcells.VerticalAlignment = Excel.Constants.xlCenter;
                }
                if (j == 2)
                {
                    excelcells = (Excel.Range)excellworksheet.Cells[1, j];
                    excelcells.Value2 = "Количество";
                    excelcells.Font.Size = 12;
                    excelcells.Font.Italic = true;
                    excelcells.Font.Bold = true;
                    excelcells.HorizontalAlignment = Excel.Constants.xlCenter;
                    excelcells.VerticalAlignment = Excel.Constants.xlCenter;
                }
                if (j == 3)
                {
                    excelcells = (Excel.Range)excellworksheet.Cells[1, j];
                    excelcells.Value2 = "Букет";
                    excelcells.Font.Size = 12;
                    excelcells.Font.Italic = true;
                    excelcells.Font.Bold = true;
                    excelcells.HorizontalAlignment = Excel.Constants.xlCenter;
                    excelcells.VerticalAlignment = Excel.Constants.xlCenter;
                }
            }
            if (values != null)
            {
                for (int m = 2; m < values.Count + 2; m++)
                {
                    for (int n = 1; n < 4; n++)
                    {
                        if (n == 1)
                        {
                            excelcells = (Excel.Range)excellworksheet.Cells[m, n];
                            excelcells.Value2 = values[m - 2].time.ToString();
                            excelcells.Font.Size = 12;
                            excelcells.Font.Italic = true;
                            excelcells.Font.Bold = false;
                            excelcells.HorizontalAlignment = Excel.Constants.xlCenter;
                            excelcells.VerticalAlignment = Excel.Constants.xlCenter;
                        }
                        if (n == 2)
                        {
                            excelcells = (Excel.Range)excellworksheet.Cells[m, n];
                            excelcells.Value2 = values[m - 2].count.ToString();
                            excelcells.Font.Size = 12;
                            excelcells.Font.Italic = true;
                            excelcells.Font.Bold = false;
                            excelcells.HorizontalAlignment = Excel.Constants.xlCenter;
                            excelcells.VerticalAlignment = Excel.Constants.xlCenter;
                        }
                        if (n == 3)
                        {
                            Bouquets b = await conn.GetAsync<Bouquets>(values[m - 2].bouqet_id);
                            excelcells = (Excel.Range)excellworksheet.Cells[m, n];
                            excelcells.Value2 = b.name.ToString();
                            excelcells.Font.Size = 12;
                            excelcells.Font.Italic = true;
                            excelcells.Font.Bold = false;
                            excelcells.HorizontalAlignment = Excel.Constants.xlCenter;
                            excelcells.VerticalAlignment = Excel.Constants.xlCenter;
                        }
                    }
                }


                Excel.ChartObjects chartsobjrcts = (Excel.ChartObjects)excellworksheet.ChartObjects(Type.Missing);
                Excel.ChartObject chartsobj = chartsobjrcts.Add(10, 200, 500, 300);
                Excel.Chart excelchart = chartsobj.Chart;
                excelcells = excellworksheet.get_Range("A1", "B" + (values.Count + 1).ToString());
                excelchart.SetSourceData(excelcells, Type.Missing);
                excelchart.ChartType = Excel.XlChartType.xlLine;
                excelchart.HasTitle = true;
                if (values == list_day_data)
                {
                    excelchart.ChartTitle.Text = "Продажи за день";
                }
                if (values == list_week_data)
                {
                    excelchart.ChartTitle.Text = "Продажи за неделю";
                }
                if (values == list_month_data)
                {
                    excelchart.ChartTitle.Text = "Продажи за месяц";
                }
                if (values == list_quater_data)
                {
                    excelchart.ChartTitle.Text = "Продажи за квартал";
                }
                excelchart.ChartTitle.Font.Size = 14;
                excelchart.ChartTitle.Font.Color = 255;
                excelchart.ChartTitle.Shadow = true;
            }
            else
            {
                System.Windows.MessageBox.Show("нет данных для отчета");
                exapp.Quit();
            }
        }
        /// <summary>
        /// Создание Excel файла для статистики продажи
        /// </summary>
        /// <param name="pie_values"></param>
        /// 
        public void Create_Excel_Doc(List<KeyValuePair<string, int>> pie_values)
        {
            if (pie_values != null)
            {
                Excel.Application exapp = new Excel.Application();
                exapp.SheetsInNewWorkbook = 1;
                exapp.Workbooks.Add(Type.Missing);
                exapp.DisplayAlerts = true;
                exapp.Visible = true;
                Excel.Workbooks exappworkbooks = exapp.Workbooks;
                Excel.Workbook exappworkbook = exappworkbooks[1];
                exappworkbook.Saved = false;
                Excel.Sheets excellsheets = exappworkbook.Worksheets;
                Excel.Worksheet excellworksheet = (Excel.Worksheet)excellsheets.get_Item(1);
                excellworksheet.Activate();
                Excel.Range excelcells;

                exapp.Cells[1, 1] = "Букет";
                exapp.Cells[1, 2] = "Количество";
                exapp.Cells[2, 1] = pie_values[0].Key;
                exapp.Cells[2, 2] = pie_values[0].Value;
                exapp.Cells[3, 1] = pie_values[1].Key;
                exapp.Cells[3, 2] = pie_values[1].Value;

                Excel.ChartObjects chartsobjrcts = (Excel.ChartObjects)excellworksheet.ChartObjects(Type.Missing);
                Excel.ChartObject chartsobj = chartsobjrcts.Add(10, 200, 500, 300);
                Excel.Chart excelchart = chartsobj.Chart;
                excelcells = excellworksheet.get_Range("A1", "B3");
                excelchart.SetSourceData(excelcells, Type.Missing);
                excelchart.ChartType = Excel.XlChartType.xlPie;
                excelchart.HasTitle = true;
                excelchart.ChartTitle.Text = "Наиболее/наименее продаваемый букет";
            }
            else
            {
                System.Windows.MessageBox.Show("нет данных для отчета");
            }
        }

        private void button_createExcel_Click(object sender, RoutedEventArgs e)
        {
            Create_Excel_Doc(list_day_data);
        }

        private void button_excel_month_Click(object sender, RoutedEventArgs e)
        {
            Create_Excel_Doc(list_month_data);
        }

        private void button_excel_week_Click(object sender, RoutedEventArgs e)
        {
            Create_Excel_Doc(list_week_data);
        }

        private void button_excel_quater_Click(object sender, RoutedEventArgs e)
        {
            Create_Excel_Doc(list_quater_data);
        }

        private void button_excel_stat_Click(object sender, RoutedEventArgs e)
        {
            Create_Excel_Doc(pie_values);
        }
    }
}
