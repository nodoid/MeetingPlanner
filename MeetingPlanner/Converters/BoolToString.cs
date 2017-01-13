using System;
using System.Globalization;
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class BoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rv = (bool)value;
            return rv.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var t = (bool)value;
            return t.ToString();
        }
    }
}
