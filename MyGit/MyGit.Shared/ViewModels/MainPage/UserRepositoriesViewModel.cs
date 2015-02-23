using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Octokit;

namespace MyGit.ViewModels.MainPage
{
    public class UserRepositoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Repository> OwnedRepos { get; set; }

        public ObservableCollection<Repository> StarredRepos { get; set; }

        public ObservableCollection<Repository> WatchedRepos { get; set; }

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
