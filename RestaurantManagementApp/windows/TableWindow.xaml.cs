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
    /// Логика взаимодействия для TableWindow.xaml
    /// </summary>
    public partial class TableWindow : Window
    {
        private int? _tableId;

        public TableWindow()
        {
            InitializeComponent();
        }

        public TableWindow(Models.Table table) : this()
        {
            _tableId = table.TableId;
            TableNumberTextBox.Text = table.TableNumber;
            CapacityTextBox.Text = table.Capacity.ToString();
            IsAvailableCheckBox.IsChecked = table.IsAvailable;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            var tableNumber = TableNumberTextBox.Text.Trim();
            if (string.IsNullOrEmpty(tableNumber))
            {
                MessageBox.Show("Номер стола не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (await IsTableNumberExist(tableNumber, _tableId))
            {
                MessageBox.Show("Номер стола должен быть уникальным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(CapacityTextBox.Text, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Количество мест должно быть больше нуля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var isAvailable = IsAvailableCheckBox.IsChecked ?? true;

            try
            {
                if (_tableId == null) // Добавление нового стола
                {
                    await TableService.AddTableAsync(tableNumber, capacity, isAvailable);
                }
                else // Обновление существующего стола
                {
                    await TableService.UpdateTableAsync(_tableId.Value, tableNumber, capacity, isAvailable);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> IsTableNumberExist(string tableNumber, int? excludeTableId)
        {
            var tables = await TableService.GetAllTablesAsync();
            return tables.Any(t => t.TableNumber == tableNumber && t.TableId != excludeTableId);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }


}
