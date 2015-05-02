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

        UsersRepository oWorkersRepository;
        List<Users> workers;
        public bool isLoogedIn;

        private async Task Initialize_Database()
        {
            DBConnection oDBConnection = new DBConnection();
            await oDBConnection.InitializeDatabase();
            oWorkersRepository = new UsersRepository(oDBConnection);
        }

        private async Task get_workers_login()
        {
            workers = await oWorkersRepository.Select_Users_Async("select login,password from Users");
        }

        public async void check_logpas()
        {
            await get_workers_login();
            foreach(var c in workers)
            {
                if(login1.Text == c.login && pass.Password == c.password)
                {
                    isLoogedIn = true;
                    break;
                }
                else
                {
                    isLoogedIn = false;
                }
            }
            if (isLoogedIn)
            {
                Main_menu mm = new Main_menu();
                mm.Show();
                this.Close();
                attention.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                attention.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            check_logpas();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize_Database();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            Registration reg_form = new Registration();
            reg_form.ShowDialog();
        }

       

    }
}
