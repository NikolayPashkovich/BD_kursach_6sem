using RestaurantManagementApp.Models;
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

namespace RestaurantManagementApp.windows
{
    /// <summary>
    /// Логика взаимодействия для DishSelectWindow.xaml
    /// </summary>
    public partial class DishSelectWindow : Window
    {
        public Dish SelectedDish { get; set; }

        public DishSelectWindow()
        {
            InitializeComponent();
            LoadDishes();
        }

        private void LoadDishes()
        {
            using (var context = new RestaurantPlazaKursachContext())
            {
                // Фильтруем только активные блюда
                var dishes = context.Dishes.Where(d => d.IsActive == true).ToList();
                DishesGrid.ItemsSource = dishes;
            }
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (DishesGrid.SelectedItem is Dish dish)
            {
                SelectedDish = dish;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите блюдо.", "Выбор блюда", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
