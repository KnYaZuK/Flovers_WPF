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
    /// Interaction logic for SpecialDealDiscountWindow.xaml
    /// </summary>
    public partial class SpecialDealDiscountWindow : MetroWindow
    {
        public SpecialDealDiscountWindow()
        {
            InitializeComponent();
        }

        struct Bouquets_SpecialDeals
        {
            public Bouquets bouquet { get; set; }
            public SpecialDeals specialdeal { get; set; }
        }

        BouquetsRepository oBouquetsRepository;
        SpecialDealsRepository oSpecialDealsRepository;


        Bouquets oBouquet; // Храним выделенный букет.
        Bouquets_SpecialDeals oBouquets_SpecialDeals;

        SQLite.SQLiteAsyncConnection conn;


        /// <summary>
        /// Инициализация переменных при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oBouquetsRepository = new BouquetsRepository(oDBConnection);
            oSpecialDealsRepository = new SpecialDealsRepository(oDBConnection);

            conn = oDBConnection.GetAsyncConnection();

            await Update_ListView_Bouquets();
            await Update_ListView_SpecialDeals();

            Clear_Control();
        }

        /// <summary>
        /// Обновление списка с букетами
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Bouquets()
        {
            listview_Bouquet.ItemsSource = await oBouquetsRepository.Select_All_Bouquets_Async();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task Update_ListView_Bouquets(string name)
        {
            List<Bouquets> query = await oBouquetsRepository.Select_All_Bouquets_Async();

            List<Bouquets> result = new List<Bouquets>();

            foreach (var b in query)
            {
                if (b.name.ToLower().Contains(name.ToLower()))
                {
                    result.Add(b);
                }
            }

            listview_Bouquet.ItemsSource = result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_SpecialDeals()
        {
            List<SpecialDeals> query = await oSpecialDealsRepository.Select_All_SpecialDeals_Async();

            List<Bouquets_SpecialDeals> result = new List<Bouquets_SpecialDeals>();

            foreach (var s in query)
            {
                Bouquets_SpecialDeals bouquet_specialdeal = new Bouquets_SpecialDeals();

                bouquet_specialdeal.specialdeal = s;
                bouquet_specialdeal.bouquet = await conn.GetAsync<Bouquets>(s.bouquets_id);

                result.Add(bouquet_specialdeal);
            }

            listview_SpecialDealDiscount.ItemsSource = result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private async Task Update_ListView_SpecialDeals(string text)
        {
            List<SpecialDeals> query = await oSpecialDealsRepository.Select_All_SpecialDeals_Async();

            List<Bouquets_SpecialDeals> result = new List<Bouquets_SpecialDeals>();

            foreach (var s in query)
            {
                Bouquets_SpecialDeals oBouquets_SpecialDeals = new Bouquets_SpecialDeals();

                oBouquets_SpecialDeals.specialdeal = s;
                oBouquets_SpecialDeals.bouquet = await conn.GetAsync<Bouquets>(s.bouquets_id);

                if (oBouquets_SpecialDeals.bouquet.name.ToLower().Contains(text.ToLower()))
                {
                    result.Add(oBouquets_SpecialDeals);
                }
            }

            listview_SpecialDealDiscount.ItemsSource = result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Clear_Control()
        {
            button_SpecialDeal_Create.IsEnabled = false;
            button_SpecialDeal_Update.IsEnabled = false;
            button_SpecialDeal_Delete.IsEnabled = false;

            numericUD_SpecialDeal_Percent.IsEnabled = false;
            numericUD_SpecialDeal_Percent.Value = null;

            datepicker.IsEnabled = false;
            datepicker.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Clear_Control_SpecialDeals()
        {
            listview_SpecialDealDiscount.SelectedIndex = -1;

            button_SpecialDeal_Update.IsEnabled = false;
            button_SpecialDeal_Delete.IsEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Clear_Control_Bouquets()
        {
            listview_Bouquet.SelectedIndex = -1;

            button_SpecialDeal_Create.IsEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_SpecialDeal_Create_Click(object sender, RoutedEventArgs e)
        {
            SpecialDeals query = null;
            try
            {
                query = (await oSpecialDealsRepository.Select_SpecialDeals_Async("select * from specialdeals where bouquets_id = " + oBouquet.bouquets_id))[0];

            }
            catch
            {
            }

            if (query == null)
            {
                await oSpecialDealsRepository.Insert_SpecialDeals_Async(new SpecialDeals(oBouquet.bouquets_id, (double)numericUD_SpecialDeal_Percent.Value, DateTime.Parse(datepicker.Text)));

                await Update_ListView_SpecialDeals();

                Special_Deal_Window oSpecial_Deal_Window = new Special_Deal_Window();
                oSpecial_Deal_Window.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Спец. предложения для данного букета уже существует.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_SpecialDeal_Update_Click(object sender, RoutedEventArgs e)
        {
            oBouquets_SpecialDeals.specialdeal.discount = (double)numericUD_SpecialDeal_Percent.Value;
            oBouquets_SpecialDeals.specialdeal.datetime = datepicker.SelectedDate.Value;

            await oSpecialDealsRepository.Update_SpecialDeals_Async(oBouquets_SpecialDeals.specialdeal);

            await Update_ListView_SpecialDeals();

            Clear_Control();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_SpecialDeal_Delete_Click(object sender, RoutedEventArgs e)
        {
            await oSpecialDealsRepository.Delete_SpecialDeals_Async(oBouquets_SpecialDeals.specialdeal);

            await Update_ListView_SpecialDeals();

            Clear_Control();
        }

        /// <summary>
        /// Выделение букета при нажатии ЛКМ и заполнение текстбоксов значениями этого букета. Запоминает выбранный букет.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Bouquet_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control_SpecialDeals();

            try
            {
                oBouquet = listview_Bouquet.SelectedItem as Bouquets;

                button_SpecialDeal_Create.IsEnabled = true;

                numericUD_SpecialDeal_Percent.IsEnabled = true;
                datepicker.IsEnabled = true;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Клиент не выбран!");
            }

        }

        /// <summary>
        /// Снятие выделения с букета при нажатии ПКМ и сброс интерфейса в состояние по умолчанию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Bouquet_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control();
            Clear_Control_Bouquets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_SpecialDealDiscount_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control_Bouquets();

            try
            {
                oBouquets_SpecialDeals = (Bouquets_SpecialDeals)listview_SpecialDealDiscount.SelectedItem;

                button_SpecialDeal_Update.IsEnabled = true;
                button_SpecialDeal_Delete.IsEnabled = true;

                numericUD_SpecialDeal_Percent.IsEnabled = true;
                numericUD_SpecialDeal_Percent.Value = oBouquets_SpecialDeals.specialdeal.discount;

                datepicker.IsEnabled = true;
                datepicker.SelectedDate = oBouquets_SpecialDeals.specialdeal.datetime;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Скидка специального предложения не выбрана!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_SpecialDealDiscount_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control();
            Clear_Control_SpecialDeals();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void textbox_Search_Bouquet_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textbox_Search_Bouquet.Text != "")
            {
                await Update_ListView_Bouquets(textbox_Search_Bouquet.Text);
            }
            else
            {
                await Update_ListView_Bouquets();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void textbox_Search_SpecialDealDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textbox_Search_SpecialDealDiscount.Text != "")
            {
                await Update_ListView_SpecialDeals(textbox_Search_SpecialDealDiscount.Text);
            }
            else
            {
                await Update_ListView_SpecialDeals();
            }
        }

        private void button_SpecialDeal_Window_Click(object sender, RoutedEventArgs e)
        {
            Special_Deal_Window oSpecial_Deal_Window = new Special_Deal_Window();
            oSpecial_Deal_Window.ShowDialog();
            this.Close();
        }
    }
}
