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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : MetroWindow
    {
        UsersRepository oUsersRepository;
        public bool isRegistered;

        public Registration()
        {
            InitializeComponent();
        }

        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();
            await oDBConnection.InitializeDatabase();
            oUsersRepository = new UsersRepository(oDBConnection);
        }

        public async void registration()
        {
            await oUsersRepository.Insert_Users_Async(new Users(lb_reg_login.Text,lb_reg_pass.Password));
            MessageBox.Show("Вы зарегистрированы!");
        }

        private void bt_register_Click(object sender, RoutedEventArgs e)
        {
            if (lb_reg_login.Text == "" || lb_reg_pass.Password == "")
            {
                lb_alert.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lb_alert.Visibility = System.Windows.Visibility.Hidden;
                registration();
            }
        }

        private async void reg_form_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();
        }

    }
}
