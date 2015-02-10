using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using Octokit;

namespace MyGit.ViewModels
{
    public class NewsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Activity> _newsItems;

        public IEnumerable<Activity> NewsItems
        {
            get { return _newsItems; }
            set
            {
                _newsItems = value;
                OnPropertyChanged();
            }
        }

        private readonly IGitHubClient _gitHubClient;

        public NewsViewModel()
        {
            _gitHubClient = App.Container.Resolve<IGitHubClient>();
        }

        public async Task Refresh()
        {
            IsLoading = true;

            NewsItems = null;
            var user = await _gitHubClient.User.Current();
            var received = await _gitHubClient.Activity.Events.GetUserReceived(user.Login);
            var performed = await _gitHubClient.Activity.Events.GetUserPerformed(user.Login);
            NewsItems = received.Union(performed).OrderByDescending(a => a.CreatedAt);

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
