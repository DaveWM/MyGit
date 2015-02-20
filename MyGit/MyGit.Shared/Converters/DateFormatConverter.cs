using System;
using Windows.UI.Xaml.Data;

namespace MyGit.Converters
{
    public sealed class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            return DateTime.Parse(value.ToString()).ToGithubDate();
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}
