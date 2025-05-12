using RestaurantManagementApp.Models;
using RestaurantManagementApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для EditPromotionDishesWindow.xaml
    /// </summary>
    public partial class EditPromotionDishesWindow : Window
    {
        private readonly Promotion _promotion;
        private readonly PromotionService _promotionService;
        public List<DishSelection> DishSelections { get; set; }

        public EditPromotionDishesWindow(Promotion promotion, PromotionService promotionService)
        {
            InitializeComponent();
            _promotion = promotion;
            _promotionService = promotionService;
            LoadData();
        }

        // Загружаем все блюда и привязанные к акции
        private void LoadData()
        {
            var allDishes = _promotionService.GetAllDishes(); // Метод получения всех блюд
            var currentDishIds = _promotionService.GetPromotionDishIds(_promotion.PromotionId); // Метод получения текущих блюд акции

            DishSelections = allDishes
                .Select(d => new DishSelection
                {
                    DishId = d.DishId,
                    DishName = d.DishName,
                    IsSelected = currentDishIds.Contains(d.DishId) // Отметим блюда, которые уже привязаны к акции
                })
                .ToList();

            DishesListBox.ItemsSource = DishSelections;
        }

        // Обработчик сохранения
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var selectedIds = DishSelections
                .Where(ds => ds.IsSelected)
                .Select(ds => ds.DishId)
                .ToList();

            await _promotionService.UpdatePromotionDishesAsync(_promotion.PromotionId, selectedIds); // Метод обновления связи
            this.DialogResult = true;
        }

        // Обработчик отмены
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        // Класс для представления блюда с состоянием выбора
        public class DishSelection : INotifyPropertyChanged
        {
            public int DishId { get; set; }
            public string DishName { get; set; }
            private bool _isSelected;

            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
        }
    }
}
