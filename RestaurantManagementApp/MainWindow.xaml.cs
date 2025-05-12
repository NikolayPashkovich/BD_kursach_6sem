using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using RestaurantManagementApp.Services;
using RestaurantManagementApp.windows;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Microsoft.Win32;
using System.Diagnostics;


namespace RestaurantManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestaurantPlazaKursachContext _context = new RestaurantPlazaKursachContext();
        private CustomerService _customerService = new(new RestaurantPlazaKursachContext());
        private ObservableCollection<Customer> _customers;
        private readonly DishService _dishService = new DishService(new RestaurantPlazaKursachContext());
        private List<Dish> allDishes;
        private StaffService _staffService = new StaffService(new RestaurantPlazaKursachContext());
        private readonly IngredientService _ingredientService = new IngredientService(new RestaurantPlazaKursachContext());
        private readonly DishIngredientService _dishIngredientService = new(new RestaurantPlazaKursachContext());
        private readonly MenuService _menuService = new(new RestaurantPlazaKursachContext());
        private readonly PromotionService _promotionService = new(new RestaurantPlazaKursachContext());
        private List<OrderWithDetailsViewModel> _allOrders = new();
        private List<Customer> _allCustomers;
        private List<Staff> _allStaff;
        private List<DishIngredient> allDishIngredients;
        private List<Ingredient> allIngredients;
        private List<Models.Menu> allMenus;
        private List<Promotion> _allPromotions = new();
        private List<Supplier> _allSuppliers = new();
        private List<Supply> _allSupplies = new();
        private List<Table> _allTables = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            LoadOrders();
            LoadCustomers();
            LoadDishes();
            LoadStaff();
            LoadIngredients();
            LoadDishIngredients();
            LoadMenus();
            LoadPromotions();
            LoadSuppliers();
            LoadSuppliesAsync();
            LoadTables();
        }
        private void LoadData()
        {
            ClientsGrid.ItemsSource = _context.Customers.ToList();
            DishesGrid.ItemsSource = _context.Dishes.ToList();
            DishIngridientsGrid.ItemsSource = _context.DishIngredients.Include("Dish").Include("Ingredient").ToList();
            ClientsGrid.ItemsSource = _context.Customers.ToList();
            DishesGrid.ItemsSource = _context.Dishes.ToList();
            DishIngridientsGrid.ItemsSource = _context.DishIngredients.Include("Dish").Include("Ingredient").ToList();
            IngredientsGrid.ItemsSource = _context.Ingredients.ToList();
            MenuGrid.ItemsSource = _context.Menus.ToList();
            OrdersGrid.ItemsSource = _context.Orders.Include("Customer").Include("Staff").Include("Table").ToList();
            PromotionsGrid.ItemsSource = _context.Promotions.ToList();
            StaffGrid.ItemsSource = _context.Staff.ToList();
            SuppliersGrid.ItemsSource = _context.Suppliers.ToList();
            SuppliesGrid.ItemsSource = _context.Supplies.Include("Ingredient").Include("Supplier").ToList();
            TablesGrid.ItemsSource = _context.Tables.ToList();

        }
        private void LoadOrders()
        {
            var orders = _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .Include(o => o.Table)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Dish)
                .Include(o => o.Review)
                .ToList();

            _allOrders = orders.Select(o => new OrderWithDetailsViewModel
            {
                OrderId = o.OrderId,
                CustomerName = o.Customer != null ? o.Customer.FirstName + " " + o.Customer.LastName : "Не указан",
                StaffName = o.Staff != null ? o.Staff.FirstName + " " + o.Staff.LastName : "Не указан",
                TableNumber = o.Table != null ? o.Table.TableNumber : "Не указан",
                OrderDateTime = o.OrderDateTime ?? DateTime.Now,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                DishNames = o.OrderDetails.Where(od => od.Dish != null)
                .Select(od => od.Dish.DishName)
                .Where(name => !string.IsNullOrEmpty(name)).ToList(),
                Rating = o.Review?.Rating
            }).ToList();

            OrdersGrid.ItemsSource = _allOrders;
        }

        private void LoadCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            _allCustomers = customers;
            _customers = new ObservableCollection<Customer>(customers);
            ClientsGrid.ItemsSource = _customers;
        }


        private void LoadDishes()
        {
            allDishes = _dishService.GetAllDishes();
            ApplyDishFilter();
        }

        private void ApplyDishFilter()
        {
            if (allDishes == null) return;

            var name = DishNameFilterBox.Text?.Trim().ToLower() ?? "";
            var category = DishCategoryFilterBox.Text?.Trim().ToLower() ?? "";
            var priceInput = DishPriceFilterBox.Text?.Trim();
            var selectedAvailability = (DishAvailabilityFilterBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            IEnumerable<Dish> filtered = allDishes;

            if (!string.IsNullOrWhiteSpace(name))
                filtered = filtered.Where(d => d.DishName.ToLower().Contains(name));

            if (!string.IsNullOrWhiteSpace(category))
                filtered = filtered.Where(d => d.Category.ToLower().Contains(category));

            if (!string.IsNullOrWhiteSpace(priceInput))
                filtered = filtered.Where(d => PriceFilterMatch(d.Price, priceInput));

            switch (selectedAvailability)
            {
                case "Только активные":
                    filtered = filtered.Where(d => d.IsActive == true);
                    break;
                case "Только скрытые":
                    filtered = filtered.Where(d => d.IsActive == false);
                    break;
            }

            DishesGrid.ItemsSource = filtered.ToList();
        }
        private bool PriceFilterMatch(decimal price, string input)
        {
            try
            {
                input = input.Replace(',', '.');
                if (input.StartsWith(">"))
                {
                    if (decimal.TryParse(input.Substring(1), out var val))
                        return price > val;
                }
                else if (input.StartsWith("<"))
                {
                    if (decimal.TryParse(input.Substring(1), out var val))
                        return price < val;
                }
                else if (decimal.TryParse(input, out var exact))
                {
                    return price == exact;
                }
            }
            catch { }

            return true;
        }

        private void DishFilterChanged(object sender, EventArgs e)
        {
            ApplyDishFilter();
        }
        private void ClearDishFilters(object sender, RoutedEventArgs e)
        {
            DishNameFilterBox.Text = "";
            DishCategoryFilterBox.Text = "";
            DishPriceFilterBox.Text = "";
            DishAvailabilityFilterBox.SelectedIndex = 0;
        }
        private void ToggleDishFilterPanel(object sender, RoutedEventArgs e)
        {
            DishFilterPanel.Visibility = DishFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleDishFilterButton.Content = DishFilterPanel.Visibility == Visibility.Visible
                ? "Фильтры ⬆"
                : "Фильтры ⬇";
        }

        private async void LoadStaff()
        {
            var staff = await _staffService.GetAllAsync();
            _allStaff = staff.ToList();
            StaffGrid.ItemsSource = _allStaff;
        }

        private async void LoadIngredients()
        {
            allIngredients = (await _ingredientService.GetAllAsync()).ToList();
            ApplyIngredientFilter();
        }

        private async void LoadDishIngredients()
        {
            allDishIngredients = (await _dishIngredientService.GetAllAsync()).ToList();
            ApplyRecipeFilter();
        }

        private async void LoadMenus()
        {
            allMenus = await _menuService.GetAllAsync();
            ApplyMenuFilter();
        }

        private async void LoadPromotions()
        {
            _allPromotions = await _promotionService.GetAllAsync();
            ApplyPromotionFilters();
        }
        private async Task LoadSuppliers()
        {
            _allSuppliers = await SupplierService.GetAllSuppliersAsync();
            SuppliersGrid.ItemsSource = _allSuppliers;
        }
        private async Task LoadSuppliesAsync()
        {
            using var context = new RestaurantPlazaKursachContext();
            _allSupplies = await context.Supplies
                .Include(s => s.Supplier)
                .Include(s => s.Ingredient)
                .ToListAsync();

            SuppliesGrid.ItemsSource = _allSupplies;
        }
        private async void LoadTables()
        {
            _allTables = await TableService.GetAllTablesAsync();
            TablesGrid.ItemsSource = _allTables;
        }

        private void AddClient(object sender, RoutedEventArgs e)
        {
            var window = new AddEditCustomerWindow();
            if (window.ShowDialog() == true)
            {
                _customerService.AddCustomer(window.Customer);
                LoadCustomers();
            }
        }

        private void EditClient(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is Customer selectedCustomer)
            {
                var window = new AddEditCustomerWindow(selectedCustomer);
                if (window.ShowDialog() == true)
                {
                    _customerService.UpdateCustomer(window.Customer);
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.");
            }
        }

        private void DeleteClient(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is Customer selectedCustomer)
            {
                var result = MessageBox.Show($"Удалить клиента {selectedCustomer.FullName}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _customerService.DeleteCustomer(selectedCustomer);
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.");
            }
        }
        private void FilterCustomers(object sender, EventArgs e)
        {
            var firstName = FirstNameFilterBox.Text?.Trim().ToLower() ?? "";
            var lastName = LastNameFilterBox.Text?.Trim().ToLower() ?? "";
            var phone = PhoneFilterBox.Text?.Trim() ?? "";

            var filtered = _allCustomers.Where(c =>
                (string.IsNullOrEmpty(firstName) || c.FirstName.ToLower().Contains(firstName)) &&
                (string.IsNullOrEmpty(lastName) || c.LastName.ToLower().Contains(lastName)) &&
                (string.IsNullOrEmpty(phone) || c.Phone.Contains(phone))
            ).ToList();

            _customers = new ObservableCollection<Customer>(filtered);
            ClientsGrid.ItemsSource = _customers;
        }

        private void ClearCustomerFilters(object sender, RoutedEventArgs e)
        {
            FirstNameFilterBox.Text = "";
            LastNameFilterBox.Text = "";
            PhoneFilterBox.Text = "";
        }

        private void ToggleClientFilterPanel(object sender, RoutedEventArgs e)
        {
            ClientFilterPanel.Visibility = ClientFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleClientFilterButton.Content = ClientFilterPanel.Visibility == Visibility.Visible
                ? "Фильтры ⬆"
                : "Фильтры ⬇";
        }


        private async void ManageReview(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is not OrderWithDetailsViewModel selectedOrder)
            {
                MessageBox.Show("Пожалуйста, выберите заказ для оценки.");
                return;
            }

            var existingRating = await ReviewService.GetReviewRatingAsync(selectedOrder.OrderId);
            var reviewWindow = new ReviewWindow(selectedOrder.OrderId, existingRating);
            if (reviewWindow.ShowDialog() == true)
            {
                if (reviewWindow.IsDeleted)
                {
                    await ReviewService.DeleteReviewAsync(selectedOrder.OrderId);
                    selectedOrder.Rating = null;
                }
                else
                {
                    await ReviewService.AddOrUpdateReviewAsync(selectedOrder.OrderId, reviewWindow.CurrentRating.Value);
                    selectedOrder.Rating = reviewWindow.CurrentRating;
                }

                OrdersGrid.Items.Refresh();
            }
        }

        private void AddDish(object sender, RoutedEventArgs e)
        {
            var window = new AddEditDishWindow();
            if (window.ShowDialog() == true)
            {
                _dishService.AddDish(window.Dish);
                LoadDishes();
            }
        }

        private void EditDish(object sender, RoutedEventArgs e)
        {
            if (DishesGrid.SelectedItem is Dish selectedDish)
            {
                var window = new AddEditDishWindow(selectedDish);
                if (window.ShowDialog() == true)
                {
                    _dishService.UpdateDish(window.Dish);
                    LoadDishes();
                }
            }
            else
            {
                MessageBox.Show("Выберите блюдо для редактирования.");
            }
        }

        private void DeleteDish(object sender, RoutedEventArgs e)
        {
            if (DishesGrid.SelectedItem is Dish selectedDish)
            {
                var result = MessageBox.Show(
                    $"Вы уверены, что хотите скрыть блюдо \"{selectedDish.DishName}\"? Оно останется в заказах, но будет недоступно для новых.",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _dishService.DeactivateDish(selectedDish.DishId);
                    LoadDishes();
                }
            }
            else
            {
                MessageBox.Show("Выберите блюдо для удаления.");
            }
        }



        private async void AddDishIngredient(object sender, RoutedEventArgs e)
        {
            var window = new AddEditDishIngredientWindow(await _context.Dishes.ToListAsync(), await _context.Ingredients.ToListAsync());

            if (window.ShowDialog() == true)
            {
                try
                {
                    await _dishIngredientService.AddAsync(window.DishIngredient);
                    LoadDishIngredients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void EditDishIngredient(object sender, RoutedEventArgs e)
        {
            if (DishIngridientsGrid.SelectedItem is DishIngredient selected)
            {

                var window = new AddEditDishIngredientWindow(await _context.Dishes.ToListAsync(), await _context.Ingredients.ToListAsync(), selected);

                if (window.ShowDialog() == true)
                {
                    try
                    {
                        await _dishIngredientService.UpdateAsync(window.DishIngredient);
                        LoadDishIngredients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите рецепт для редактирования.");
            }
        }

        private async void DeleteDishIngredient(object sender, RoutedEventArgs e)
        {
            if (DishIngridientsGrid.SelectedItem is DishIngredient selected)
            {

                var result = MessageBox.Show("Удалить рецепт?", "Подтверждение", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    await _dishIngredientService.DeleteAsync(selected);
                    LoadDishIngredients();
                }
            }
            else
            {
                MessageBox.Show("Выберите рецепт для удаления.");
            }
        }
        private void ApplyRecipeFilter()
        {
            if (allDishIngredients == null) return;

            string dishName = RecipeDishFilterBox.Text.Trim().ToLower();
            string ingredientName = RecipeIngredientFilterBox.Text.Trim().ToLower();
            string quantityInput = RecipeQuantityFilterBox.Text.Trim();

            IEnumerable<DishIngredient> filtered = allDishIngredients;

            if (!string.IsNullOrWhiteSpace(dishName))
                filtered = filtered.Where(d => d.Dish.DishName.ToLower().Contains(dishName));

            if (!string.IsNullOrWhiteSpace(ingredientName))
                filtered = filtered.Where(d => d.Ingredient.IngredientName.ToLower().Contains(ingredientName));

            if (!string.IsNullOrWhiteSpace(quantityInput))
                filtered = filtered.Where(d => QuantityFilterMatch(d.Quantity, quantityInput));

            DishIngridientsGrid.ItemsSource = filtered.ToList();
        }
        private bool QuantityFilterMatch(decimal quantity, string input)
        {
            try
            {
                input = input.Replace(',', '.');
                if (input.StartsWith(">"))
                {
                    if (decimal.TryParse(input[1..], out var val))
                        return quantity > val;
                }
                else if (input.StartsWith("<"))
                {
                    if (decimal.TryParse(input[1..], out var val))
                        return quantity < val;
                }
                else if (decimal.TryParse(input, out var exact))
                {
                    return quantity == exact;
                }
            }
            catch { }

            return true;
        }
        private void RecipeFilterChanged(object sender, EventArgs e)
        {
            ApplyRecipeFilter();
        }
        private void ClearRecipeFilters(object sender, RoutedEventArgs e)
        {
            RecipeDishFilterBox.Text = "";
            RecipeIngredientFilterBox.Text = "";
            RecipeQuantityFilterBox.Text = "";
        }
        private void ToggleRecipeFilterPanel(object sender, RoutedEventArgs e)
        {
            RecipeFilterPanel.Visibility = RecipeFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleRecipeFilterButton.Content = RecipeFilterPanel.Visibility == Visibility.Visible
                ? "Фильтры ⬆"
                : "Фильтры ⬇";
        }

        private async void AddIngredient(object sender, RoutedEventArgs e)
        {
            var window = new AddEditIngredientWindow();
            if (window.ShowDialog() == true)
            {
                await _ingredientService.AddAsync(window.Ingredient);
                LoadIngredients();
            }
        }

        private async void EditIngredient(object sender, RoutedEventArgs e)
        {
            if (IngredientsGrid.SelectedItem is Ingredient selected)
            {
                var window = new AddEditIngredientWindow(selected);
                if (window.ShowDialog() == true)
                {
                    await _ingredientService.UpdateAsync(window.Ingredient);
                    LoadIngredients();
                }
            }
            else
            {
                MessageBox.Show("Выберите ингредиент для редактирования.");
            }
        }

        private async void DeleteIngredient(object sender, RoutedEventArgs e)
        {
            if (IngredientsGrid.SelectedItem is Ingredient selected)
            {
                if (!await _ingredientService.CanDeleteAsync(selected.IngredientId))
                {
                    MessageBox.Show("Нельзя удалить ингредиент, так как он участвовал в поставках. Это историческая информация.");
                    return;
                }

                var result = MessageBox.Show("Удалить ингредиент?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await _ingredientService.DeleteAsync(selected);
                    LoadIngredients();
                }
            }
            else
            {
                MessageBox.Show("Выберите ингредиент для удаления.");
            }
        }
        private void ApplyIngredientFilter()
        {
            if (allIngredients == null) return;

            string name = IngredientNameFilterBox.Text.Trim().ToLower();
            string unit = UnitFilterBox.Text.Trim().ToLower();
            string stockInput = StockFilterBox.Text.Trim();

            IEnumerable<Ingredient> filtered = allIngredients;

            if (!string.IsNullOrWhiteSpace(name))
                filtered = filtered.Where(i => i.IngredientName.ToLower().Contains(name));

            if (!string.IsNullOrWhiteSpace(unit))
                filtered = filtered.Where(i => i.Unit.ToLower().Contains(unit));

            if (!string.IsNullOrWhiteSpace(stockInput))
                filtered = filtered.Where(i => StockFilterMatch(i.CurrentStock ?? 0, stockInput));

            IngredientsGrid.ItemsSource = filtered.ToList();
        }
        private bool StockFilterMatch(decimal stock, string input)
        {
            try
            {
                input = input.Replace(',', '.');
                if (input.StartsWith(">"))
                {
                    if (decimal.TryParse(input[1..], out var val))
                        return stock > val;
                }
                else if (input.StartsWith("<"))
                {
                    if (decimal.TryParse(input[1..], out var val))
                        return stock < val;
                }
                else if (decimal.TryParse(input, out var exact))
                {
                    return stock == exact;
                }
            }
            catch { }

            return true;
        }
        private void IngredientFilterChanged(object sender, EventArgs e)
        {
            ApplyIngredientFilter();
        }
        private void ClearIngredientFilters(object sender, RoutedEventArgs e)
        {
            IngredientNameFilterBox.Text = "";
            UnitFilterBox.Text = "";
            StockFilterBox.Text = "";
        }
        private void ToggleIngredientFilterPanel(object sender, RoutedEventArgs e)
        {
            IngredientFilterPanel.Visibility = IngredientFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleIngredientFilterButton.Content = IngredientFilterPanel.Visibility == Visibility.Visible
                ? "Фильтры ⬆"
                : "Фильтры ⬇";
        }
    
        private async void AddMenu(object sender, RoutedEventArgs e)
        {
            var window = new AddEditMenuWindow();
            if (window.ShowDialog() == true)
            {
                try
                {
                    await _menuService.AddAsync(window.Menu);
                    LoadMenus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void EditMenu(object sender, RoutedEventArgs e)
        {
            if (MenuGrid.SelectedItem is Models.Menu selected)
            {
                var window = new AddEditMenuWindow(selected);
                if (window.ShowDialog() == true)
                {
                    try
                    {
                        await _menuService.UpdateAsync(window.Menu);
                        LoadMenus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите меню для редактирования.");
            }
        }

        private async void DeleteMenu(object sender, RoutedEventArgs e)
        {
            if (MenuGrid.SelectedItem is Models.Menu selected)
            {
                if (MessageBox.Show("Удалить это меню?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await _menuService.DeleteAsync(selected);
                    LoadMenus();
                }
            }
            else
            {
                MessageBox.Show("Выберите меню для удаления.");
            }
        }
        private void ApplyMenuFilter()
        {
            if (allMenus == null) return;

            string name = MenuNameFilterBox.Text.Trim().ToLower();
            string dishes = DishesFilterBox.Text.Trim().ToLower();
            DateTime? fromDate = StartDateFromPicker.SelectedDate;
            DateTime? toDate = StartDateToPicker.SelectedDate;

            IEnumerable<Models.Menu> filtered = allMenus;

            if (!string.IsNullOrWhiteSpace(name))
                filtered = filtered.Where(m => m.MenuName.ToLower().Contains(name));

            if (fromDate.HasValue)
                filtered = filtered.Where(m => m.StartDate.Date >= fromDate.Value.Date);

            if (toDate.HasValue)
                filtered = filtered.Where(m => m.StartDate.Date <= toDate.Value.Date);

            if (!string.IsNullOrWhiteSpace(dishes))
                filtered = filtered.Where(m => m.DishNames.Any(d => d.ToLower().Contains(dishes)));

            MenuGrid.ItemsSource = filtered.ToList();
        }

        private void MenuFilterChanged(object sender, EventArgs e)
        {
            ApplyMenuFilter();
        }
        private void ClearMenuFilters(object sender, RoutedEventArgs e)
        {
            MenuNameFilterBox.Text = "";
            DishesFilterBox.Text = "";
            StartDateFromPicker.SelectedDate = null;
            StartDateToPicker.SelectedDate = null;
        }

        private void ToggleMenuFilterPanel(object sender, RoutedEventArgs e)
        {
            MenuFilterPanel.Visibility = MenuFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleMenuFilterButton.Content = MenuFilterPanel.Visibility == Visibility.Visible
                ? "Фильтры ⬆"
                : "Фильтры ⬇";
        }


        private void AddOrder(object sender, RoutedEventArgs e)
        {
            bool hasFreeTables = _context.Tables.Any(t => t.IsAvailable == true);
            if (!hasFreeTables)
            {
                MessageBox.Show("Все столы заняты", "Нет доступных столов");
                return; 
            }
            var addOrderWindow = new AddOrderWindow();

            if (addOrderWindow.ShowDialog() == true)
            {
                LoadOrders();
            }
        }

        private async void EditOrder(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is OrderWithDetailsViewModel selectedOrder)
            {
                var orderDto = await OrderService.GetOrderByIdAsync(selectedOrder.OrderId);
                if (orderDto == null)
                {
                    MessageBox.Show("Не удалось загрузить данные заказа.");
                    return;
                }

                var editWindow = new AddOrderWindow();
                editWindow.LoadOrder(orderDto); 

                if (editWindow.ShowDialog() == true)
                {
                    LoadOrders();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для редактирования.");
            }
        }


        private async void DeleteOrder(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is OrderWithDetailsViewModel selectedOrder)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить заказ #{selectedOrder.OrderId}?",
                                             "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await OrderService.DeleteOrderAsync(selectedOrder.OrderId);
                    LoadOrders();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.");
            }
        }
        private void ApplyOrderFilters()
        {
            var filtered = _allOrders.AsEnumerable();

            // Клиент
            var customerText = CustomerFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(customerText))
                filtered = filtered.Where(o => o.CustomerName?.Contains(customerText, StringComparison.OrdinalIgnoreCase) == true);

            // Сотрудник
            var staffText = StaffFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(staffText))
                filtered = filtered.Where(o => o.StaffName?.Contains(staffText, StringComparison.OrdinalIgnoreCase) == true);

            // Стол
            var tableText = TableFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(tableText))
                filtered = filtered.Where(o => o.TableNumber?.Contains(tableText, StringComparison.OrdinalIgnoreCase) == true);

            // Статус
            var statusText = StatusFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(statusText))
                filtered = filtered.Where(o => o.Status?.Contains(statusText, StringComparison.OrdinalIgnoreCase) == true);

            // Оценка
            var ratingText = RatingFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(ratingText))
            {
                if (ratingText.StartsWith(">") && int.TryParse(ratingText[1..], out var minRating))
                    filtered = filtered.Where(o => o.Rating.HasValue && o.Rating > minRating);
                else if (ratingText.StartsWith("<") && int.TryParse(ratingText[1..], out var maxRating))
                    filtered = filtered.Where(o => o.Rating.HasValue && o.Rating < maxRating);
                else if (int.TryParse(ratingText, out var exactRating))
                    filtered = filtered.Where(o => o.Rating == exactRating);
            }

            // Сумма
            var amountText = AmountFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(amountText))
            {
                if (amountText.StartsWith(">") && decimal.TryParse(amountText[1..], out var minAmount))
                    filtered = filtered.Where(o => o.TotalAmount > minAmount);
                else if (amountText.StartsWith("<") && decimal.TryParse(amountText[1..], out var maxAmount))
                    filtered = filtered.Where(o => o.TotalAmount < maxAmount);
                else if (decimal.TryParse(amountText, out var exactAmount))
                    filtered = filtered.Where(o => o.TotalAmount == exactAmount);
            }

            // Новый диапазон дат
            DateTime? from = OrderDateFromPicker.SelectedDate;
            DateTime? to = OrderDateToPicker.SelectedDate;
            if (from.HasValue)
                filtered = filtered.Where(o => o.OrderDateTime.Date >= from.Value.Date);
            if (to.HasValue)
                filtered = filtered.Where(o => o.OrderDateTime.Date <= to.Value.Date);

            // Новый фильтр по блюдам
            var dishText = OrderDishesFilterBox.Text?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(dishText))
                filtered = filtered.Where(o => o.DishNames.Any(d => d.ToLower().Contains(dishText)));

            OrdersGrid.ItemsSource = filtered.ToList();
        }



        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            ApplyOrderFilters();
        }
        private void ToggleFilterPanel(object sender, RoutedEventArgs e)
        {
            if (FilterPanel.Visibility == Visibility.Visible)
            {
                FilterPanel.Visibility = Visibility.Collapsed;
                ToggleFilterButton.Content = "Фильтры ⬇";
            }
            else
            {
                FilterPanel.Visibility = Visibility.Visible;
                ToggleFilterButton.Content = "Фильтры ⬆";
            }
        }

        private void ClearFilters(object sender, RoutedEventArgs e)
        {
            CustomerFilterBox.Text = "";
            StaffFilterBox.Text = "";
            TableFilterBox.Text = "";
            StatusFilterBox.Text = "";
            RatingFilterBox.Text = "";
            AmountFilterBox.Text = "";
            OrderDateFromPicker.SelectedDate = null;
            OrderDateToPicker.SelectedDate = null;
            OrderDishesFilterBox.Text = "";

            ApplyOrderFilters();
        }



        private async void AddPromotion(object sender, RoutedEventArgs e)
        {
            var window = new PromotionWindow();
            if (window.ShowDialog() == true)
            {
                try
                {
                    await _promotionService.AddAsync(window.Promotion);
                    LoadPromotions();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
                }
            }
        }

        private async void EditPromotion(object sender, RoutedEventArgs e)
        {
            if (PromotionsGrid.SelectedItem is Promotion selected)
            {
                var window = new PromotionWindow(selected);
                if (window.ShowDialog() == true)
                {
                    try
                    {
                        await _promotionService.UpdateAsync(window.Promotion);
                        LoadPromotions();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при редактировании: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите акцию для редактирования.");
            }
        }

        private async void DeletePromotion(object sender, RoutedEventArgs e)
        {
            if (PromotionsGrid.SelectedItem is Promotion selected)
            {
                if (MessageBox.Show("Удалить акцию?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await _promotionService.DeleteAsync(selected);
                    LoadPromotions();
                }
            }
            else
            {
                MessageBox.Show("Выберите акцию для удаления.");
            }
        }
        private void PromotionFilterChanged(object sender, RoutedEventArgs e)
        {
            ApplyPromotionFilters();
        }

        private void ApplyPromotionFilters()
        {
            var filtered = _allPromotions.AsEnumerable();

            // Название акции
            var nameText = PromotionNameFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(nameText))
                filtered = filtered.Where(p => p.PromotionName.Contains(nameText, StringComparison.OrdinalIgnoreCase));

            // Скидка
            var discountText = DiscountFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(discountText))
            {
                if (discountText.StartsWith(">") && decimal.TryParse(discountText[1..], out var min))
                    filtered = filtered.Where(p => p.DiscountPercent > min);
                else if (discountText.StartsWith("<") && decimal.TryParse(discountText[1..], out var max))
                    filtered = filtered.Where(p => p.DiscountPercent < max);
                else if (decimal.TryParse(discountText, out var exact))
                    filtered = filtered.Where(p => p.DiscountPercent == exact);
            }

            // Поиск по блюдам
            var dishText = PromotionDishFilterBox.Text?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(dishText))
                filtered = filtered.Where(p => p.DishNames.Any(d => d.ToLower().Contains(dishText)));

            // Диапазон по дате действия (пересекается)
            var from = PromotionDateFromPicker.SelectedDate;
            var to = PromotionDateToPicker.SelectedDate;

            if (from.HasValue)
                filtered = filtered.Where(p => p.EndDate.Date >= from.Value.Date); // акция ещё не закончилась до from
            if (to.HasValue)
                filtered = filtered.Where(p => p.StartDate.Date <= to.Value.Date); // акция началась до to

            // Только активные сегодня
            if (ActiveTodayCheckBox.IsChecked == true)
            {
                var today = DateTime.Today;
                filtered = filtered.Where(p => p.StartDate.Date <= today && p.EndDate.Date >= today);
            }

            PromotionsGrid.ItemsSource = filtered.ToList();
        }

        private void ClearPromotionFilters(object sender, RoutedEventArgs e)
        {
            PromotionNameFilterBox.Text = "";
            DiscountFilterBox.Text = "";
            PromotionDishFilterBox.Text = "";

            PromotionDateFromPicker.SelectedDate = null;
            PromotionDateToPicker.SelectedDate = null;
            ActiveTodayCheckBox.IsChecked = false;

            ApplyPromotionFilters();
        }

        private void TogglePromotionFilterPanel(object sender, RoutedEventArgs e)
        {
            if (PromotionFilterPanel.Visibility == Visibility.Visible)
            {
                PromotionFilterPanel.Visibility = Visibility.Collapsed;
                (sender as Button)!.Content = "Фильтры ⬇";
            }
            else
            {
                PromotionFilterPanel.Visibility = Visibility.Visible;
                (sender as Button)!.Content = "Фильтры ⬆";
            }
        }

        private async void AddStaff(object sender, RoutedEventArgs e)
        {
            var window = new AddEditStaffWindow();
            if (window.ShowDialog() == true)
            {
                await _staffService.AddAsync(window.StaffMember);
                LoadStaff();
            }
        }

        private async void EditStaff(object sender, RoutedEventArgs e)
        {
            if (StaffGrid.SelectedItem is Staff selectedStaff)
            {
                var window = new AddEditStaffWindow(selectedStaff);
                if (window.ShowDialog() == true)
                {
                    await _staffService.UpdateAsync(window.StaffMember);
                    LoadStaff();
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
            }
        }

        private async void DeleteStaff(object sender, RoutedEventArgs e)
        {
            if (StaffGrid.SelectedItem is Staff selectedStaff)
            {
                var result = MessageBox.Show($"Удалить {selectedStaff.FullName}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await _staffService.DeleteAsync(selectedStaff);
                    LoadStaff();
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для удаления.");
            }
        }
        private void FilterStaff(object sender, EventArgs e)
        {
            var firstName = StaffFirstNameFilterBox.Text?.Trim().ToLower() ?? "";
            var lastName = StaffLastNameFilterBox.Text?.Trim().ToLower() ?? "";
            var position = StaffPositionFilterBox.Text?.Trim().ToLower() ?? "";

            var filtered = _allStaff.Where(s =>
                (string.IsNullOrEmpty(firstName) || s.FirstName.ToLower().Contains(firstName)) &&
                (string.IsNullOrEmpty(lastName) || s.LastName.ToLower().Contains(lastName)) &&
                (string.IsNullOrEmpty(position) || s.Position.ToLower().Contains(position))
            ).ToList();

            StaffGrid.ItemsSource = filtered;
        }
        private void ClearStaffFilters(object sender, RoutedEventArgs e)
        {
            StaffFirstNameFilterBox.Text = "";
            StaffLastNameFilterBox.Text = "";
            StaffPositionFilterBox.Text = "";
        }
        private void ToggleStaffFilterPanel(object sender, RoutedEventArgs e)
        {
            StaffFilterPanel.Visibility = StaffFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleStaffFilterButton.Content = StaffFilterPanel.Visibility == Visibility.Visible
                ? "Фильтры ⬆"
                : "Фильтры ⬇";
        }

        private async void AddSupplier(object sender, RoutedEventArgs e)
        {
            var window = new SupplierWindow();
            if (window.ShowDialog() == true)
            {
                try
                {
                    await SupplierService.AddSupplierAsync(window.Supplier);
                    await LoadSuppliers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void EditSupplier(object sender, RoutedEventArgs e)
        {
            if (SuppliersGrid.SelectedItem is Supplier selected)
            {
                var window = new SupplierWindow(new Supplier
                {
                    SupplierId = selected.SupplierId,
                    SupplierName = selected.SupplierName,
                    ContactPhone = selected.ContactPhone
                });

                if (window.ShowDialog() == true)
                {
                    try
                    {
                        await SupplierService.UpdateSupplierAsync(window.Supplier);
                        await LoadSuppliers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите поставщика для редактирования.");
            }
        }

        private async void DeleteSupplier(object sender, RoutedEventArgs e)
        {
            if (SuppliersGrid.SelectedItem is Supplier selected)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить поставщика?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        await SupplierService.DeleteSupplierAsync(selected.SupplierId);
                        await LoadSuppliers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите поставщика для удаления.");
            }
        }
        private void ApplySupplierFilters()
        {
            var filtered = _allSuppliers.AsEnumerable();

            var nameText = SupplierNameFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(nameText))
                filtered = filtered.Where(s => s.SupplierName.Contains(nameText, StringComparison.OrdinalIgnoreCase));

            var phoneText = SupplierPhoneFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(phoneText))
                filtered = filtered.Where(s => s.ContactPhone.Contains(phoneText, StringComparison.OrdinalIgnoreCase));

            SuppliersGrid.ItemsSource = filtered.ToList();
        }
        private void SupplierFilterChanged(object sender, RoutedEventArgs e)
        {
            ApplySupplierFilters();
        }
        private void ClearSupplierFilters(object sender, RoutedEventArgs e)
        {
            SupplierNameFilterBox.Text = "";
            SupplierPhoneFilterBox.Text = "";
            ApplySupplierFilters();
        }
        private void ToggleSupplierFilterPanel(object sender, RoutedEventArgs e)
        {
            SupplierFilterPanel.Visibility = SupplierFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleSupplierFilterButton.Content = SupplierFilterPanel.Visibility == Visibility.Visible ? "Фильтры ⬆" : "Фильтры ⬇";
        }

        private async void AddSupply(object sender, RoutedEventArgs e)
        {
            var window = new SupplyWindow();
            if (window.ShowDialog() == true)
                await LoadSuppliesAsync();
        }

        private async void EditSupply(object sender, RoutedEventArgs e)
        {
            if (SuppliesGrid.SelectedItem is Supply selectedSupply)
            {
                var window = new SupplyWindow(selectedSupply);
                if (window.ShowDialog() == true)
                    await LoadSuppliesAsync();
            }
            else
            {
                MessageBox.Show("Выберите поставку для редактирования.");
            }
        }

        private async void DeleteSupply(object sender, RoutedEventArgs e)
        {
            if (SuppliesGrid.SelectedItem is Supply selectedSupply)
            {
                if (MessageBox.Show("Удалить поставку?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await SupplyService.DeleteSupplyAsync(selectedSupply.SupplyId);
                    await LoadSuppliesAsync();
                }
            }
            else
            {
                MessageBox.Show("Выберите поставку для удаления.");
            }
        }
        private void ApplySupplyFilters()
        {
            var filtered = _allSupplies.AsEnumerable();

            var supplierText = SupplierFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(supplierText))
                filtered = filtered.Where(s => s.Supplier.SupplierName.Contains(supplierText, StringComparison.OrdinalIgnoreCase));

            var ingredientText = IngredientFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(ingredientText))
                filtered = filtered.Where(s => s.Ingredient.IngredientName.Contains(ingredientText, StringComparison.OrdinalIgnoreCase));

            var quantityText = QuantityFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(quantityText))
            {
                if (quantityText.StartsWith(">") && decimal.TryParse(quantityText[1..], out var min))
                    filtered = filtered.Where(s => s.Quantity > min);
                else if (quantityText.StartsWith("<") && decimal.TryParse(quantityText[1..], out var max))
                    filtered = filtered.Where(s => s.Quantity < max);
                else if (decimal.TryParse(quantityText, out var exact))
                    filtered = filtered.Where(s => s.Quantity == exact);
            }

            if (SupplyDateFromPicker.SelectedDate.HasValue)
            {
                var from = SupplyDateFromPicker.SelectedDate.Value.Date;
                filtered = filtered.Where(s => s.SupplyDate.Date >= from);
            }

            if (SupplyDateToPicker.SelectedDate.HasValue)
            {
                var to = SupplyDateToPicker.SelectedDate.Value.Date;
                filtered = filtered.Where(s => s.SupplyDate.Date <= to);
            }

            SuppliesGrid.ItemsSource = filtered.ToList();
        }
        private void SupplyFilterChanged(object sender, RoutedEventArgs e)
        {
            ApplySupplyFilters();
        }
        private void ClearSupplyFilters(object sender, RoutedEventArgs e)
        {
            SupplierFilterBox.Text = "";
            IngredientFilterBox.Text = "";
            QuantityFilterBox.Text = "";
            SupplyDateFromPicker.SelectedDate = null;
            SupplyDateToPicker.SelectedDate = null;

            ApplySupplyFilters();
        }
        private void ToggleSupplyFilterPanel(object sender, RoutedEventArgs e)
        {
            SupplyFilterPanel.Visibility = SupplyFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleSupplyFilterButton.Content = SupplyFilterPanel.Visibility == Visibility.Visible ? "Фильтры ⬆" : "Фильтры ⬇";
        }

        private async void AddTable(object sender, RoutedEventArgs e)
        {
            var tableWindow = new TableWindow();
            var result = tableWindow.ShowDialog();
            if (result == true)
            {
                LoadTables();
            }
        }


        private async void EditTable(object sender, RoutedEventArgs e)
        {
            if (TablesGrid.SelectedItem is Models.Table selectedTable)
            {
                if (selectedTable != null)
                {
                    var tableWindow = new TableWindow(selectedTable);
                    var result = tableWindow.ShowDialog();
                    if (result == true)
                    {
                        LoadTables();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите стол для редактирования.");
            }
        }

        private async void DeleteTable(object sender, RoutedEventArgs e)
        {
            if (TablesGrid.SelectedItem is Models.Table selectedTable)
            {
                if (selectedTable != null)
                {
                    var confirmation = MessageBox.Show("Вы уверены, что хотите удалить этот стол?", "Удаление стола", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (confirmation == MessageBoxResult.Yes)
                    {
                        await TableService.DeleteTableAsync(selectedTable.TableId);
                        LoadTables();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите стол для удаления.");
            }
        }
        private void ApplyTableFilters()
        {
            var filtered = _allTables.AsEnumerable();

            var numberText = TableNumberFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(numberText))
                filtered = filtered.Where(t => t.TableNumber.Contains(numberText, StringComparison.OrdinalIgnoreCase));

            var capacityText = TableCapacityFilterBox.Text?.Trim();
            if (!string.IsNullOrEmpty(capacityText))
                filtered = filtered.Where(t => CapacityFilterMatch(t.Capacity, capacityText));

            if (OnlyAvailableTablesCheckBox.IsChecked == true)
                filtered = filtered.Where(t => t.IsAvailable == true);

            TablesGrid.ItemsSource = filtered.ToList();
        }
        private bool CapacityFilterMatch(int capacity, string input)
        {
            input = input.Replace(" ", "");

            try
            {
                if (input.StartsWith(">=") && int.TryParse(input.Substring(2), out var min1))
                    return capacity >= min1;
                if (input.StartsWith("<=") && int.TryParse(input.Substring(2), out var max1))
                    return capacity <= max1;
                if (input.StartsWith(">") && int.TryParse(input.Substring(1), out var min2))
                    return capacity > min2;
                if (input.StartsWith("<") && int.TryParse(input.Substring(1), out var max2))
                    return capacity < max2;
                if (int.TryParse(input, out var exact))
                    return capacity == exact;
            }
            catch
            {
            }

            return true;
        }

        private void TableFilterChanged(object sender, RoutedEventArgs e)
        {
            ApplyTableFilters();
        }

        private void ClearTableFilters(object sender, RoutedEventArgs e)
        {
            TableNumberFilterBox.Text = "";
            TableCapacityFilterBox.Text = "";
            OnlyAvailableTablesCheckBox.IsChecked = false;
            ApplyTableFilters();
        }
        private void ToggleTableFilterPanel(object sender, RoutedEventArgs e)
        {
            TableFilterPanel.Visibility = TableFilterPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

            ToggleTableFilterButton.Content = TableFilterPanel.Visibility == Visibility.Visible ? "Фильтры ⬆" : "Фильтры ⬇";
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl)
            {
                if (tabControl.SelectedItem == OrdersTab)
                {
                    LoadOrders();
                }
                else if (tabControl.SelectedItem == ClientsTab)
                {
                    LoadCustomers();
                }
                else if (tabControl.SelectedItem == DishTab)
                {
                    LoadDishes();
                }
                else if (tabControl.SelectedItem == StaffTab)
                {
                    LoadStaff();
                }
                else if (tabControl.SelectedItem == IngredientsTab)
                {
                    LoadIngredients();
                }
                else if (tabControl.SelectedItem == DishIngridientsTab)
                {
                    LoadDishIngredients();
                }
                else if (tabControl.SelectedItem == MenuTab)
                {
                    LoadMenus();
                }
                else if (tabControl.SelectedItem == PromotionTab)
                {
                    LoadPromotions();
                }
                else if (tabControl.SelectedItem == SuppliersTab)
                {
                    LoadPromotions();
                }
                else if (tabControl.SelectedItem == SuplyTab)
                {
                    LoadSuppliesAsync();
                }
                else if (tabControl.SelectedItem == TableTab)
                {
                    LoadTables();
                }

                // добавь другие вкладки, если будут, например:
                // else if (tabControl.SelectedItem == TableTab)
                // {
                //     LoadDishes();
                // }
            }
        }

        private async void EditMenuDishes(object sender, RoutedEventArgs e)
        {
            if (MenuGrid.SelectedItem is Models.Menu selectedMenu)
            {
                var window = new EditMenuDishesWindow(selectedMenu.MenuId, _menuService);
                if (window.ShowDialog() == true)
                {
                    LoadMenus();
                }
            }
            else
            {
                MessageBox.Show("Выберите меню для редактирования.");
            }
        }

        private async void EditPromotionDishes(object sender, RoutedEventArgs e)
        {
            if (PromotionsGrid.SelectedItem is Promotion selectedPromotion)
            {
                var window = new EditPromotionDishesWindow(selectedPromotion, _promotionService);
                if (window.ShowDialog() == true)
                {
                    LoadPromotions();
                }
            }
            else
            {
                MessageBox.Show("Выберите акцию для редактирования.");
            }
        }
        private void ExportOrdersToPdf(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "Orders.pdf"
            };

            if (dialog.ShowDialog() != true)
                return;

            var document = new Document();
            document.Info.Title = "Список заказов";

            var section = document.AddSection();
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
            section.PageSetup.LeftMargin = Unit.FromCentimeter(2);
            section.PageSetup.RightMargin = Unit.FromCentimeter(2);
            section.PageSetup.TopMargin = Unit.FromCentimeter(2);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(2);

            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.Font.Name = "Arial";
            table.Format.Font.Size = 9;

            table.AddColumn(Unit.FromCentimeter(3));  
            table.AddColumn(Unit.FromCentimeter(3));  
            table.AddColumn(Unit.FromCentimeter(2));   
            table.AddColumn(Unit.FromCentimeter(3));   
            table.AddColumn(Unit.FromCentimeter(2.5));  
            table.AddColumn(Unit.FromCentimeter(2.5));  
            table.AddColumn(Unit.FromCentimeter(6));  
            table.AddColumn(Unit.FromCentimeter(2));   

                         var headerRow = table.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            headerRow.HeadingFormat = true;
            headerRow.Format.Font.Bold = true;
            headerRow.Format.Alignment = ParagraphAlignment.Center;

            string[] headers = { "Клиент", "Сотрудник", "Стол", "Дата", "Сумма", "Статус", "Блюда", "Оценка" };
            for (int i = 0; i < headers.Length; i++)
                headerRow.Cells[i].AddParagraph(headers[i]);

            foreach (var order in OrdersGrid.ItemsSource as List<OrderWithDetailsViewModel>)
            {
                var row = table.AddRow();
                row.TopPadding = 2;
                row.BottomPadding = 2;

                row.Cells[0].AddParagraph(order.CustomerName);
                row.Cells[1].AddParagraph(order.StaffName);
                row.Cells[2].AddParagraph(order.TableNumber);
                row.Cells[3].AddParagraph(order.OrderDateTime.ToString("g"));
                row.Cells[4].AddParagraph(order.TotalAmount.ToString("F2"));
                row.Cells[5].AddParagraph(order.Status);
                row.Cells[6].AddParagraph(string.Join(", ", order.DishNames ?? new List<string>()));
                row.Cells[7].AddParagraph(order.Rating?.ToString() ?? "-");

                for (int i = 0; i < 8; i++)
                {
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Left;
                    row.Cells[i].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
                }
            }

            var renderer = new PdfDocumentRenderer(true) { Document = document };
            renderer.RenderDocument();
            renderer.Save(dialog.FileName);

            Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true });
        }


    }
}