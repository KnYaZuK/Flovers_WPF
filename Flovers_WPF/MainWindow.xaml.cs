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
using MahApps.Metro.Controls;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

<<<<<<< HEAD
        UsersRepository oUsersRepository; //Контроллер таблиц "Пользователи"
=======
        struct ClientCard
        {
            public Clients client { get; set;}
            public Cards card { get; set; }
        }

        ClientsRepository oClientsRepository; //Объект для работы с таблицей Клиентов
        CardsRepository oCardsRepository;
>>>>>>> origin/KnYaZuK_Laptop

        public bool Loged; //Проверка пользователя на успешный вход в систему

<<<<<<< HEAD
=======
        DBConnection oDBConnection;

        /// <summary>
        /// Устанавливает связь с базой данных.
        /// </summary>
        /// <returns></returns>
>>>>>>> origin/KnYaZuK_Laptop
        private async Task Initialize_Database()
        {
            oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

<<<<<<< HEAD
            oUsersRepository = new UsersRepository(oDBConnection);
=======
            oClientsRepository = new ClientsRepository(oDBConnection);
            oCardsRepository = new CardsRepository(oDBConnection);
        }
        /// <summary>
        /// Создание новой записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Create_Click(object sender, RoutedEventArgs e)
        {
            Cards card = new Cards();
            await oCardsRepository.Insert_Cards_Async(card);

            Clients client = new Clients(textbox_full_name.Text,textbox_phone_number.Text,textbox_email.Text,textbox_referer_number.Text, card.cards_id );
            await oClientsRepository.Insert_Clients_Async(client);
            

            await Update_Grid_View();
            await Update_ListBox_View();

            Clear_Controls();
>>>>>>> origin/KnYaZuK_Laptop
        }
        /// <summary>
        /// Метод проверки корректности введённых данных
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>Возвращает true или false</returns>
        public async Task<bool> Check_Login( string login, string password )
        {
            List<Users> users = await oUsersRepository.Select_All_Users_Async();

<<<<<<< HEAD
            foreach(var c in users)
            {
                if( login == c.login && password == c.password)
                {
                    return true;
                }
            }
=======
            datagridview.ItemsSource = result;
        }

        private async Task Update_ListBox_View()
        {
            List<ClientCard> clientcard = new List<ClientCard>();

            List<Clients> result = await oClientsRepository.Select_All_Clients_Async();

            SQLite.SQLiteAsyncConnection conn = oDBConnection.GetAsyncConnection();

            foreach( Clients client in result   )
            {
                ClientCard cc = new ClientCard();
                cc.client = client;
                cc.card = await conn.GetAsync<Cards>(client.cards_id);
                clientcard.Add(cc);
            }

            listview.ItemsSource = clientcard;
        }

        /// <summary>
        /// Инициализация базы данных и обновление GridView при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();
>>>>>>> origin/KnYaZuK_Laptop

            return false;
        }
        /// <summary>
        /// Вызов метода входа пользователя в систему
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void login_Click(object sender, RoutedEventArgs e)
        {
            if (await Check_Login(textbox_Login.Text, passwordbox_password.Password))
            {
                MainMenuWindow mm = new MainMenuWindow();
                mm.Show();
                this.Close();
            }
            else
            {
                label_attention.Visibility = System.Windows.Visibility.Visible;
            }
<<<<<<< HEAD
=======

            ClientCard cc;
            cc = (ClientCard) e.AddedItems[0];
            oClients = cc.client;

            spanel_Clients.DataContext = oClients;

            button_Create.IsEnabled = false;
            button_Update.IsEnabled = true;
            button_Delete.IsEnabled = true;
>>>>>>> origin/KnYaZuK_Laptop
        }
        /// <summary>
        /// Вызов окна с регистрацией нового пользователя программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void register_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow reg_form = new RegistrationWindow();

            reg_form.ShowDialog();
        }
        /// <summary>
        /// Выполнение операций при загрузке окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();
        }
    }
}
