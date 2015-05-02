using System;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;

namespace MyGit.Converters
{
    public class CamelCaseSpacingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var camelCaseRegex = new Regex(@"[A-Z][a-z]*");
            return String.Join(" ", camelCaseRegex.Matches(value.ToString()).OfType<Match>().Select(m => m.Value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value.ToString().Replace(" ", string.Empty);
        }
    }
}
