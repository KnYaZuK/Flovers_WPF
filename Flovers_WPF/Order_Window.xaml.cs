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
    /// Interaction logic for Order_Window.xaml
    /// </summary>
    public partial class Order_Window : MetroWindow
    {
        public Order_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Структура для работы с заказом и его клиентом
        /// </summary>
        struct Clients_Orders
        {
            public Clients client { get; set; }
            public Orders order { get; set; }
        }

        /// <summary>
        /// Структура для работы с корзиной и её букетами
        /// </summary>
        struct Carts_Bouquets
        {
            public Carts cart { get; set; }
            public Bouquets bouquet { get; set; }
        }

        bool first_launch = true;

        ClientsRepository oClientsRepository;   //
        OrdersRepository oOrdersRepository;     //  Контроллеры
        CartsRepository oCartsRepository;       //  Таблиц
        BouquetsRepository oBouquetsRepository; //

        SQLite.SQLiteAsyncConnection conn;      //  Прямой коннект к БД для выдачи записей из таблиц по ID

        Clients_Orders oClients_Orders;         //  Выделенные в списках объекты
        //Carts_Bouquets oCarts_Bouquets; //

        /// <summary>
        /// Загрузка окна, инициализация контроллеров, загрузка в listview данных и сброс управления.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oClientsRepository = new ClientsRepository(oDBConnection);
            oOrdersRepository = new OrdersRepository(oDBConnection);
            oCartsRepository = new CartsRepository(oDBConnection);
            oBouquetsRepository = new BouquetsRepository(oDBConnection);

            conn = oDBConnection.GetAsyncConnection();

            await Update_ListView_Orders();

            Clear_Control();
        }

        /// <summary>
        /// Обновление listview с Заказами. Ставим в соответствие каждому заказу его клиента.
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Orders()
        {
            List<Clients_Orders> lClients_Orders = new List<Clients_Orders>();
            List<Orders> lOrders = new List<Orders>();
            lOrders = await oOrdersRepository.Select_All_Orders_Async();

            foreach ( var o in lOrders )
            {
                Clients_Orders clients_orders = new Clients_Orders();

                clients_orders.order = o;
                clients_orders.client = await conn.GetAsync<Clients>(o.clients_id);

                lClients_Orders.Add(clients_orders);
            }

            listview_Orders.ItemsSource = lClients_Orders;
        }

        /// <summary>
        /// Обновление listview с Корзиной заказа. Содержит элементы корзины выделенного заказа.
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Carts()
        {
            List<Carts_Bouquets> lCarts_Bouquets = new List<Carts_Bouquets>();
            List<Carts> lCarts = await oCartsRepository.Select_Carts_Async("select * from carts where carts.orders_id = " + oClients_Orders.order.orders_id);

            foreach ( var c in lCarts )
            {
                Carts_Bouquets carts_bouquets = new Carts_Bouquets();

                carts_bouquets.cart = c;
                carts_bouquets.bouquet = await conn.GetAsync<Bouquets>(c.bouquets_id);

                lCarts_Bouquets.Add(carts_bouquets);
            }

            listview_Carts.ItemsSource = lCarts_Bouquets;
        }

        /// <summary>
        /// Сброс управления
        /// </summary>
        private void Clear_Control()
        {
            listview_Carts.ItemsSource = null;

            button_Status_Change.IsEnabled = false;
            button_Delete.IsEnabled = false;
        }

        /// <summary>
        /// Открытие диалогового окна создания нового заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_New_Order_Click(object sender, RoutedEventArgs e)
        {
            New_Order_Window NOW = new New_Order_Window();
            NOW.Show();
        }

        /// <summary>
        /// Выделение заказа в listview Заказов ЛКМ. Включение управления соответствующих элементов. Обновление listview Корзины, соответствующей выделенному заказу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listview_Orders_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            oClients_Orders = (Clients_Orders)listview_Orders.SelectedItem;

            await Update_ListView_Carts();

            button_Status_Change.IsEnabled = true;
            button_Delete.IsEnabled = true;
        }

        /// <summary>
        /// Снятие выделение с заказа в listview Заказов ПКМ. Сброс управления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Orders_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Orders.SelectedIndex = -1;

            Clear_Control();
        }

        /// <summary>
        /// При повторной активации окна обновляет listview с Заказами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MetroWindow_Activated(object sender, EventArgs e)
        {
            if (!first_launch)
                await Update_ListView_Orders();
            else
                first_launch = false;
        }

        /// <summary>
        /// Удаление выделенного заказа.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Delete_Click(object sender, RoutedEventArgs e)
        {
            await oOrdersRepository.Delete_Orders_Async(oClients_Orders.order);

            List<Carts> lCarts = await oCartsRepository.Select_Carts_Async("select * from carts where carts.orders_id = " + oClients_Orders.order.orders_id);

            foreach ( var c in lCarts )
            {
                await oCartsRepository.Delete_Carts_Async(c);
            }

            await Update_ListView_Orders();
        }
    }
}
