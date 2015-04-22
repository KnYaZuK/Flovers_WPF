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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;
using Flovers_WPF.Repository;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ClientsRepository oClientsRepository; //Объект для работы с таблицей Клиентов

        Clients oClients; //Текущий выделенный клиент

        /// <summary>
        /// Устанавливает связь с базой данных.
        /// </summary>
        /// <returns></returns>
        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();
            await oDBConnection.InitializeDatabase();
            oClientsRepository = new ClientsRepository(oDBConnection);
        }
        /// <summary>
        /// Создание новой записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Create_Click(object sender, RoutedEventArgs e)
        {
            await oClientsRepository.Insert_Clients_Async(new Clients() { full_name = textbox_full_name.Text, phone_number = textbox_phone_number.Text, email = textbox_email.Text });
            await Update_Grid_View();

            Clear_Controls();
        }
        /// <summary>
        /// Изменение выделенной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Update_Click(object sender, RoutedEventArgs e)
        {
            Clear_Controls();
        }

        /// <summary>
        /// Удаление выделенной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Delete_Click(object sender, RoutedEventArgs e)
        {
            await oClientsRepository.Delete_Clients_Async(oClients);
            await Update_Grid_View();

            Clear_Controls();
        }

        /// <summary>
        /// Обновление содержимого GridView
        /// </summary>
        /// <returns></returns>
        private async Task Update_Grid_View()
        {
            List<Clients> result = await oClientsRepository.Select_All_Clients_Async();

            gridview.ItemsSource = result;
        }

        /// <summary>
        /// Инициализация базы данных и обновление GridView при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();
            await Update_Grid_View();
        }

        /// <summary>
        /// Хранит в себе выделенный элемент GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }

            oClients = e.AddedItems[0] as Clients;

            button_Create.IsEnabled = false;
            button_Update.IsEnabled = true;
            button_Delete.IsEnabled = true;
        }

        /// <summary>
        /// Присвоить стандартные значения текст-боксам
        /// </summary>
        private void Clear_Controls()
        {
            textbox_full_name.Text = "ФИО";
            textbox_phone_number.Text = "Номер Телефона";
            textbox_email.Text = "Email";

            button_Create.IsEnabled = true;
            button_Update.IsEnabled = false;
            button_Delete.IsEnabled = false;
        }
    }
}
