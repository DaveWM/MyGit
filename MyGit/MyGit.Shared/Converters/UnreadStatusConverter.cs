using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MyGit.Converters
{
    public class UnreadStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof (Symbol) || value.GetType() != typeof(bool))
                return null;

            if ((bool)value)
            {
                return Symbol.MailFilled;
            }
            else
            {
                return Symbol.Read;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
