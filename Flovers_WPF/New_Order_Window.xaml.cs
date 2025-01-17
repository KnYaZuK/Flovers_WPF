﻿using MahApps.Metro.Controls;
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
        New_Adress_Window adress_window;
        public New_Order_Window()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Структура для работы с клиентом и его картой
        /// </summary>
        struct Client_Card
        {
            public Clients client { get; set; }
            public Cards card { get; set; }
        }


        /// <summary>
        /// Структура для работы с корзиной и букетом
        /// </summary>
        struct Cart_Bouquet
        {
            public Bouquets bouquet { get; set; }
            public Carts cart { get; set; }
            public double cost { get; set; }
        }


        /// <summary>
        /// Структура для работы с букетом и его компонентами
        /// </summary>
        struct Bouquet_Content
        {
            public Bouquets bouquet { get; set; }
            public List<Contents> lContent { get; set; }
            public double cost { get; set; }
        }

        OrdersRepository oOrdersRepository;         //
        ClientsRepository oClientsRepository;       //
        BouquetsRepository oBouquetsRepository;     // Контроллеры
        CartsRepository oCartsRepository;           // Таблиц
        ContentsRepository oContentsRepository;     //
        CardsRepository oCardsRepository;           //
        AccessoriesRepository oAccessoriesRepository;
        FlowersRepository oFlowersRepository;
        SpecialDealsRepository oSpecialDealsRepository;
        DiscountsRepository oDiscountRepository;

        SQLite.SQLiteAsyncConnection conn; // Прямой коннект к БД для выдачи записей из таблиц по ID

        Client_Card oClient_Card;           //
        Cart_Bouquet oCart_Bouquet;         // Выделенные в списках объекты
        Bouquet_Content oBouquet_Content;   //

        List<Cart_Bouquet> lCart_Bouquet;   // Список покупок клиента

        private bool Firts_Start;
        /// <summary>
        /// Загрузка окна, инициализация контроллеров, загрузка в listview данных и сброс управления.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Firts_Start = true;

            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();


            oOrdersRepository = new OrdersRepository(oDBConnection);

            oClientsRepository = new ClientsRepository(oDBConnection);
            oCardsRepository = new CardsRepository(oDBConnection);

            oBouquetsRepository = new BouquetsRepository(oDBConnection);
            oContentsRepository = new ContentsRepository(oDBConnection);

            oCartsRepository = new CartsRepository(oDBConnection);

            oAccessoriesRepository = new AccessoriesRepository(oDBConnection);
            oFlowersRepository = new FlowersRepository(oDBConnection);

            oSpecialDealsRepository = new SpecialDealsRepository(oDBConnection);
            oDiscountRepository = new DiscountsRepository(oDBConnection);

            conn = oDBConnection.GetAsyncConnection();

            lCart_Bouquet = new List<Cart_Bouquet>();

            await Update_ListView_Clients();
            await Update_ListView_Bouquets();

            Clear_Control_All();
        }

        /// <summary>
        /// Обновление listview с Клиентами. Каждому клиенту ставится в соответствие его бонусная карта
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Clients()
        {
            List<Client_Card> lClient_Card = new List<Client_Card>();

            List<Clients> lClients = await oClientsRepository.Select_All_Clients_Async();

            foreach (var c in lClients)
            {
                Client_Card client_card = new Client_Card();

                client_card.client = c;
                client_card.card = await conn.GetAsync<Cards>(c.cards_id);

                lClient_Card.Add(client_card);
            }

            listview_Clients.ItemsSource = lClient_Card;
        }

        /// <summary>
        /// Обновление listview с Клиентами с фильтром поисковой строки по ФИО, Телефону или Email. Каждому клиенту ставится в соответствие его бонусная карта
        /// </summary>
        /// <param name="full_name"></param>
        /// <returns></returns>
        private async Task Update_ListView_Clients(string text)
        {
            List<Client_Card> lClient_Card = new List<Client_Card>();

            List<Clients> lClients = await oClientsRepository.Select_All_Clients_Async();

            foreach (var c in lClients)
            {
                Client_Card client_card = new Client_Card();

                if (c.full_name.ToLower().Contains(text.ToLower()) || c.phone_number.ToLower().Contains(text.ToLower()) || c.email.ToLower().Contains(text.ToLower()))
                {
                    client_card.client = c;
                    client_card.card = await conn.GetAsync<Cards>(c.cards_id);

                    lClient_Card.Add(client_card);
                }
            }

            listview_Clients.ItemsSource = lClient_Card;

        }

        /// <summary>
        /// Обновление listview с букетами. Для каждого букета ставится в соответствие список его кмпонентов. Подсчитывается стоимость букета с учётом скидок Специальных Предложений
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Bouquets()
        {
            List<Bouquet_Content> lBouquet_Content = new List<Bouquet_Content>();

            List<Bouquets> lBouquets = await oBouquetsRepository.Select_All_Bouquets_Async();

            foreach (var b in lBouquets)
            {
                Bouquet_Content bouquet_content = new Bouquet_Content();

                bouquet_content.bouquet = b;
                bouquet_content.lContent = await oContentsRepository.Select_Contents_Async("select * from contents where bouquets_id = " + b.bouquets_id);
                bouquet_content.cost = 0;

                foreach (var c in bouquet_content.lContent)
                {
                    if (c.accessories_id != -1)
                    {
                        bouquet_content.cost += (double)c.count * conn.GetAsync<Accessories>(c.accessories_id).Result.price;
                    }
                    else
                    {
                        bouquet_content.cost += (double)c.count * conn.GetAsync<Flowers>(c.flowers_id).Result.price;
                    }
                }

                bouquet_content.cost += bouquet_content.bouquet.price_extra;

                // Обработка скидок по спец. предложениям
                try
                {
                    SpecialDeals oSpecialDeals = (await oSpecialDealsRepository.Select_SpecialDeals_Async("select * from specialdeals where bouquets_id = " + b.bouquets_id))[0];

                    if (oSpecialDeals != null && oSpecialDeals.datetime >= DateTime.Now)
                    {
                        bouquet_content.cost = bouquet_content.cost * (1 - oSpecialDeals.discount / 100);
                    }
                }
                catch
                {

                }

                lBouquet_Content.Add(bouquet_content);
            }

            listview_Bouquets.ItemsSource = lBouquet_Content;
        }

        /// <summary>
        /// Обновление listview с букетами. Для каждого букета ставится в соответствие список его кмпонентов. Подсчитывается стоимость букета с учётом скидок Специальных Предложений
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Bouquets(string name)
        {
            List<Bouquet_Content> lBouquet_Content = new List<Bouquet_Content>();

            List<Bouquets> lBouquets = await oBouquetsRepository.Select_All_Bouquets_Async();

            foreach (var b in lBouquets)
            {
                Bouquet_Content bouquet_content = new Bouquet_Content();

                if (b.name.ToLower().Contains(name.ToLower()))
                {
                    bouquet_content.bouquet = b;
                    bouquet_content.lContent = await oContentsRepository.Select_Contents_Async("select * from contents where bouquets_id = " + b.bouquets_id);
                    bouquet_content.cost = 0;

                    foreach (var c in bouquet_content.lContent)
                    {
                        if (c.accessories_id != -1)
                        {
                            bouquet_content.cost += (double)c.count * conn.GetAsync<Accessories>(c.accessories_id).Result.price;
                        }
                        else
                        {
                            bouquet_content.cost += (double)c.count * conn.GetAsync<Flowers>(c.flowers_id).Result.price;
                        }
                    }

                    bouquet_content.cost += bouquet_content.bouquet.price_extra;

                    // Обработка скидок по спец. предложениям
                    try
                    {
                        SpecialDeals oSpecialDeals = (await oSpecialDealsRepository.Select_SpecialDeals_Async("select * from specialdeals where bouquets_id = " + b.bouquets_id))[0];

                        if (oSpecialDeals != null && oSpecialDeals.datetime >= DateTime.Now)
                        {
                            bouquet_content.cost = bouquet_content.cost * (1 - oSpecialDeals.discount / 100);
                        }
                    }
                    catch
                    {

                    }

                    lBouquet_Content.Add(bouquet_content);
                }

            }

            listview_Bouquets.ItemsSource = lBouquet_Content;
        }

        /// <summary>
        /// Обновление listview с Корзиной ( Покупками ) клиента. Подсчитывается общая стоимость корзины. Расчитывается скидка от общей суммы заказа.
        /// </summary>
        private async void Update_ListView_Carts()
        {
            listview_Carts.ItemsSource = null;
            listview_Carts.ItemsSource = lCart_Bouquet;

            double cost = 0;

            foreach (var c in lCart_Bouquet)
            {
                cost += c.cost;
            }

            label_Cost.Content = "Общая стоимость: " + cost.ToString();

            List<Discounts> lDiscounts = await oDiscountRepository.Select_All_Discounts_Async();

            Discounts oDiscount = new Discounts(0, 0);

            foreach (var d in lDiscounts)
            {
                if (cost > d.value && oDiscount.value < d.value && oDiscount.percent < d.percent) // Находим наибольшую скидку для заказа.
                {
                    oDiscount = d;
                }
            }

            label_Discount.Content = "Скидка:" + oDiscount.percent.ToString() + "%";

            if (oDiscount.percent != 0)
            {
                oCart_Bouquet.cost = cost * (1 - oDiscount.percent / 100);
            }
            else
            {
                oCart_Bouquet.cost = cost;
            }

            label_Total_Cost.Content = "Итого: " + oCart_Bouquet.cost;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        private void Update_ListView_Carts(string name)
        {
            List<Cart_Bouquet> result = new List<Cart_Bouquet>();

            foreach (var b in lCart_Bouquet)
            {
                if (b.bouquet.name.ToLower().Contains(name.ToLower()))
                {
                    result.Add(b);
                }
            }

            listview_Carts.ItemsSource = null;
            listview_Carts.ItemsSource = result;
        }

        /// <summary>
        /// Сброс управления на ноль.
        /// </summary>
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

        /// <summary>
        /// Сброс управления при выбраном клиенте.
        /// </summary>
        private void Clear_Control()
        {
            grid.DataContext = null;

            numeric_Count.IsEnabled = false;

            button_Create.IsEnabled = false;
            button_Delete.IsEnabled = false;

            listview_Bouquets.SelectedIndex = -1;
        }

        /// <summary>
        /// Добавление букета в корзину. Проверка наличия на складе достаточного количества компонентов перед осуществлением добавления. Проверка наличия в корзине букетов такого же типа.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Create_Click(object sender, RoutedEventArgs e)
        {
            // Создаём букет и корзину и заполняем поля данными.
            Cart_Bouquet cart_bouquet = new Cart_Bouquet();

            cart_bouquet.bouquet = oBouquet_Content.bouquet;

            cart_bouquet.cart = new Carts((decimal)numeric_Count.Value, oBouquet_Content.bouquet.bouquets_id);
            cart_bouquet.cost = oBouquet_Content.cost * (double)cart_bouquet.cart.count;

            // Проверка на наличие записи с букетом в корзине
            bool bouquet_found = false;
            int bouquet_i = 0;

            for (int i = 0; i < lCart_Bouquet.Count; i++)
            {
                if (lCart_Bouquet[i].bouquet.bouquets_id == cart_bouquet.bouquet.bouquets_id)
                {
                    bouquet_found = true;
                    bouquet_i = i;

                    cart_bouquet.cost += lCart_Bouquet[i].cost;
                    cart_bouquet.cart.count += lCart_Bouquet[i].cart.count;

                    break;
                }
            }

            // Проверяем на достаточное кол-во компонентов для букета на складе.
            bool Components_Enough = true;

            foreach (var c in oBouquet_Content.lContent)
            {
                if (c.accessories_id != -1)
                {
                    Accessories result = await conn.GetAsync<Accessories>(c.accessories_id);

                    if (result.in_stock < (c.count * cart_bouquet.cart.count))
                    {
                        MessageBox.Show("Недостаточно Аксессуаров \"" + result.name + "\" на складе для составления данного букета!");

                        Components_Enough = false;

                        break;
                    }
                }
                else
                {
                    Flowers result = await conn.GetAsync<Flowers>(c.flowers_id);

                    if (result.in_stock < (c.count * cart_bouquet.cart.count))
                    {
                        MessageBox.Show("Недостаточно Цветов \"" + result.name + "\" на складе для составления данного букета!");

                        Components_Enough = false;

                        break;
                    }
                }
            }

            // Если компонентов достаточно
            if (Components_Enough)
            {
                if (bouquet_found)
                {
                    lCart_Bouquet[bouquet_i] = cart_bouquet;
                }
                else
                {
                    lCart_Bouquet.Add(cart_bouquet);
                }

                Update_ListView_Carts();

                Clear_Control();

                button__Complete.IsEnabled = true;
            }
        }

        /// <summary>
        /// Удаление букета из корзины
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Delete_Click(object sender, RoutedEventArgs e)
        {
            lCart_Bouquet.Remove(oCart_Bouquet);

            Update_ListView_Carts();

            Clear_Control();

            listview_Carts.SelectedIndex = -1;
        }

        /// <summary>
        /// Завершение заказа. Вычитание со склада компонентов, необходимых для создания каждого букета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button__Complete_Click(object sender, RoutedEventArgs e)
        {
            // Составляем соотношение букета и его компонентов.
            foreach (var b in lCart_Bouquet)
            {
                Bouquet_Content current_bouquet = new Bouquet_Content();

                current_bouquet.bouquet = b.bouquet;

                current_bouquet.lContent = await oContentsRepository.Select_Contents_Async("select * from contents where bouquets_id = " + b.bouquet.bouquets_id);

                // Вычитаем со склада компоненты для букета.
                foreach (var c in current_bouquet.lContent)
                {
                    if (c.accessories_id != -1)
                    {
                        Accessories result = await conn.GetAsync<Accessories>(c.accessories_id);

                        for (int i = 0; i < b.cart.count; i++)
                        {
                            result.in_stock -= c.count;
                        }

                        await oAccessoriesRepository.Update_Accessories_Async(result);
                    }
                    else
                    {
                        Flowers result = await conn.GetAsync<Flowers>(c.flowers_id);

                        for (int i = 0; i < b.cart.count; i++)
                        {
                            result.in_stock -= c.count;
                        }

                        await oFlowersRepository.Update_Flowers_Async(result);
                    }
                }
            }

            // Добавляем данные о заказе в БД
            Orders order = new Orders(oClient_Card.client.clients_id, DateTime.Now, textbox_Address.Text, oCart_Bouquet.cost, "В обработке");

            await oOrdersRepository.Insert_Orders_Async(order);

            // Добавляем данные о корзине заказа в БД
            foreach (var c in lCart_Bouquet)
            {
                c.cart.orders_id = order.orders_id;

                await oCartsRepository.Insert_Carts_Async(c.cart);
            }

            Clear_Control_All();

            this.Close();

            System.Windows.Forms.MessageBox.Show("Заказ успешно создан!");
        }

        /// <summary>
        /// Выделение клиента ЛКМ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Clients_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                oClient_Card = (Client_Card)listview_Clients.SelectedItem;

                textbox_Address.IsEnabled = true;

                label_Cost.IsEnabled = true;
                label_Discount.IsEnabled = true;
                label_Total_Cost.IsEnabled = true;

                textbox_Search_Cart.IsEnabled = true;
                textbox_Search_Bouquet.IsEnabled = true;

                listview_Carts.IsEnabled = true;
                listview_Bouquets.IsEnabled = true;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Клиент не выбран!");
            }

        }

        /// <summary>
        /// Сброс выделения с клиента ПКМ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Clients_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Clients.SelectedIndex = -1;

            Clear_Control_All();
        }

        /// <summary>
        /// Выделение букета в listview Корзины ЛКМ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Carts_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                oCart_Bouquet = (Cart_Bouquet)listview_Carts.SelectedItem;

                button_Delete.IsEnabled = true;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Букет не выбран!");
            }

        }

        /// <summary>
        /// Снятие выделения с букета в listview Корзины ПКМ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Carts_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Carts.SelectedIndex = -1;

            button_Delete.IsEnabled = false;
        }

        /// <summary>
        /// Выделение букета в listview Букетов ЛКМ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Bouquets_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                oBouquet_Content = (Bouquet_Content)listview_Bouquets.SelectedItem;

                numeric_Count.IsEnabled = true;
                numeric_Count.Value = 1;

                button_Create.IsEnabled = true;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Букет не выбран!");
            }
        }

        /// <summary>
        /// Снятие выделения с букета в listview Букетов ПКМ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Bouquets_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Bouquets.SelectedIndex = -1;

            Clear_Control();
        }

        /// <summary>
        /// Переход к окну создания нового клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_New_Client_Click(object sender, RoutedEventArgs e)
        {
            NewClientWindow ncw = new NewClientWindow();

            ncw.ShowDialog();
        }


        /// <summary>
        /// Переход к выбору адреса на карте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_choose_adress_Click(object sender, RoutedEventArgs e)
        {
            adress_window = new New_Adress_Window();
            adress_window.Closed += new EventHandler(address_closed);
            adress_window.Show();
        }
        /// <summary>
        /// Событие закрытия формы нахождения адреса на карте (передает адрес на форму заказа)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void address_closed(object sender, EventArgs e)
        {
            textbox_Address.Text = adress_window.NewAddress;
        }

        private async void textbox_Search_Client_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textbox_Search_Client.Text != "")
            {
                await Update_ListView_Clients(textbox_Search_Client.Text);
            }
            else
            {
                await Update_ListView_Clients();
            }

        }

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

        private void textbox_Search_Cart_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textbox_Search_Cart.Text != "")
            {
                Update_ListView_Carts(textbox_Search_Cart.Text);
            }
            else
            {
                Update_ListView_Carts();
            }
        }

        private async void MetroWindow_Activated(object sender, EventArgs e)
        {
            if (!Firts_Start)
            {
                await Update_ListView_Clients();
            }
            else
            {
                Firts_Start = false;
            }
        }
    }
}
