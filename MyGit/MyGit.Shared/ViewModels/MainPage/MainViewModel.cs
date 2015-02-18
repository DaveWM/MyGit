using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Octokit;

namespace MyGit.ViewModels.MainPage
{
    public class MainViewModel : BaseViewModel
    {
        public NotificationsViewModel NotificationsViewModel { get; set; }
        public NewsViewModel NewsViewModel { get; set; }
        public UserRepositoriesViewModel ReposViewModel { get; set; }
        public UserIssuesViewModel IssuesViewModel { get; set; }

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

        public MainViewModel()
        {
            NotificationsViewModel = new NotificationsViewModel();
            NewsViewModel = new NewsViewModel();
            ReposViewModel = new UserRepositoriesViewModel();
            IssuesViewModel = new UserIssuesViewModel();

            this.Refresh();

            GetCurrentUser();
        }

        private async void GetCurrentUser()
        {
            User = await GitHubClient.User.Current();
        }

        protected override async Task RefreshInternal()
        {
            await Task.WhenAll(new List<Task> { NotificationsViewModel.Refresh(), NewsViewModel.Refresh(), ReposViewModel.Refresh(), IssuesViewModel.Refresh() });
        }

        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(() => Refresh());
            }
        }

        public DelegateCommand GoToUserDetails
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    App.Frame.Navigate(typeof (Views.UserDetailsPage));
                });
            }
        }

        public DelegateCommand GoToSearch
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    App.Frame.Navigate(typeof(Views.SearchPage));
                });
            }
        }

        public DelegateCommand Logout
        {
            get { return new DelegateCommand(() =>
            {
                App.Frame.Navigate(typeof (Views.LoginPage));
            });}
        }
    }
}
