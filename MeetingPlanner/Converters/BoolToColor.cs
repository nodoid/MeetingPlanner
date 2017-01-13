using System;
using System.Globalization;
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class BoolToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color col;
            var b = value.ToString();
            col = b.ToLower() == "true" ? Color.Blue : Color.Red;
            return (Color)col;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var col = (Color)value;
            return col != Color.Red;
        }
    }
}
