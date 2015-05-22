using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.MainPage
{
    public class NotificationsViewModel : BaseViewModel
    {
        private IEnumerable<Notification> _notifications;

        public IEnumerable<Notification> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged();
            }
        }

        protected override async Task RefreshInternal()
        {
            Notifications = null;

            var unorderedNotifications = await GitHubClient.Notification.GetAllForCurrent(new NotificationsRequest
            {
                All = true
            });
            Notifications = unorderedNotifications.OrderByDescending(n => n.Unread).ThenByDescending(n => n.UpdatedAt);
        }
    }
}
