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
    /// Логика взаимодействия для AddEditDishIngredientWindow.xaml
    /// </summary>
    public partial class AddEditDishIngredientWindow : Window
    {
        public DishIngredient DishIngredient { get; private set; } = new();
        public List<Dish> Dishes { get; set; } = new();
        public List<Ingredient> Ingredients { get; set; } = new();
        public bool IsEditMode { get; set; }

        public AddEditDishIngredientWindow(List<Dish> dishes, List<Ingredient> ingredients, DishIngredient? existing = null)
        {
            InitializeComponent();

            Dishes = dishes;
            Ingredients = ingredients;

            DishComboBox.ItemsSource = Dishes;
            IngredientComboBox.ItemsSource = Ingredients;

            if (existing != null)
            {
                DishIngredient = existing;
                DishComboBox.SelectedValue = existing.DishId;
                IngredientComboBox.SelectedValue = existing.IngredientId;
                QuantityTextBox.Text = existing.Quantity.ToString();
                IsEditMode = true;

                DishComboBox.IsEnabled = false;
                IngredientComboBox.IsEnabled = false;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DishComboBox.SelectedValue == null || IngredientComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите блюдо и ингредиент");
                return;
            }

            if (!decimal.TryParse(QuantityTextBox.Text, out decimal quantity))
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            if (quantity < 0.1m || quantity > 5000)
            {
                MessageBox.Show("Количество должно быть от 0.1 до 5000");
                return;
            }

            DishIngredient.DishId = (int)DishComboBox.SelectedValue;
            DishIngredient.IngredientId = (int)IngredientComboBox.SelectedValue;
            DishIngredient.Quantity = quantity;

            DialogResult = true;
        }
    }
}
