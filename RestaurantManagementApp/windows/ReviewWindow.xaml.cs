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
    /// Логика взаимодействия для ReviewWindow.xaml
    /// </summary>
    public partial class ReviewWindow : Window
    {
        public int OrderId { get; }
        public int? CurrentRating { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        public ReviewWindow(int orderId, int? existingRating = null)
        {
            InitializeComponent();
            OrderId = orderId;
            CurrentRating = existingRating;
            if (existingRating.HasValue)
                RatingTextBox.Text = existingRating.Value.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(RatingTextBox.Text, out int rating) || rating < 1 || rating > 5)
            {
                MessageBox.Show("Введите оценку от 1 до 5.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentRating = rating;
            DialogResult = true;
            Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            IsDeleted = true;
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
