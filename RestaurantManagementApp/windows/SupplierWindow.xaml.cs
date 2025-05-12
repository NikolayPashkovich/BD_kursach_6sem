using RestaurantManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для SupplierWindow.xaml
    /// </summary>
    public partial class SupplierWindow : Window
    {
        public Supplier Supplier { get; private set; }

        public SupplierWindow(Supplier supplier = null)
        {
            InitializeComponent();
            Supplier = supplier ?? new Supplier();

            if (supplier != null)
            {
                SupplierNameTextBox.Text = supplier.SupplierName;
                ContactPhoneTextBox.Text = supplier.ContactPhone;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = SupplierNameTextBox.Text.Trim();
            string phone = ContactPhoneTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Название обязательно.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\+?\d{10,15}$"))
            {
                MessageBox.Show("Введите корректный номер телефона.");
                return;
            }

            Supplier.SupplierName = name;
            Supplier.ContactPhone = phone;

            DialogResult = true;
        }
    }

}
