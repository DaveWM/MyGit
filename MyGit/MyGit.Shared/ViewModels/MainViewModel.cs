using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using Octokit;

namespace MyGit.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public NotificationsViewModel NotificationsViewModel { get; set; }
        public NewsViewModel NewsViewModel { get; set; }

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

        private bool _isLoading;

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

        private readonly IGitHubClient _gitHubClient;

        public MainViewModel()
        {
            NotificationsViewModel = new NotificationsViewModel();
            NewsViewModel = new NewsViewModel();
            _gitHubClient = App.Container.Resolve<IGitHubClient>();

            Refresh();
        }

        public async void Refresh()
        {
            IsLoading = true;

            User = await _gitHubClient.User.Current();
            await Task.WhenAll(new List<Task> { NotificationsViewModel.Refresh(), NewsViewModel.Refresh() });

            IsLoading = false;
        }

        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(Refresh);
            }
        }

        public DelegateCommand Logout
        {
            get { return new DelegateCommand(() =>
            {
                App.Frame.Navigate(typeof (LoginPage));
            });}
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
