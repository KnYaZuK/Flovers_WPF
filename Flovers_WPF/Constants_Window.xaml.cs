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
    /// Interaction logic for Constants_Window.xaml
    /// </summary>
    public partial class Constants_Window : MetroWindow
    {
        public Constants_Window()
        {
            InitializeComponent();
        }

        ConstantsRepository oConstantsRepository;

        Constants oConstants;
        
        /// <summary>
        /// Загрузка окна, инициализация контроллеров и БД, обновление listview, сброс управления.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Const_Win_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oConstantsRepository = new ConstantsRepository(oDBConnection);

            await Check_Address_and_Phone();

            await Update_ListView_Constants();
        }

        /// <summary>
        /// Проверка на наличие в БД записей с Адресом и Номером Телефона, если записей нет - добавление в БД данных записей.
        /// </summary>
        private async Task Check_Address_and_Phone()
        {
            List<Constants> lConstants = await oConstantsRepository.Select_Constants_Async("select * from constants where name = 'Address' or name = 'Phone_Number'");

            bool Address = true;
            bool Phone_Number = true;
            
            foreach ( var c in lConstants )
            {
                if(c.name == "Address")
                {
                    Address = false;
                }

                if (c.name == "Phone_Number")
                {
                    Phone_Number = false;
                }
            }

            if ( Address )
            {
                await oConstantsRepository.Insert_Constants_Async(new Constants("Address","Value"));
            }

            if (Phone_Number)
            {
                await oConstantsRepository.Insert_Constants_Async(new Constants("Phone_Number", "Phone_Number"));
            }
        }

        /// <summary>
        /// Обновление listview элементами из БД таблицы Констант
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Constants()
        {
            listview_const.ItemsSource = await oConstantsRepository.Select_All_Constants_Async();

            Clear_Control();
        }

        /// <summary>
        /// Сброс управления
        /// </summary>
        private void Clear_Control()
        {
            oConstants = null;

            textbox_Name.IsEnabled = true;

            button_Create.IsEnabled = true;
            button_Update.IsEnabled = false;
            button_Delete.IsEnabled = false;

            textbox_Name.Text = "";
            textbox_Value.Text = "";

            listview_const.SelectedIndex = -1;
        }

        /// <summary>
        /// Создане новой записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Create_Click(object sender, RoutedEventArgs e)
        {
            await oConstantsRepository.Insert_Constants_Async(new Constants(textbox_Name.Text, textbox_Value.Text));

            await Update_ListView_Constants();
        }

        /// <summary>
        /// Изменение выделенной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Update_Click(object sender, RoutedEventArgs e)
        {
            oConstants.name = textbox_Name.Text;
            oConstants.value = textbox_Value.Text;

            await oConstantsRepository.Update_Constants_Async(oConstants);

            await Update_ListView_Constants();
        }

        /// <summary>
        /// Удаление выделенной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_Delete_Click(object sender, RoutedEventArgs e)
        {
            await oConstantsRepository.Delete_Constants_Async(oConstants);

            await Update_ListView_Constants();
        }

        /// <summary>
        /// Выделение элемента в listview Констант ЛКМ и хранение выделенной константы. Если выбранная константа имела название Address или Phone_Number, то её можно только редактировать.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_const_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                oConstants = (Constants)listview_const.SelectedItem;

                grid.DataContext = oConstants;

                if ((oConstants.name == "Address") || (oConstants.name == "Phone_Number"))
                {
                    button_Create.IsEnabled = false;
                    button_Update.IsEnabled = true;
                    button_Delete.IsEnabled = false;

                    textbox_Name.IsEnabled = false;
                }
                else
                {
                    button_Create.IsEnabled = false;
                    button_Update.IsEnabled = true;
                    button_Delete.IsEnabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Элемент не выбран");
            }
        }

        /// <summary>
        /// Снятие выделения с элемента в listview Констант ПКМ.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_const_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control();
        }
    }
}
