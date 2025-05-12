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
    /// Логика взаимодействия для EditMenuDishesWindow.xaml
    /// </summary>
    public partial class EditMenuDishesWindow : Window
    {
        private readonly MenuService _menuService;
        private readonly int _menuId;

        public List<DishSelection> DishSelections { get; set; }

        public EditMenuDishesWindow(int menuId, MenuService menuService)
        {
            InitializeComponent();
            _menuService = menuService;
            _menuId = menuId;
            LoadData();
        }

        private void LoadData()
        {
            var allDishes = _menuService.GetAllDishes(); // будет создан ниже
            var currentDishIds = _menuService.GetMenuDishIds(_menuId); // тоже будет создан

            DishSelections = allDishes
                .Select(d => new DishSelection
                {
                    DishId = d.DishId,
                    DishName = d.DishName,
                    IsSelected = currentDishIds.Contains(d.DishId)
                })
                .ToList();

            DishesListBox.ItemsSource = DishSelections;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var selectedIds = DishSelections
                .Where(ds => ds.IsSelected)
                .Select(ds => ds.DishId)
                .ToList();

            await _menuService.UpdateMenuDishesAsync(_menuId, selectedIds);
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

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
