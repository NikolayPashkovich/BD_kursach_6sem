using RestaurantManagementApp.Models;
using RestaurantManagementApp.Services;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace RestaurantManagementApp.windows
{
    public partial class AddEditIngredientWindow : Window
    {
        public Ingredient Ingredient { get; private set; }
        private readonly IngredientService _service;
        private bool _isEdit;

        public AddEditIngredientWindow()
        {
            InitializeComponent();
            Ingredient = new Ingredient();
            _service = new IngredientService(new RestaurantPlazaKursachContext());
        }

        public AddEditIngredientWindow(Ingredient ingredient)
        {
            InitializeComponent();
            _service = new IngredientService(new RestaurantPlazaKursachContext());

            Ingredient = new Ingredient
            {
                IngredientId = ingredient.IngredientId,
                IngredientName = ingredient.IngredientName,
                Unit = ingredient.Unit,
                CurrentStock = ingredient.CurrentStock
            };

            NameBox.Text = Ingredient.IngredientName;
            UnitBox.Text = Ingredient.Unit;
            StockBox.Text = Ingredient.CurrentStock?.ToString();

            _isEdit = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text.Trim();
            var unit = UnitBox.Text.Trim();
            var stockText = StockBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Название не может быть пустым.", "Ошибка");
                return;
            }

            if (_service.IsNameDuplicate(name, _isEdit ? Ingredient.IngredientId : null))
            {
                MessageBox.Show("Ингредиент с таким названием уже существует.", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(unit))
            {
                MessageBox.Show("Единица измерения обязательна.", "Ошибка");
                return;
            }

            if (!decimal.TryParse(stockText, out decimal stock) || stock < 0)
            {
                MessageBox.Show("Остаток должен быть положительным числом.", "Ошибка");
                return;
            }

            Ingredient.IngredientName = name;
            Ingredient.Unit = unit;
            Ingredient.CurrentStock = stock;

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
