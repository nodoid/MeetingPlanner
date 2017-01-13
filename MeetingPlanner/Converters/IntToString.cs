using System;
using System.Globalization;
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class IntToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rv = (int)value;
            return rv.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }
    }
}
