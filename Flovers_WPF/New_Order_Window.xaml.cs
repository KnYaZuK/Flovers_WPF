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
    /// Interaction logic for New_Order_Window.xaml
    /// </summary>
    public partial class New_Order_Window : MetroWindow
    {
        public New_Order_Window()
        {
            InitializeComponent();
        }

        struct Client_Card
        {
            public Clients client { get; set; }
            public Cards card { get; set; }
        }

        struct Cart_Bouquet
        {
            public Bouquets bouquet { get; set; }
            public Carts cart { get; set; }
            public double cost { get; set; }
        }

        struct Bouquet_Content
        {
            public Bouquets bouquet { get; set; }
            public List<Contents> lContent { get; set; }
            public double cost { get; set; }
        }

        ClientsRepository oClientsRepository;
        BouquetsRepository oBouquetsRepository;
        CartsRepository oCartsRepository;
        ContentsRepository oContentsRepository;
        CardsRepository oCardsRepository;

        SQLite.SQLiteAsyncConnection conn;

        Client_Card oClient_Card;
        Cart_Bouquet oCart_Bouquet;
        Bouquet_Content oBouquet_Content;

        List<Cart_Bouquet> lCart_Bouquet;

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oClientsRepository = new ClientsRepository(oDBConnection);
            oCartsRepository = new CartsRepository(oDBConnection);
            oBouquetsRepository = new BouquetsRepository(oDBConnection);
            oContentsRepository = new ContentsRepository(oDBConnection);
            oCardsRepository = new CardsRepository(oDBConnection);

            conn = oDBConnection.GetAsyncConnection();

            lCart_Bouquet = new List<Cart_Bouquet>();

            await Update_ListView_Clients();
            await Update_ListView_Bouquets();

            Clear_Control_All();
        }

        private async Task Update_ListView_Clients()
        {
            List<Client_Card> lClient_Card = new List<Client_Card>();

            List<Clients> lClients = await oClientsRepository.Select_All_Clients_Async();

            foreach ( var c in lClients )
            {
                Client_Card client_card = new Client_Card();

                client_card.client = c;
                client_card.card = await conn.GetAsync<Cards>(c.cards_id);

                lClient_Card.Add(client_card);
            }

            listview_Clients.ItemsSource = lClient_Card;
        }

        private async Task Update_ListView_Bouquets()
        {
            List<Bouquet_Content> lBouquet_Content = new List<Bouquet_Content>();

            List<Bouquets> lBouquets = await oBouquetsRepository.Select_All_Bouquets_Async();

            foreach( var b in lBouquets )
            {
                Bouquet_Content bouquet_content = new Bouquet_Content();

                bouquet_content.bouquet = b;
                bouquet_content.lContent = await oContentsRepository.Select_Contents_Async("select * from contents where bouquets_id = " + b.bouquets_id );
                bouquet_content.cost = 0;

                foreach ( var c in bouquet_content.lContent )
                {
                    if ( c.accessories_id != -1 )
                    {
                        bouquet_content.cost += (double)c.count * conn.GetAsync<Accessories>(c.accessories_id).Result.price;
                    }
                    else
                    {
                        bouquet_content.cost += (double)c.count * conn.GetAsync<Flowers>(c.flowers_id).Result.price;
                    }
                }

                bouquet_content.cost += bouquet_content.bouquet.price_extra;

                lBouquet_Content.Add(bouquet_content);
            }

            listview_Bouquets.ItemsSource = lBouquet_Content;
        }

        private void Update_ListView_Carts()
        {
            listview_Carts.ItemsSource = null;
            listview_Carts.ItemsSource = lCart_Bouquet;

            double cost = 0;

            foreach ( var c in lCart_Bouquet )
            {
                cost += c.cost;
            }

            label_Cost.Content = "Общая стоимость: " + cost.ToString();
            label_Discount.Content = "Скидка: 0%";
            label_Total_Cost.Content = "Итого: " + cost;

            if ( cost > 1000 )
            {
                label_Discount.Content = "Скидка: 5%";
                label_Total_Cost.Content = "Итого: " + cost * 0.95;
            }

            if (cost > 5000)
            {
                label_Discount.Content = "Скидка: 10%";
                label_Total_Cost.Content = "Итого: " + cost * 0.9;
            }

            if (cost > 10000)
            {
                label_Discount.Content = "Скидка: 20%";
                label_Total_Cost.Content = "Итого: " + cost * 0.8;
            }
        }

        private void Clear_Control_All()
        {
            grid.DataContext = null;

            numeric_Count.IsEnabled = false;

            button_Create.IsEnabled = false;
            button_Delete.IsEnabled = false;
            button__Complete.IsEnabled = false;

            textbox_Address.IsEnabled = false;

            label_Cost.IsEnabled = false;
            label_Discount.IsEnabled = false;
            label_Total_Cost.IsEnabled = false;

            textbox_Search_Cart.IsEnabled = false;
            textbox_Search_Bouquet.IsEnabled = false;

            listview_Carts.IsEnabled = false;
            listview_Bouquets.IsEnabled = false;
        }

        private void Clear_Control()
        {
            grid.DataContext = null;

            numeric_Count.IsEnabled = false;

            button_Create.IsEnabled = false;
            button_Delete.IsEnabled = false;

            listview_Bouquets.SelectedIndex = -1;
        }

        private void button_Create_Click(object sender, RoutedEventArgs e)
        {
            Cart_Bouquet cart_bouquet = new Cart_Bouquet();
            cart_bouquet.bouquet = oBouquet_Content.bouquet;
            cart_bouquet.cart = new Carts((decimal) numeric_Count.Value, oBouquet_Content.bouquet.bouquets_id);
            cart_bouquet.cost = oBouquet_Content.cost;

            lCart_Bouquet.Add(cart_bouquet);

            Update_ListView_Carts();

            Clear_Control();

            button__Complete.IsEnabled = true;
        }

        private void button_Delete_Click(object sender, RoutedEventArgs e)
        {
            lCart_Bouquet.Remove(oCart_Bouquet);

            Update_ListView_Carts();

            Clear_Control();

            listview_Carts.SelectedIndex = -1;
        }

        private void button__Complete_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Forms.MessageBox.Show("Заказ успешно создан!");
        }

        private void listview_Clients_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            oClient_Card = (Client_Card) listview_Clients.SelectedItem;

            textbox_Address.IsEnabled = true;

            label_Cost.IsEnabled = true;
            label_Discount.IsEnabled = true;
            label_Total_Cost.IsEnabled = true;

            textbox_Search_Cart.IsEnabled = true;
            textbox_Search_Bouquet.IsEnabled = true;

            listview_Carts.IsEnabled = true;
            listview_Bouquets.IsEnabled = true;
        }

        private void listview_Clients_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Clients.SelectedIndex = -1;

            Clear_Control_All();
        }

        private void listview_Carts_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            oCart_Bouquet = (Cart_Bouquet)listview_Carts.SelectedItem;

            button_Delete.IsEnabled = true;
        }

        private void listview_Carts_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Carts.SelectedIndex = -1;

            button_Delete.IsEnabled = false;
        }

        private void listview_Bouquets_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            oBouquet_Content = (Bouquet_Content)listview_Bouquets.SelectedItem;

            numeric_Count.IsEnabled = true;
            numeric_Count.Value = 1;

            button_Create.IsEnabled = true;

        }

        private void listview_Bouquets_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Bouquets.SelectedIndex = -1;

            Clear_Control();
        }

        private void button_New_Client_Click(object sender, RoutedEventArgs e)
        {
            NewClientWindow ncw = new NewClientWindow();

            ncw.ShowDialog();
        }

        private void button_choose_adress_Click(object sender, RoutedEventArgs e)
        {
            New_Adress_Window adress_window = new New_Adress_Window();
            adress_window.ShowDialog();
        }
    }
}
