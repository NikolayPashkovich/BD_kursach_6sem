using RestaurantManagementApp.Models;
using RestaurantManagementApp.Services;
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
    /// Логика взаимодействия для AddEditCustomerWindow.xaml
    /// </summary>
    public partial class AddEditCustomerWindow : Window
    {
        public Customer Customer { get; private set; }
        private readonly CustomerService _customerService;
        private bool isEditMode;

        public AddEditCustomerWindow()
        {
            InitializeComponent();
            Customer = new Customer();
            _customerService = new CustomerService(new RestaurantPlazaKursachContext()); // Инициализация сервиса
            this.DataContext = Customer;
            isEditMode = false;
        }

        public AddEditCustomerWindow(Customer existingCustomer)
        {
            InitializeComponent();
            _customerService = new CustomerService(new RestaurantPlazaKursachContext()); // Инициализация сервиса

            Customer = new Customer
            {
                CustomerId = existingCustomer.CustomerId,
                FirstName = existingCustomer.FirstName,
                LastName = existingCustomer.LastName,
                Phone = existingCustomer.Phone
            };
            this.DataContext = Customer;
            FirstNameBox.Text = Customer.FirstName;
            LastNameBox.Text = Customer.LastName;
            PhoneBox.Text = Customer.Phone;
            isEditMode = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();
            string phone = PhoneBox.Text.Trim();
            // Простая валидация
            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Имя не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Фамилия не может быть пустой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(phone, @"^\+?\d{10,15}$"))
            {
                MessageBox.Show("Введите корректный номер телефона (только цифры, можно с +, от 10 до 15 цифр).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Проверка на уникальность номера телефона
            if (_customerService.IsPhoneDuplicate(phone, Customer?.CustomerId))
            {
                MessageBox.Show("Этот номер телефона уже используется другим клиентом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Customer.FirstName = firstName;
            Customer.LastName = lastName;
            Customer.Phone = phone;

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
