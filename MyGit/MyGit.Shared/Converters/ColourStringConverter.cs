using System;
using System.Collections.Generic;
using System.Text;
using Windows.Media.Devices;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using NotificationsBackgroundTask;

namespace MyGit.Converters
{
    public class ColourStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var hexString = value as string;
            var colour = ColourParser.ParseHexString(hexString);
            return new SolidColorBrush(colour);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
