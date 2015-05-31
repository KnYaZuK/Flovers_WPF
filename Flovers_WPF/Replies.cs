using Flovers_WPF.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Flovers_WPF;

namespace Flovers_WPF
{
    class Replies
    {
        /// <summary>
        /// создание Excel отчета за день/неделю/месяц/квартал
        /// </summary>
        /// <param name="values"></param>
        /// <param name="win"></param>
        /// <param name="conn"></param>
        /// <param name="list_day_data"></param>
        /// <param name="list_week_data"></param>
        /// <param name="list_month_data"></param>
        /// <param name="list_quater_data"></param>
        public async void Create_Excel_Doc(List<Flovers_WPF.Reply_Window.Data> values, Reply_Window win, SQLite.SQLiteAsyncConnection conn, List<Flovers_WPF.Reply_Window.Data> list_day_data, 
            List<Flovers_WPF.Reply_Window.Data> list_week_data, List<Flovers_WPF.Reply_Window.Data> list_month_data, List<Flovers_WPF.Reply_Window.Data> list_quater_data)
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
        /// создание Excel отчета о наиболее/наименее продаваемом букете
        /// </summary>
        /// <param name="pie_values"></param>
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
    }
}
