using RestaurantManagementApp.Models;
using System.Windows;

namespace RestaurantManagementApp.windows
{
    public partial class AddEditStaffWindow : Window
    {
        public Staff StaffMember { get; private set; }
        private bool isEdit;

        public AddEditStaffWindow()
        {
            InitializeComponent();
            StaffMember = new Staff();
            this.DataContext = StaffMember;
            isEdit = false;
        }

        public AddEditStaffWindow(Staff existingStaff)
        {
            InitializeComponent();
            StaffMember = new Staff
            {
                StaffId = existingStaff.StaffId,
                FirstName = existingStaff.FirstName,
                LastName = existingStaff.LastName,
                Position = existingStaff.Position
            };
            this.DataContext = StaffMember;
            FirstNameBox.Text = StaffMember.FirstName;
            LastNameBox.Text = StaffMember.LastName;
            PositionBox.Text = StaffMember.Position;
            isEdit = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameBox.Text.Trim();
            string lastName = LastNameBox.Text.Trim();
            string position = PositionBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Имя не может быть пустым.");
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Фамилия не может быть пустой.");
                return;
            }

            if (string.IsNullOrWhiteSpace(position))
            {
                MessageBox.Show("Должность не может быть пустой.");
                return;
            }

            StaffMember.FirstName = firstName;
            StaffMember.LastName = lastName;
            StaffMember.Position = position;

            this.DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
