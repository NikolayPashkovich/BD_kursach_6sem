using RestaurantManagementApp.Models;
using RestaurantManagementApp.Services;
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
    /// Логика взаимодействия для SupplyWindow.xaml
    /// </summary>
    public partial class SupplyWindow : Window
    {
        public Supply SelectedSupply { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public SupplyWindow(Supply supply = null)
        {
            InitializeComponent();
            LoadData();

            if (supply != null)
            {
                SelectedSupply = supply;
                SupplierComboBox.SelectedItem = Suppliers.FirstOrDefault(s => s.SupplierId == supply.SupplierId);
                IngredientComboBox.SelectedItem = Ingredients.FirstOrDefault(i => i.IngredientId == supply.IngredientId);
                SupplyDatePicker.SelectedDate = supply.SupplyDate;
                QuantityTextBox.Text = supply.Quantity.ToString();
            }
        }

        private void LoadData()
        {
            using var context = new RestaurantPlazaKursachContext();
            Suppliers = context.Suppliers.ToList();
            Ingredients = context.Ingredients.ToList();

            SupplierComboBox.ItemsSource = Suppliers;
            SupplierComboBox.DisplayMemberPath = "SupplierName";

            IngredientComboBox.ItemsSource = Ingredients;
            IngredientComboBox.DisplayMemberPath = "IngredientName";
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (SupplierComboBox.SelectedItem == null || IngredientComboBox.SelectedItem == null ||
                !decimal.TryParse(QuantityTextBox.Text, out var quantity) || quantity <= 0 ||
                SupplyDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            var supplierId = ((Supplier)SupplierComboBox.SelectedItem).SupplierId;
            var ingredientId = ((Ingredient)IngredientComboBox.SelectedItem).IngredientId;
            var date = SupplyDatePicker.SelectedDate.Value;

            if (SelectedSupply == null)
            {
                await SupplyService.AddSupplyAsync(supplierId, ingredientId, date, quantity);
            }
            else
            {
                await SupplyService.UpdateSupplyAsync(SelectedSupply.SupplyId, supplierId, ingredientId, date, quantity);
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

}
