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
using MahApps.Metro.Controls;
using Flovers_WPF.Repository;
using Flovers_WPF.DataAccess;
using Flovers_WPF.DataModel;

namespace Flovers_WPF
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : MetroWindow
    {
        UsersRepository oUsersRepository;

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oUsersRepository = new UsersRepository(oDBConnection);
        }

        private async void reg_form_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();
        }
        /// <summary>
        /// регистрация нового пользователя в системе
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public async void Registration(string login, string password)
        {
            await oUsersRepository.Insert_Users_Async(new Users( login, password ));

            MessageBox.Show("Вы зарегистрированы!");

            this.Close();
        }
        /// <summary>
        /// Проверка на уникальность имени пользователя системы
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns>true, если такого логина нет или false, если такое имя существует</returns>
        public async Task<bool> Check_Login( string login )
        {
            List<Users> users = await oUsersRepository.Select_All_Users_Async();

            foreach( var u in users )
            {
                if ( u.login == login )
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Registration_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_Login.Text == "" || passwordbox_Password.Password == "")
            {
                label_Allert.Content = "Заполнены не все поля.";
                label_Allert.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                if ( await Check_Login(textbox_Login.Text ))
                {
                    label_Allert.Visibility = System.Windows.Visibility.Hidden;

                    Registration(textbox_Login.Text, passwordbox_Password.Password);

                    this.Close();
                }
                else
                {
                    label_Allert.Content = "Такое имя пользователя уже существует.";
                    label_Allert.Visibility = System.Windows.Visibility.Visible;
                }
                
            }
        }


    }
}
