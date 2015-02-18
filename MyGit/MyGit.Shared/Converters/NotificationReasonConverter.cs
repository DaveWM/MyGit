using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace MyGit.Converters
{
    class NotificationReasonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var reason = value as string;

            switch (reason)
            {
                case "subscribed":
                    return "Watching the repo";
                case "manual":
                    return "Comment in a subscibed thread";
                case "author":
                    return "Comment in a thread you created";
                case "comment":
                    return "You commented";
                case "mention":
                    return "You were mentioned";
                case "team_mention":
                    return "Your team was mentioned";
                case "state_change":
                    return "Thread state changed";
                case "assign":
                    return "You were assigned";
                default:
                    return "Don't know why you were notified ¯\\_(ツ)_/¯";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
