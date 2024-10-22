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
    /// Interaction logic for Components_Window.xaml
    /// </summary>
    public partial class Components_Window : MetroWindow
    {
        public Components_Window()
        {
            InitializeComponent();
        }

        AccessoriesRepository oAccessoriesRepository; // Контроллер работы с аксессуарами
        FlowersRepository oFlowersRepository; // Контроллер работы с цветами

        Accessories oAccessories; // Хранит в себе выделенный в ListView объект
        Flowers oFlowers; // Хранит в себе выделенный в ListView объект

        /// <summary>
        /// Инициализация контроллеров при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oAccessoriesRepository = new AccessoriesRepository(oDBConnection);
            oFlowersRepository = new FlowersRepository(oDBConnection);

            await Update_ListView();

            Clear_Controls();
        }

        /// <summary>
        /// Обновление содержимого ListView
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView()
        {
            if (combobox_Type.SelectedIndex == 0)
            {
                List<Flowers> result = await oFlowersRepository.Select_All_Flowers_Async();

                listview.ItemsSource = result;
            }
            if (combobox_Type.SelectedIndex == 1)
            {
                List<Accessories> result = await oAccessoriesRepository.Select_All_Accessories_Async();

                listview.ItemsSource = result;
            }
        }

        private async Task Update_ListView(string name)
        {
            listview.ItemsSource = null;

            if (combobox_Type.SelectedIndex == 0)
            {
                List<Flowers> query = await oFlowersRepository.Select_All_Flowers_Async();

                List<Flowers> result = new List<Flowers>();

                foreach ( var f in query )
                {
                    if ( f.name.ToLower().Contains(name.ToLower()))
                    {
                        result.Add(f);
                    }
                }

                listview.ItemsSource = result;
            }
            if (combobox_Type.SelectedIndex == 1)
            {
                List<Accessories> query = await oAccessoriesRepository.Select_All_Accessories_Async();

                List<Accessories> result = new List<Accessories>();

                foreach (var a in query)
                {
                    if (a.name.ToLower().Contains(name.ToLower()))
                    {
                        result.Add(a);
                    }
                }

                listview.ItemsSource = result;
            }
        }

        /// <summary>
        /// Сбрасывает значения textbox и состояние кнопок
        /// </summary>
        private void Clear_Controls()
        {
            grid.DataContext = null;

            textbox_name.Text = "";
            numericupdown_price.Value = null;
            textbox_measure.Text = "";
            numericupdown_count.Value = null;

            button_insert.IsEnabled = true;
            button_update.IsEnabled = false;
            button_delete.IsEnabled = false;
        }

        /// <summary>
        /// Вставка новой записи в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_insert_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_Type.SelectedIndex == 0)
            {
                await oFlowersRepository.Insert_Flowers_Async(new Flowers(textbox_name.Text, (double)numericupdown_price.Value, (decimal)numericupdown_count.Value, textbox_measure.Text));
            }
            if (combobox_Type.SelectedIndex == 1)
            {
                await oAccessoriesRepository.Insert_Accessories_Async(new Accessories(textbox_name.Text, (double)numericupdown_price.Value, (decimal)numericupdown_count.Value, textbox_measure.Text));
            }

            await Update_ListView();

            Clear_Controls();
        }

        /// <summary>
        /// Изменение записи в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_update_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_Type.SelectedIndex == 0)
            {
                oFlowers.name = textbox_name.Text;
                oFlowers.price = (double)numericupdown_price.Value;
                oFlowers.in_stock = (decimal)numericupdown_count.Value;
                oFlowers.measure = textbox_measure.Text;

                await oFlowersRepository.Update_Flowers_Async(oFlowers);
            }
            if (combobox_Type.SelectedIndex == 1)
            {
                oAccessories.name = textbox_name.Text;
                oAccessories.price = (double)numericupdown_price.Value;
                oAccessories.in_stock = (decimal)numericupdown_count.Value;
                oAccessories.measure = textbox_measure.Text;

                await oAccessoriesRepository.Update_Accessories_Async(oAccessories);
            }

            await Update_ListView();

            Clear_Controls();
        }

        /// <summary>
        /// Удаление записи из таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_delete_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_Type.SelectedIndex == 0)
            {
                await oFlowersRepository.Delete_Flowers_Async(oFlowers);
            }
            if (combobox_Type.SelectedIndex == 1)
            {
                await oAccessoriesRepository.Delete_Accessories_Async(oAccessories);
            }

            await Update_ListView();

            Clear_Controls();
        }

        /// <summary>
        /// Выделение записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (combobox_Type.SelectedIndex == 0)
            {
                oFlowers = listview.SelectedItem as Flowers;
                grid.DataContext = oFlowers;
            }
            if (combobox_Type.SelectedIndex == 1)
            {
                oAccessories = listview.SelectedItem as Accessories;
                grid.DataContext = oAccessories;
            }

            button_insert.IsEnabled = false;
            button_update.IsEnabled = true;
            button_delete.IsEnabled = true;
        }

        /// <summary>
        /// Снятие выделения с записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listview.SelectedIndex = -1;

            Clear_Controls();
        }

        /// <summary>
        /// Изменение выделения в ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void combobox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Update_ListView();

            Clear_Controls();
        }

        private async void texbox_Search_Component_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ( texbox_Search_Component.Text != "")
            {
                await Update_ListView(texbox_Search_Component.Text);
            }
            else
            {
                await Update_ListView();
            }
        }

    }
}
