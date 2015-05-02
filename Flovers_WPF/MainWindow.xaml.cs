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

        UsersRepository oUsersRepository; //Контроллер таблиц "Пользователи"

        public bool Loged; //Проверка пользователя на успешный вход в систему

        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oUsersRepository = new UsersRepository(oDBConnection);
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

            foreach(var c in users)
            {
                if( login == c.login && password == c.password)
                {
                    return true;
                }
            }

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
