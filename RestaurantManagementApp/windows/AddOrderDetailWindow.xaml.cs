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
    /// Логика взаимодействия для AddOrderDetailWindow.xaml
    /// </summary>
    public partial class AddOrderDetailWindow : Window
    {
        // Результат для передачи в окно заказа
        public OrderDetailDto ResultDetail { get; set; } = new OrderDetailDto();

        public AddOrderDetailWindow()
        {
            InitializeComponent();
            LoadPromotions();
            PaymentMethodComboBox.SelectedIndex = 0;
            PaymentDatePicker.SelectedDate = DateTime.Now;
        }
        public AddOrderDetailWindow(OrderDetailDto existingDetail) : this()
        {
            ResultDetail = new OrderDetailDto
            {
                DishID = existingDetail.DishID,
                PromotionID = existingDetail.PromotionID,
                Quantity = existingDetail.Quantity,
                PaymentMethod = existingDetail.PaymentMethod,
                PaymentDate = existingDetail.PaymentDate,
                DishName = existingDetail.DishName,
                PromotionName = existingDetail.PromotionName,
                PriceAtOrder = existingDetail.PriceAtOrder
            };

            // Заполняем UI
            DishTextBox.Text = existingDetail.DishName;
            QuantityTextBox.Text = existingDetail.Quantity.ToString();
            PaymentDatePicker.SelectedDate = existingDetail.PaymentDate;

            // Устанавливаем метод оплаты
            foreach (ComboBoxItem item in PaymentMethodComboBox.Items)
            {
                if (item.Content.ToString() == existingDetail.PaymentMethod)
                {
                    PaymentMethodComboBox.SelectedItem = item;
                    break;
                }
            }

            // Устанавливаем акцию
            Loaded += (s, e) =>
            {
                if (PromotionComboBox.ItemsSource is IEnumerable<Promotion> promotions)
                {
                    var promo = promotions.FirstOrDefault(p => p.PromotionId == existingDetail.PromotionID);
                    if (promo != null)
                    {
                        PromotionComboBox.SelectedItem = promo;
                    }
                }
            };
        }


        private void LoadPromotions()
        {
            using (var context = new RestaurantPlazaKursachContext())
            {
                var promotions = context.Promotions.ToList();
                PromotionComboBox.ItemsSource = promotions;
            }
        }

        // Выбор блюда – открываем окно выбора блюда (можно реализовать по аналогии)
        private void SelectDish_Click(object sender, RoutedEventArgs e)
        {
            // Допустим, у нас есть окно выбора блюда
            var dishSelectWindow = new DishSelectWindow();
            if (dishSelectWindow.ShowDialog() == true)
            {
                var selectedDish = dishSelectWindow.SelectedDish;
                if (selectedDish != null)
                {
                    ResultDetail.DishID = selectedDish.DishId;
                    ResultDetail.DishName = selectedDish.DishName;
                    // Если цена не редактируется – её не передаём (триггер посчитает)
                    DishTextBox.Text = selectedDish.DishName;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }
            ResultDetail.Quantity = quantity;
            // Проверка на выбранное блюдо
            if (ResultDetail.DishID == 0)
            {
                MessageBox.Show("Пожалуйста, выберите блюдо.");
                return;
            }
            if (PaymentMethodComboBox.SelectedItem is ComboBoxItem item)
            {
                ResultDetail.PaymentMethod = item.Content.ToString();
            }
            else
            {
                ResultDetail.PaymentMethod = "Cash";
            }
            ResultDetail.PaymentDate = PaymentDatePicker.SelectedDate ?? DateTime.Now;

            if (PromotionComboBox.SelectedItem is Promotion promotion)
            {
                ResultDetail.PromotionID = promotion.PromotionId;
                ResultDetail.PromotionName = promotion.PromotionName;
            }
            else
            {
                ResultDetail.PromotionID = null;
                ResultDetail.PromotionName = string.Empty;
            }

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
