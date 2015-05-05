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
    /// Interaction logic for NewClientWindow.xaml
    /// </summary>
    public partial class NewClientWindow : MetroWindow
    {
        public NewClientWindow()
        {
            InitializeComponent();
        }

        struct Clients_Cards
        {
            public Clients client { get; set; }
            public Cards card { get; set; }
        }

        ClientsRepository oClientsRepository;
        CardsRepository oCardsRepository;

        Clients_Cards cc;

        SQLite.SQLiteAsyncConnection conn;

        /// <summary>
        /// Загрузка окна и инициализация переменных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oClientsRepository = new ClientsRepository(oDBConnection);
            oCardsRepository = new CardsRepository(oDBConnection);

            conn = oDBConnection.GetAsyncConnection();

            await Update_ListView();

            Clear_Controls();
        }

        /// <summary>
        /// Обновление содержимого ListView
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView()
        {
            List<Clients_Cards> clientcard = new List<Clients_Cards>();

            List<Clients> result = await oClientsRepository.Select_All_Clients_Async(); 

            foreach (Clients client in result)
            {
                Clients_Cards cc = new Clients_Cards();
                cc.client = client;
                cc.card = await conn.GetAsync<Cards>(client.cards_id);
                clientcard.Add(cc);
            }

            listview_Clients_Cards.ItemsSource = clientcard;
        }

        /// <summary>
        /// Сбрасывает значения textbox и состояние кнопок
        /// </summary>
        private void Clear_Controls()
        {
            grid.DataContext = null;

            button_Create.IsEnabled = true;
            button_Update.IsEnabled = false;
            tb_referer_.IsEnabled = true;
        }

        /// <summary>
        /// Нажатие клавиши регистрации клиента. Создаёт в таблице запись Cards и Clients. Связывает записи Cards и Clients.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Create_Click(object sender, RoutedEventArgs e)
        {
            Cards card = new Cards();
            await oCardsRepository.Insert_Cards_Async(card);

            Clients client = new Clients(tb_name.Text, tb_phone.Text, tb_email.Text, tb_referer_.Text, card.cards_id);
            await oClientsRepository.Insert_Clients_Async(client);

            await Update_ListView();

            Clear_Controls();
        }

        /// <summary>
        /// Нажатие клавиши обновления данных клиента. Обновляет только поля с ФИО, Телефоном и Email.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Update_Click(object sender, RoutedEventArgs e)
        {
            cc.client.full_name = tb_name.Text;
            cc.client.phone_number = tb_phone.Text;
            cc.client.email = tb_email.Text;

            await oClientsRepository.Update_Clients_Async(cc.client);
        }

        /// <summary>
        /// Сохраняет в памяти данные выделенной записи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems.Count <= 0)
        //    {
        //        return;
        //    }

        //    cc = (Clients_Cards)e.AddedItems[0];

        //    grid.DataContext = cc;

        //    button_Create.IsEnabled = false;
        //    button_Update.IsEnabled = true;
        //}

        /// <summary>
        /// Очищает запись от выделения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview_Clients_Cards.SelectedIndex = -1;
            Clear_Controls();
        }

        /// <summary>
        /// Сохраняет в памяти данные выделенной записи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cc = (Clients_Cards) listview_Clients_Cards.SelectedItem;

            grid.DataContext = cc.client;

            button_Create.IsEnabled = false;
            button_Update.IsEnabled = true;
            tb_referer_.IsEnabled = false;
        }
    }
}
