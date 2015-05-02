using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace MyGit.Converters
{
    public class RefToBranchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString().Split('/').Last();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
