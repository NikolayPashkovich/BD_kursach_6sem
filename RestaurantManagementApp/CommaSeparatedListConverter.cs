using RestaurantManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RestaurantManagementApp.Converters
{
    public class CommaSeparatedListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as System.Collections.Generic.List<string>;
            if (list != null)
            {
                return string.Join(", ", list); // Объединяет элементы списка в строку с разделением через запятую
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
