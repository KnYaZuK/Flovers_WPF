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
    /// Interaction logic for CreateFlowerWindow.xaml
    /// </summary>
    public partial class CreateFlowerWindow : MetroWindow
    {
        public CreateFlowerWindow()
        {
            InitializeComponent();
        }

        struct Accessories_Flowers
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public decimal Count { get; set; }
            //public Accessories accessories { get; set; }
            //public Flowers flower { get; set; }
        }

        BouquetsRepository oBouquetsRepository;
        ContentsRepository oContentsRepository;
        AccessoriesRepository oAccessoriesRepository;
        FlowersRepository oFlowersRepository;

        Bouquets bouquet;
        Accessories accessories;
        Flowers flowers;

        SQLite.SQLiteAsyncConnection conn;

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oBouquetsRepository = new BouquetsRepository(oDBConnection);
            oContentsRepository = new ContentsRepository(oDBConnection);
            oAccessoriesRepository = new AccessoriesRepository(oDBConnection);
            oFlowersRepository = new FlowersRepository(oDBConnection);

            conn = oDBConnection.GetAsyncConnection();

            await Update_ListView_Bouquets();

            Clear_Control_Bouquet();
        }

        private async Task Update_ListView_Bouquets()
        {
            List<Bouquets> result = await oBouquetsRepository.Select_All_Bouquets_Async();

            listview_Bouquet.ItemsSource = result;
        }

        private async Task Update_ListView_Contents()
        {
            List<Accessories_Flowers> acc_flo = new List<Accessories_Flowers>();

            List<Contents> result = await oContentsRepository.Select_All_Contents_Async();

            foreach(var c in result)
            {
                Accessories_Flowers af = new Accessories_Flowers();

                if ( c.accessories_id != -1 )
                {
                    Accessories a = await conn.GetAsync<Accessories>(c.accessories_id);
                    af.Type = "Аксессуар";
                    af.Name = a.name;
                    af.Count = c.count;
                }
                else
                {
                    Flowers f = await conn.GetAsync<Flowers>(c.flowers_id);
                    af.Type = "Цветок";
                    af.Name = f.name;
                    af.Count = c.count;
                }

                acc_flo.Add(af);
            }

            listview_Content.ItemsSource = acc_flo;
        }

        private async Task Update_ComboBox_Contents( bool type )
        {
            if ( type )
            {
                List<Accessories> result = await oAccessoriesRepository.Select_All_Accessories_Async();

                combobox_Content.ItemsSource = result;
            }
            else
            {
                List<Flowers> result = await oFlowersRepository.Select_All_Flowers_Async();

                combobox_Content.ItemsSource = result;
            }
        }

        private void Clear_Control_Bouquet()
        {
            button_CreateBouquet.IsEnabled = true;
            button_UpdateBouquet.IsEnabled = false;
            grid_Content.IsEnabled = false;
        }

        private void Clear_Control_Content()
        {
            button_CreateContent.IsEnabled = true;
            button_UpdateContent.IsEnabled = false;
            button_DeleteContent.IsEnabled = false;
        }

        private async void button_CreateBouquet_Click(object sender, RoutedEventArgs e)
        {
            await oBouquetsRepository.Insert_Bouquets_Async(new Bouquets(textbox_NameBouquet.Text, Double.Parse(textbox_PriceExtraBouquet.Text)));

            await Update_ListView_Bouquets();

            Clear_Control_Bouquet();
        }

        private async void button_UpdateBouquet_Click(object sender, RoutedEventArgs e)
        {
            bouquet.name = textbox_NameBouquet.Text;
            bouquet.price_extra = Double.Parse(textbox_PriceExtraBouquet.Text);

            await oBouquetsRepository.Update_Bouquets_Async(bouquet);

            await Update_ListView_Bouquets();
        }

        private async void button_CreateContent_Click(object sender, RoutedEventArgs e)
        {
            if ( combobox_TypeContent.SelectedIndex == 0)
            {
                await oContentsRepository.Insert_Contents_Async(new Contents( (decimal) numericupdown_CountContent.Value, bouquet.bouquets_id, -1, accessories.accessories_id));
            }

            if (combobox_TypeContent.SelectedIndex == 1)
            {
                await oContentsRepository.Insert_Contents_Async(new Contents( (decimal) numericupdown_CountContent.Value, bouquet.bouquets_id, flowers.flowers_id, -1));
            }

            await Update_ListView_Contents();

            Clear_Control_Content();

        }

        private void button_UpdateContent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_DeleteContent_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void listview_Bouquet_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bouquet = listview_Bouquet.SelectedItem as Bouquets;

            grid_Bouquet.DataContext = bouquet;

            button_CreateBouquet.IsEnabled = false;
            button_UpdateBouquet.IsEnabled = true;
            grid_Content.IsEnabled = true;

            await Update_ListView_Contents();

            Clear_Control_Content();
        }

        private void listview_Bouquet_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Bouquet.SelectedIndex = -1;

            grid_Bouquet.DataContext = null;
            listview_Content.ItemsSource = null;

            Clear_Control_Bouquet();
        }

        private void listview_Content_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_CreateBouquet.IsEnabled = false;
            button_UpdateContent.IsEnabled = true;
            button_DeleteContent.IsEnabled = true;
        }

        private void listview_Content_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Content.SelectedIndex = -1;

            grid_Content.DataContext = null;

            Clear_Control_Content();
        }

        private async void combobox_TypeContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_TypeContent.SelectedIndex == 0)
            {
                await Update_ComboBox_Contents(true);
            }

            if (combobox_TypeContent.SelectedIndex == 1)
            {
                await Update_ComboBox_Contents(false);
            }
        }

        private void combobox_Content_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_TypeContent.SelectedIndex == 0)
            {
                accessories = combobox_Content.SelectedItem as Accessories;
            }

            if (combobox_TypeContent.SelectedIndex == 1)
            {
                flowers = combobox_Content.SelectedItem as Flowers;
            }
        }
    }
}
