using RestaurantManagementApp.Models;
using RestaurantManagementApp.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantManagementApp.windows
{
    public partial class AddEditDishWindow : Window
    {
        public Dish Dish { get; private set; }
        private readonly DishService _dishService;
        private readonly bool isEditMode;

        public AddEditDishWindow()
        {
            InitializeComponent();
            Dish = new Dish();
            _dishService = new DishService(new RestaurantPlazaKursachContext());
        }

        public AddEditDishWindow(Dish dish) : this()
        {
            isEditMode = true;
            Dish = new Dish
            {
                DishId = dish.DishId,
                DishName = dish.DishName,
                Price = dish.Price,
                Category = dish.Category,
                IsActive = dish.IsActive
            };

            DishNameBox.Text = Dish.DishName;
            PriceBox.Text = Dish.Price.ToString();
            CategoryBox.Text = Dish.Category;
            IsActiveBox.IsChecked = Dish.IsActive ?? true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = DishNameBox.Text.Trim();
            string priceText = PriceBox.Text.Trim();
            string category = (CategoryBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            bool isActive = IsActiveBox.IsChecked == true;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Название не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (> 0).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_dishService.IsDishNameDuplicate(name, isEditMode ? Dish.DishId : null))
            {
                MessageBox.Show("Блюдо с таким названием уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Dish.DishName = name;
            Dish.Price = price;
            Dish.Category = category;
            Dish.IsActive = isActive;

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
