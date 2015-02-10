using System;
using Windows.UI.Xaml.Data;

namespace MyGit.Converters
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool original = (bool)value;
            return !original;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool original = (bool)value;
            return !original;
        }
    }
}
