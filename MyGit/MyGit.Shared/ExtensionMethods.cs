using System;

namespace MyGit
{
    public static class ExtensionMethods
    {
        public static string ToGithubDate(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            // span is less than or equal to 60 seconds, measure in seconds.
            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                return "just now";
            }
            // span is less than or equal to 60 minutes, measure in minutes.
            if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                return timeSpan.Minutes > 1
                    ? timeSpan.Minutes + " minutes ago"
                : "a minute ago";
            }
            // span is less than or equal to 24 hours, measure in hours.
            if (timeSpan <= TimeSpan.FromHours(24))
            {
                return timeSpan.Hours > 1
                    ? timeSpan.Hours +" hours ago"
                    : "an hour ago";
            }
            // span is less than or equal to 30 days (1 month), measure in days.
            if (timeSpan <= TimeSpan.FromDays(30))
            {
                return timeSpan.Days > 1
                    ? timeSpan.Days +" days ago"
                    : "a day ago";
            }

            return dateTime.ToString("dd/MM/yyyy");
        }
    }
}
