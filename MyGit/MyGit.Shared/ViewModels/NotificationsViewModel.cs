using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using Octokit;

namespace MyGit.ViewModels
{
    public class NotificationsViewModel : INotifyPropertyChanged
    {
        private readonly IGitHubClient _gitHubClient;
        public NotificationsViewModel()
        {
            _gitHubClient = App.Container.Resolve<IGitHubClient>();
        }

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

        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(()=> Refresh());
            }
        }

        public async Task Refresh()
        {
            IsLoading = true;

            Notifications = null;
            var unorderedNotifications = await _gitHubClient.Notification.GetAllForCurrent(new NotificationsRequest
            {
                All = true
            });
            Notifications = unorderedNotifications.OrderBy(n => n.Unread).ThenByDescending(n => n.UpdatedAt);

            IsLoading = false;
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
