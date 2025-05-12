using RestaurantManagementApp.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantManagementApp.windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditMenuWindow.xaml
    /// </summary>
    public partial class AddEditMenuWindow : Window
    {
        public Models.Menu Menu { get; private set; }

        public AddEditMenuWindow(Models.Menu? menu = null)
        {
            InitializeComponent();

            Menu = menu != null ? new Models.Menu
            {
                MenuId = menu.MenuId,
                MenuName = menu.MenuName,
                StartDate = menu.StartDate
            } : new Models.Menu { StartDate = DateTime.Now };

            MenuNameBox.Text = Menu.MenuName;
            StartDatePicker.SelectedDate = Menu.StartDate;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MenuNameBox.Text))
            {
                MessageBox.Show("Название не может быть пустым.");
                return;
            }

            if (StartDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату начала.");
                return;
            }

            Menu.MenuName = MenuNameBox.Text.Trim();
            Menu.StartDate = StartDatePicker.SelectedDate.Value;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
