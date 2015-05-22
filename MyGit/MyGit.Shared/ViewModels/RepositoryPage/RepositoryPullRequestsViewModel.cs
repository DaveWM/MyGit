using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.RepositoryPage
{
    public class RepositoryPullRequestsViewModel : BaseRepoViewModel
    {
        private IEnumerable<PullRequest> _pullRequests;

        public IEnumerable<PullRequest> PullRequests
        {
            get { return _pullRequests; }
            set
            {
                _pullRequests = value;
                OnPropertyChanged();
            }
        } 
        public RepositoryPullRequestsViewModel(string owner, string repo) : base(owner, repo)
        {
        }

        protected async override Task RefreshInternal()
        {
            PullRequests = await GitHubClient.PullRequest.GetAllForRepository(Owner, Repo);
        }
    }
}
