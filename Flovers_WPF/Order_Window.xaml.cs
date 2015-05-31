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
        CardsRepository oCardsRepository;       //
        PaymentsRepository oPaymentsRepository;

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
            oCardsRepository = new CardsRepository(oDBConnection);
            oPaymentsRepository = new PaymentsRepository(oDBConnection);

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
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task Update_ListView_Orders( string name)
        {
            List<Clients_Orders> lClients_Orders = new List<Clients_Orders>();
            List<Orders> lOrders = new List<Orders>();
            lOrders = await oOrdersRepository.Select_All_Orders_Async();

            foreach (var o in lOrders)
            {
                Clients_Orders clients_orders = new Clients_Orders();

                clients_orders.order = o;
                clients_orders.client = await conn.GetAsync<Clients>(o.clients_id);

                if ( clients_orders.client.full_name.ToLower().Contains(textbox_Search_Orders.Text.ToLower()))
                {
                    lClients_Orders.Add(clients_orders);
                }
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
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Payments()
        {
            listview_Payments.ItemsSource = await oPaymentsRepository.Select_Payments_Async("select * from payments where orders_id = " + oClients_Orders.order.orders_id);
        }

        /// <summary>
        /// Сброс управления
        /// </summary>
        private void Clear_Control()
        {
            listview_Orders.SelectedIndex = -1;

            listview_Carts.ItemsSource = null;
            listview_Payments.ItemsSource = null;

            combobox_Status.SelectedIndex = -1;

            grid_Status.IsEnabled = false;

            grid_Payments.IsEnabled = false;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Status_Change_Click(object sender, RoutedEventArgs e)
        {
            switch ( combobox_Status.SelectedIndex )
            {
                case 0: // В обработке
                    {
                        oClients_Orders.order.status = combobox_Status.Text;
                    }
                    break;
                case 1: // Готовится к отправке
                    {
                        oClients_Orders.order.status = combobox_Status.Text;
                    }
                    break;

                case 2: // Отправлен
                    {
                        oClients_Orders.order.status = combobox_Status.Text;
                    }
                    break;

                case 3: // Завершён
                    {
                        
                    }
                    break;
            }

            await oOrdersRepository.Update_Orders_Async(oClients_Orders.order);

            await Update_ListView_Orders();

            Clear_Control();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Insert_Payments_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на внесение в оплату корректной суммы денег + баллов
            if ((numericUD_Orders_Price.Value + numericUD_Cards_Points.Value) == oClients_Orders.order.price)
            {
                // Создание записи оплаты
                Payments oPayment = new Payments(DateTime.Now, (double)numericUD_Orders_Price.Value, (decimal)numericUD_Cards_Points.Value, oClients_Orders.order.orders_id);

                // Вставка в БД записи об оплате и проверка на точную сумму
                await oPaymentsRepository.Insert_Payments_Async(oPayment);

                // Вычитание из карточки клиента баллов, использованных в оплате заказа.
                Cards card_of_client = await conn.GetAsync<Cards>(oClients_Orders.client.cards_id);
                card_of_client.bonus_card_points -= (int)oPayment.value_points;

                await oCardsRepository.Update_Cards_Async(card_of_client);

                // Начисление баллов вышестоящим по иерархии людям. 7%, 2% и 1%.
                // Мой реферер получает 7%, его 3% и сл. 1%.
                if (oClients_Orders.client.referer_id != -1)
                {
                    // Начисление баллов первому клиенту по иерархии. 7%
                    Clients client = await conn.GetAsync<Clients>(oClients_Orders.client.referer_id);
                    Cards card = await conn.GetAsync<Cards>(client.cards_id);

                    card.bonus_card_points += (int)(oPayment.value_money * 0.07);

                    await oCardsRepository.Update_Cards_Async(card);

                    if (client.referer_id != -1)
                    {
                        // Начисление баллов второму клиенту по иерархии. 2%
                        client = await conn.GetAsync<Clients>(oClients_Orders.client.referer_id);
                        card = await conn.GetAsync<Cards>(client.cards_id);

                        card.bonus_card_points += (int)(oPayment.value_money * 0.02);

                        await oCardsRepository.Update_Cards_Async(card);

                        if (client.referer_id != -1)
                        {
                            // Начисление баллов третьему клиенту по иерархии. 1%
                            client = await conn.GetAsync<Clients>(oClients_Orders.client.referer_id);
                            card = await conn.GetAsync<Cards>(client.cards_id);

                            card.bonus_card_points += (int)(oPayment.value_money * 0.01);

                            await oCardsRepository.Update_Cards_Async(card);
                        }
                    }
                }
                
                // Изменение статуса заказа
                oClients_Orders.order.status = combobox_Status.Text;
                await oOrdersRepository.Update_Orders_Async(oClients_Orders.order);

                await Update_ListView_Orders();
            }
            else
            {
                MessageBox.Show("Указана неверная сумма Денег и Баллов!");
            }

            Clear_Control();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Orders_Price_Max_Click(object sender, RoutedEventArgs e)
        {
            numericUD_Orders_Price.Value = oClients_Orders.order.price;
            numericUD_Cards_Points.Value = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Cards_Points_Max_Click(object sender, RoutedEventArgs e)
        {
            numericUD_Cards_Points.Value = (await conn.GetAsync<Cards>(oClients_Orders.client.cards_id)).bonus_card_points;

            if ( numericUD_Cards_Points.Value > oClients_Orders.order.price)
            {
                numericUD_Cards_Points.Value = oClients_Orders.order.price;
                numericUD_Orders_Price.Value = 0;
            }
            else
            {
                numericUD_Orders_Price.Value = oClients_Orders.order.price - numericUD_Cards_Points.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void combobox_Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (combobox_Status.SelectedIndex)
            {
                case 0: // В обработке
                    {
                        grid_Payments.IsEnabled = false;

                        button_Status_Change.IsEnabled = true;
                    }
                    break;
                case 1: // Готовится к отправке
                    {
                        grid_Payments.IsEnabled = false;

                        button_Status_Change.IsEnabled = true;
                    }
                    break;

                case 2: // Отправлен
                    {
                        grid_Payments.IsEnabled = false;

                        button_Status_Change.IsEnabled = true;
                    }
                    break;

                case 3: // Завершён
                    {
                        grid_Payments.IsEnabled = true;

                        button_Status_Change.IsEnabled = false;

                        numericUD_Cards_Points.Maximum = (await conn.GetAsync<Cards>(oClients_Orders.client.cards_id)).bonus_card_points;
                    }
                    break;
            }
        }

        /// <summary>
        /// Выделение заказа в listview Заказов ЛКМ. Включение управления соответствующих элементов. Обновление listview Корзины, соответствующей выделенному заказу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listview_Orders_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                oClients_Orders = (Clients_Orders)listview_Orders.SelectedItem;

                switch (oClients_Orders.order.status)
                {
                    case "В обработке":
                        {
                            combobox_Status.SelectedIndex = 0;

                            grid_Status.IsEnabled = true;
                        }
                        break;
                    case "Готовится к отправке":
                        {
                            combobox_Status.SelectedIndex = 1;

                            grid_Status.IsEnabled = true;
                        }
                        break;
                    case "Отправлен":
                        {
                            combobox_Status.SelectedIndex = 2;

                            grid_Status.IsEnabled = true;
                        }
                        break;
                    case "Завершён":
                        {
                            combobox_Status.SelectedIndex = 3;

                            grid_Payments.IsEnabled = false;
                        }
                        break;
                }

                await Update_ListView_Payments();

                await Update_ListView_Carts();

                button_Delete.IsEnabled = true;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Клиент не выбран!");
            }
        }

        /// <summary>
        /// Снятие выделение с заказа в listview Заказов ПКМ. Сброс управления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Orders_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void textbox_Search_Orders_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textbox_Search_Orders.Text != "")
            {
                await Update_ListView_Orders(textbox_Search_Orders.Text);
            }
            else
            {
                await Update_ListView_Orders();
            }
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
    }
}
