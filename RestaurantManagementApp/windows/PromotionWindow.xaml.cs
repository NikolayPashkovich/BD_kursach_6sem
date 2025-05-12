using RestaurantManagementApp.Models;
using System.Windows;

namespace RestaurantManagementApp.windows
{
    /// <summary>
    /// Логика взаимодействия для PromotionWindow.xaml
    /// </summary>
    public partial class PromotionWindow : Window
    {
        public Promotion Promotion { get; private set; }

        public PromotionWindow(Promotion? existing = null)
        {
            InitializeComponent();

            if (existing != null)
            {
                Promotion = new Promotion
                {
                    PromotionId = existing.PromotionId,
                    PromotionName = existing.PromotionName,
                    DiscountPercent = existing.DiscountPercent,
                    StartDate = existing.StartDate,
                    EndDate = existing.EndDate
                };

                NameBox.Text = Promotion.PromotionName;
                DiscountBox.Text = Promotion.DiscountPercent.ToString();
                StartDatePicker.SelectedDate = Promotion.StartDate;
                EndDatePicker.SelectedDate = Promotion.EndDate;
            }
            else
            {
                Promotion = new Promotion();
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                !decimal.TryParse(DiscountBox.Text, out decimal discount) ||
                StartDatePicker.SelectedDate == null ||
                EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            if (discount <= 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100%.");
                return;
            }

            if (EndDatePicker.SelectedDate < StartDatePicker.SelectedDate)
            {
                MessageBox.Show("Дата окончания не может быть раньше даты начала.");
                return;
            }

            Promotion.PromotionName = NameBox.Text.Trim();
            Promotion.DiscountPercent = discount;
            Promotion.StartDate = StartDatePicker.SelectedDate.Value;
            Promotion.EndDate = EndDatePicker.SelectedDate.Value;

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

}
