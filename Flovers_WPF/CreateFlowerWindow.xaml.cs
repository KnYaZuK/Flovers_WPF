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

        BouquetsRepository oBouquetsRepository;
        ContentsRepository oContentsRepository;

        Bouquets bouquet;

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            oDBConnection.InitializeDatabase();

            oBouquetsRepository = new BouquetsRepository(oDBConnection);
            oContentsRepository = new ContentsRepository(oDBConnection);

            await Update_ListViewBouquets();

            Clear_Control_Bouquet();
        }

        private async Task Update_ListViewBouquets()
        {
            List<Bouquets> result = await oBouquetsRepository.Select_All_Bouquets_Async();

            listview_Bouquet.ItemsSource = result;
        }

        private async Task Update_ListViewContents()
        {
            List<Contents> result = await oContentsRepository.Select_All_Contents_Async();

            listview_Content.ItemsSource = result;
        }

        private void Clear_Control_Bouquet()
        {
            button_CreateBouquet.IsEnabled = true;
            button_UpdateBouquet.IsEnabled = false;
            grid_Content.IsEnabled = false;
        }

        private void Clear_Control_Content()
        {
            button_UpdateContent.IsEnabled = false;
            button_DeleteContent.IsEnabled = false;
        }

        private async void button_CreateBouquet_Click(object sender, RoutedEventArgs e)
        {
            await oBouquetsRepository.Insert_Bouquets_Async(new Bouquets(textbox_NameBouquet.Text, Double.Parse(textbox_PriceExtraBouquet.Text)));

            await Update_ListViewBouquets();

            Clear_Control_Bouquet();
        }

        private async void button_UpdateBouquet_Click(object sender, RoutedEventArgs e)
        {
            bouquet.name = textbox_NameBouquet.Text;
            bouquet.price_extra = Double.Parse(textbox_PriceExtraBouquet.Text);

            await oBouquetsRepository.Update_Bouquets_Async(bouquet);

            await Update_ListViewBouquets();
        }

        private void button_CreateContent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_UpdateContent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_DeleteContent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listview_Bouquet_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bouquet = listview_Bouquet.SelectedItem as Bouquets;

            grid_Bouquet.DataContext = bouquet;

            button_CreateBouquet.IsEnabled = false;
            button_UpdateBouquet.IsEnabled = true;
            grid_Content.IsEnabled = true;

            Clear_Control_Content();
        }

        private void listview_Bouquet_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Bouquet.SelectedIndex = -1;

            grid_Bouquet.DataContext = null;

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
    }
}
