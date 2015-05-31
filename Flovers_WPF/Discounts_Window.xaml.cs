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

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for Discounts_Window.xaml
    /// </summary>
    public partial class Discounts_Window : MetroWindow
    {
        public Discounts_Window()
        {
            InitializeComponent();
        }

        DiscountsRepository oDiscountsRepository;

        Discounts oDiscounts;
        
        /// <summary>
        /// Загрузка окна, инициализация контроллеров и БД, обновление listview, сброс управления.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Const_Win_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oDiscountsRepository = new DiscountsRepository(oDBConnection);

            await Update_ListView_Discounts();
        }

        /// <summary>
        /// Обновление listview элементами из БД таблицы Констант
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Discounts()
        {
            listview_const.ItemsSource = await oDiscountsRepository.Select_All_Discounts_Async();

            Clear_Control();
        }

        /// <summary>
        /// Сброс управления
        /// </summary>
        private void Clear_Control()
        {
            oDiscounts = null;

            button_Create.IsEnabled = true;
            button_Update.IsEnabled = false;
            button_Delete.IsEnabled = false;

            numericUD_Percent.Value = 0;
            numericUD_Value.Value = 0.00;

            listview_const.SelectedIndex = -1;
        }

        /// <summary>
        /// Создане новой записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Create_Click(object sender, RoutedEventArgs e)
        {
            oDiscounts = new Discounts();

            oDiscounts.percent = (double)numericUD_Percent.Value;
            oDiscounts.value = (double)numericUD_Value.Value;

            await oDiscountsRepository.Insert_Discounts_Async(oDiscounts);

            oDiscounts = null;

            await Update_ListView_Discounts();
        }

        /// <summary>
        /// Изменение выделенной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Update_Click(object sender, RoutedEventArgs e)
        {
            oDiscounts.percent = (double)numericUD_Percent.Value;
            oDiscounts.value = (double)numericUD_Value.Value;

            await oDiscountsRepository.Update_Discounts_Async(oDiscounts);

            await Update_ListView_Discounts();
        }

        /// <summary>
        /// Удаление выделенной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Delete_Click(object sender, RoutedEventArgs e)
        {
            await oDiscountsRepository.Delete_Discounts_Async(oDiscounts);

            await Update_ListView_Discounts();
        }

        /// <summary>
        /// Выделение элемента в listview Констант ЛКМ и хранение выделенной константы. Если выбранная константа имела название Address или Phone_Number, то её можно только редактировать.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_const_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                oDiscounts = (Discounts)listview_const.SelectedItem;

                numericUD_Percent.Value = oDiscounts.percent;
                numericUD_Value.Value = oDiscounts.value;

                button_Create.IsEnabled = false;
                button_Update.IsEnabled = true;
                button_Delete.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("Элемент не выбран");
            }
        }

        /// <summary>
        /// Снятие выделения с элемента в listview Констант ПКМ.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_const_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control();
        }
    }
}
