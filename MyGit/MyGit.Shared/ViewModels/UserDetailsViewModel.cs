using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using Octokit;

namespace MyGit.ViewModels
{
    public class UserDetailsViewModel : INotifyPropertyChanged
    {
        private readonly IGitHubClient _gitHubClient;

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public UserDetailsViewModel()
        {
            _gitHubClient = App.Container.Resolve<IGitHubClient>();

            Refresh();
        }

        private async void Refresh()
        {
            User = await _gitHubClient.User.Current();
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
