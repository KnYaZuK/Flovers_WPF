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
    /// Interaction logic for CreateFlowerWindow.xaml
    /// </summary>
    public partial class CreateFlowerWindow : MetroWindow
    {
        public CreateFlowerWindow()
        {
            InitializeComponent();
        }

        struct Accessories_Flowers
        {
            public Contents contents { get; set; } // Хранит экземпляр выделенного компонента. Необходимо для обновления и удаления.
            public decimal count { get; set; } // Хранит количество компонента в составе

            public string type { get; set; } // Хранит символьное название компонента.
            public object component { get; set; } // Хранит компонент ( Аксессуар или Цветок ).

            public int type_index { get; set; } // Хранит индекс типа компонента в комбобоксе. Необходимо для биндингов.
            public int component_index { get; set; } // Хранит индекс компонента в комбобоксе. Необходимо для биндингов.
        }

        BouquetsRepository oBouquetsRepository;         //
        ContentsRepository oContentsRepository;         // Контроллеры
        AccessoriesRepository oAccessoriesRepository;   // Таблиц
        FlowersRepository oFlowersRepository;           //

        Bouquets bouquet; // Храним выделенный букет.
        object component; // Храним выделенный в комбобоксе аксессуар или цветок.

        Accessories_Flowers accessories_flowers; // Храним данные для вывода их в listview компонентов

        SQLite.SQLiteAsyncConnection conn;


        /// <summary>
        /// Инициализация переменных при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DBConnection oDBConnection = new DBConnection();

            await oDBConnection.InitializeDatabase();

            oBouquetsRepository = new BouquetsRepository(oDBConnection);
            oContentsRepository = new ContentsRepository(oDBConnection);
            oAccessoriesRepository = new AccessoriesRepository(oDBConnection);
            oFlowersRepository = new FlowersRepository(oDBConnection);

            conn = oDBConnection.GetAsyncConnection();


            await Update_ListView_Bouquets();

            Clear_Control_Bouquet();
        }

        /// <summary>
        /// Обновление списка с букетами
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Bouquets()
        {
            List<Bouquets> result = await oBouquetsRepository.Select_All_Bouquets_Async();

            listview_Bouquet.ItemsSource = result;
        }

        /// <summary>
        /// Обновление списка с Компонентами. В список загружаются только компоненты выбранного букета.
        /// </summary>
        /// <returns></returns>
        private async Task Update_ListView_Contents()
        {
            List<Accessories_Flowers> acc_flo = new List<Accessories_Flowers>();

            List<Contents> result = await oContentsRepository.Select_Contents_Async("select * from contents where contents.bouquets_id = " + bouquet.bouquets_id );

            foreach(var c in result)
            {
                Accessories_Flowers af = new Accessories_Flowers();

                af.contents = c; // Необходимо для реализации обновления и удаления
                af.count = c.count; // Необходима для корректного отображения количества.
                
                if ( c.accessories_id != -1 )
                {
                    af.component = await conn.GetAsync<Accessories>(c.accessories_id);
                    af.type = "Аксессуар";
                    af.type_index = 0;

                    af.component_index = await Get_Component_Index( true, (af.component as Accessories).accessories_id );
                }
                else
                {
                    af.component =  conn.GetAsync<Flowers>(c.flowers_id).Result;
                    af.type = "Цветок";
                    af.type_index = 1;

                    af.component_index = await Get_Component_Index( false, (af.component as Flowers).flowers_id);
                }

                acc_flo.Add(af);
            }

            listview_Content.ItemsSource = acc_flo;
        }

        /// <summary>
        /// Функция создаёт соответствие меж компонентом и текущим индексом аналогичного компонента в комбобоксе.
        /// </summary>
        /// <param name="type">true - Аксессуар, false - Цветок</param>
        /// <param name="component_id">id соответствующего Компонента</param>
        /// <returns>Возвращает index компонента в комбобоксе</returns>
        private async Task<int> Get_Component_Index( bool type, int component_id )
        {
            int count = 0;

            if ( type )
            {
                List<Accessories> ores = await oAccessoriesRepository.Select_All_Accessories_Async();

                foreach ( var a in ores )
                {
                    if ( a.accessories_id == component_id )
                    {
                        return count;
                    }

                    count++;
                }
            }
            else
            {
                List<Flowers> result = await oFlowersRepository.Select_All_Flowers_Async();

                foreach (var f in result)
                {
                    if (f.flowers_id == component_id )
                    {
                        return count;
                    }

                    count++;
                }
            }

            return 0;
        }

        /// <summary>
        /// Обновление комбобокса выбора компонента. Зависит от выбранного объекта в комбобоксе типов.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task Update_ComboBox_Contents( bool type )
        {
            if ( type )
            {
                List<Accessories> result = await oAccessoriesRepository.Select_All_Accessories_Async();

                combobox_Content.ItemsSource = result;
            }
            else
            {
                List<Flowers> result = await oFlowersRepository.Select_All_Flowers_Async();

                combobox_Content.ItemsSource = result;
            }
        }

        /// <summary>
        /// Сброс интерфейса сетки букета в состояние по умолчанию.
        /// </summary>
        private void Clear_Control_Bouquet()
        {
            grid_Bouquet.DataContext = null;

            listview_Bouquet.SelectedIndex = -1;
            listview_Content.ItemsSource = null;

            button_CreateBouquet.IsEnabled = true;
            button_UpdateBouquet.IsEnabled = false;

            grid_Content.IsEnabled = false;
        }

        /// <summary>
        /// Сброс интерфейса сетки компонентов в состояние по умолчанию.
        /// </summary>
        private void Clear_Control_Content()
        {
            grid_Content.DataContext = null;

            listview_Content.SelectedIndex = -1;

            combobox_Content.ItemsSource = null;
            combobox_TypeContent.SelectedIndex = -1;

            numericupdown_CountContent.Value = null;

            button_CreateContent.IsEnabled = true;
            button_UpdateContent.IsEnabled = false;
            button_DeleteContent.IsEnabled = false;
        }

        /// <summary>
        /// Создание нового букета.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_CreateBouquet_Click(object sender, RoutedEventArgs e)
        {
            await oBouquetsRepository.Insert_Bouquets_Async(new Bouquets(textbox_NameBouquet.Text, (Double) numeric_Bouquet_CostExtra.Value));


            await Update_ListView_Bouquets();

            Clear_Control_Bouquet();
        }

        /// <summary>
        /// Обновление выбранного букета. Обновляются поля с Именем и Доп.Стоимостью
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_UpdateBouquet_Click(object sender, RoutedEventArgs e)
        {
            bouquet.name = textbox_NameBouquet.Text;
            bouquet.price_extra = (Double) numeric_Bouquet_CostExtra.Value;

            await oBouquetsRepository.Update_Bouquets_Async(bouquet);

            await Update_ListView_Bouquets();

            Clear_Control_Bouquet();
        }

        /// <summary>
        /// Создание компонента для выбранного букета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_CreateContent_Click(object sender, RoutedEventArgs e)
        {
            if ( combobox_TypeContent.SelectedIndex == 0)
            {
                await oContentsRepository.Insert_Contents_Async(new Contents( (decimal) numericupdown_CountContent.Value, bouquet.bouquets_id, -1, (component as Accessories).accessories_id));
            }

            if (combobox_TypeContent.SelectedIndex == 1)
            {
                await oContentsRepository.Insert_Contents_Async(new Contents( (decimal) numericupdown_CountContent.Value, bouquet.bouquets_id, (component as Flowers).flowers_id, -1));
            }

            await Update_ListView_Contents();

            Clear_Control_Content();

        }

        /// <summary>
        /// Обновление выбранного компонента у выбранного букета.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_UpdateContent_Click(object sender, RoutedEventArgs e)
        {

            accessories_flowers.contents.count = (decimal) numericupdown_CountContent.Value;

            if (combobox_TypeContent.SelectedIndex == 0)
            {
                accessories_flowers.contents.accessories_id = (component as Accessories).accessories_id;
                accessories_flowers.contents.flowers_id = -1;
            }

            if (combobox_TypeContent.SelectedIndex == 1)
            {
                accessories_flowers.contents.flowers_id = (component as Flowers).flowers_id;
                accessories_flowers.contents.accessories_id = -1;
            }

            await oContentsRepository.Update_Contents_Async(accessories_flowers.contents);

            await Update_ListView_Contents();

            Clear_Control_Content();
        }

        /// <summary>
        /// Удаление выбранного компонента выбранного букета.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_DeleteContent_Click(object sender, RoutedEventArgs e)
        {
            await oContentsRepository.Delete_Contents_Async(accessories_flowers.contents);

            await Update_ListView_Contents();

            Clear_Control_Content();
        }

        /// <summary>
        /// Выделение букета при нажатии ЛКМ и заполнение текстбоксов значениями этого букета. Запоминает выбранный букет.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listview_Bouquet_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bouquet = listview_Bouquet.SelectedItem as Bouquets;

                grid_Bouquet.DataContext = bouquet;

                button_CreateBouquet.IsEnabled = false;
                button_UpdateBouquet.IsEnabled = true;
                grid_Content.IsEnabled = true;

                await Update_ListView_Contents();

                Clear_Control_Content();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Клиент не выбран!");
            }

        }

        /// <summary>
        /// Снятие выделения с букета при нажатии ПКМ и сброс интерфейса в состояние по умолчанию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Bouquet_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control_Bouquet();
        }

        /// <summary>
        /// Выделение компонента при нажатии ЛКМ и заполнение текстбоксов значениями этого компонента. Запоминает выбранный компонент. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Content_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                accessories_flowers = (Accessories_Flowers)listview_Content.SelectedItem;

                Clear_Control_Content();

                grid_Content.DataContext = accessories_flowers;

                button_CreateContent.IsEnabled = false;
                button_UpdateContent.IsEnabled = true;
                button_DeleteContent.IsEnabled = true;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Букет не выбран!");
            }

        }

        /// <summary>
        /// Снятие выделения с компонента при нажатии ПКМ и сброс интерфейса в состояние по умолчанию.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_Content_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clear_Control_Content();
        }

        /// <summary>
        /// При изменении типа компонента обновляет значения комбобокса с компонентами.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void combobox_TypeContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_TypeContent.SelectedIndex == 0)
            {
                await Update_ComboBox_Contents(true);
            }

            if (combobox_TypeContent.SelectedIndex == 1)
            {
                await Update_ComboBox_Contents(false);
            }
        }

        /// <summary>
        /// Запоминает выбранный в комбобоксе компонент.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combobox_Content_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_TypeContent.SelectedIndex == 0)
            {
                component = combobox_Content.SelectedItem as Accessories;
            }

            if (combobox_TypeContent.SelectedIndex == 1)
            {
                component = combobox_Content.SelectedItem as Flowers;
            }
        }
    }
}
