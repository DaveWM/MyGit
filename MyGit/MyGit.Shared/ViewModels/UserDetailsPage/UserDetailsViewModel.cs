using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.UserDetailsPage
{
    public class UserDetailsViewModel : BaseViewModel
    {
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
            this.Refresh();
        }

        protected override async Task RefreshInternal()
        {
            User = await GitHubClient.User.Current();
        }
    }
}
