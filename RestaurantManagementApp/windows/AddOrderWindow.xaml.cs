using RestaurantManagementApp.Models;
using RestaurantManagementApp.Services;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantManagementApp.windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        private List<Customer> _customers;
        private List<Staff> _staff;
        private List<Models.Table> _tables;
        private int _orderId;
        private bool _isEditMode = false;
        private DateTime _originalOrderDateTime;


        private List<OrderDetailDto> _orderDetails = new List<OrderDetailDto>();

        public AddOrderWindow()
        {
            InitializeComponent();
            LoadComboBoxes();
            OrderDatePicker.SelectedDate = DateTime.Now;
        }


        private void LoadComboBoxes()
        {
            using (var context = new RestaurantPlazaKursachContext())
            {
                _customers = context.Customers.ToList();
                CustomerComboBox.ItemsSource = _customers;
                CustomerComboBox.DisplayMemberPath = "FullName";

                _staff = context.Staff.ToList();
                StaffComboBox.ItemsSource = _staff;
                StaffComboBox.DisplayMemberPath = "FullName";

                if (_isEditMode)
                {
                    _tables = context.Tables.ToList();
                }
                else
                {
                    _tables = context.Tables.Where(t => t.IsAvailable == true).ToList();
                }

                TableComboBox.ItemsSource = _tables;
                TableComboBox.DisplayMemberPath = "TableNumber";
            }
        }


        private void AddDetail_Click(object sender, RoutedEventArgs e)
        {
            var detailWindow = new AddOrderDetailWindow();
            if (detailWindow.ShowDialog() == true)
            {
                var detail = detailWindow.ResultDetail;
                _orderDetails.Add(detail);
                RefreshDetailsGrid();
            }
        }

        private void RefreshDetailsGrid()
        {
            OrderDetailsGrid.ItemsSource = null;
            OrderDetailsGrid.ItemsSource = _orderDetails;
        }

        public void LoadOrder(OrderDto order)
        {
            _isEditMode = true;
            _orderId = order.OrderID;
            _originalOrderDateTime = order.OrderDateTime;
            if (!_tables.Any(t => t.TableId == order.TableID))
            {
                using (var context = new RestaurantPlazaKursachContext())
                {
                    var currentTable = context.Tables.FirstOrDefault(t => t.TableId == order.TableID);
                    if (currentTable != null)
                    {
                        _tables.Add(currentTable);
                        TableComboBox.ItemsSource = null;
                        TableComboBox.ItemsSource = _tables;
                        TableComboBox.DisplayMemberPath = "TableNumber";
                    }
                }
            }
            CustomerComboBox.SelectedItem = _customers.FirstOrDefault(c => c.CustomerId == order.CustomerID);
            StaffComboBox.SelectedItem = _staff.FirstOrDefault(s => s.StaffId == order.StaffID);
            TableComboBox.SelectedItem = _tables.FirstOrDefault(t => t.TableId == order.TableID);

            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == order.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
            OrderDatePicker.SelectedDate = order.OrderDateTime.Date;

            _orderDetails = order.Details;
            RefreshDetailsGrid();
            TableComboBox.IsEnabled = false;
        }



        private async void SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime orderDateTime;

                if (_orderId == 0)
                {
                    DateTime selectedDate = OrderDatePicker.SelectedDate ?? DateTime.Today;
                    orderDateTime = selectedDate.Date.Add(DateTime.Now.TimeOfDay);
                }
                else
                {
                    orderDateTime = _originalOrderDateTime;
                }

                var orderDto = new OrderDto
                {
                    OrderID = _orderId,
                    CustomerID = ((Customer)CustomerComboBox.SelectedItem)?.CustomerId,
                    StaffID = ((Staff)StaffComboBox.SelectedItem).StaffId,
                    TableID = ((Models.Table)TableComboBox.SelectedItem).TableId,
                    Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString(),
                    OrderDateTime = orderDateTime,
                    Details = _orderDetails
                };

                if (_orderId == 0)
                {
                    int newOrderId = await OrderService.CreateOrderAsync(orderDto);
                    MessageBox.Show($"Заказ #{newOrderId} успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await OrderService.UpdateOrderAsync(orderDto);
                    MessageBox.Show($"Заказ #{_orderId} успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении заказа: Не все поля заполнены");
            }
        }
        private void EditDetail_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDetailsGrid.SelectedItem is OrderDetailDto selectedDetail)
            {
                var editWindow = new AddOrderDetailWindow(selectedDetail);
                if (editWindow.ShowDialog() == true)
                {
                    int index = _orderDetails.IndexOf(selectedDetail);
                    if (index >= 0)
                    {
                        _orderDetails[index] = editWindow.ResultDetail;
                        RefreshDetailsGrid();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.");
            }
        }
        private void DeleteDetail_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDetailsGrid.SelectedItem is OrderDetailDto selectedDetail)
            {
                if (MessageBox.Show("Удалить выбранную деталь?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _orderDetails.Remove(selectedDetail);
                    RefreshDetailsGrid();
                    OrderDetailsGrid.SelectedItem = null;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }




        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
