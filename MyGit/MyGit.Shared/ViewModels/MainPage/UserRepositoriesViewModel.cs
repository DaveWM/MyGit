using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Octokit;

namespace MyGit.ViewModels.MainPage
{
    public class UserRepositoriesViewModel : BaseViewModel
    {
        private ObservableCollection<Repository> _ownedRepos;

        public ObservableCollection<Repository> OwnedRepos
        {
            get { return _ownedRepos; }
            set
            {
                _ownedRepos = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Repository> _starredRepos;

        public ObservableCollection<Repository> StarredRepos
        {
            get { return _starredRepos; }
            set
            {
                _starredRepos = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Repository> _watchedRepos;

        public ObservableCollection<Repository> WatchedRepos
        {
            get { return _watchedRepos; }
            set
            {
                _watchedRepos = value;
                OnPropertyChanged();
            }
        }

        public UserRepositoriesViewModel()
        {
            OwnedRepos = new ObservableCollection<Repository>();
            StarredRepos = new ObservableCollection<Repository>();
            WatchedRepos = new ObservableCollection<Repository>();
        }

        protected override async Task RefreshInternal()
        {
            OwnedRepos.Clear();
            StarredRepos.Clear();
            WatchedRepos.Clear();

            (await GitHubClient.Repository.GetAllForCurrent()).OrderByDescending(r => r.UpdatedAt).ForEach(r => OwnedRepos.Add(r));
            (await GitHubClient.Activity.Starring.GetAllForCurrent()).OrderByDescending(r => r.StargazersCount).ForEach(r => StarredRepos.Add(r));
            (await GitHubClient.Activity.Watching.GetAllForCurrent()).OrderByDescending(r => r.StargazersCount).ForEach(r => WatchedRepos.Add(r));
        }
    }
}
