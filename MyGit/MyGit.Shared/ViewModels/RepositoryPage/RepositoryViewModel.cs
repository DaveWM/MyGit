using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.RepositoryPage
{
    public class RepositoryViewModel : BaseRepoViewModel
    {
        private Repository _repository;

        public Repository Repository
        {
            get { return _repository; }
            set
            {
                _repository = value; 
                OnPropertyChanged();
            }
        }

        public RepositoryIssuesViewModel IssuesViewModel { get; set; }
        public RepositoryPullRequestsViewModel PullRequestsViewModel { get; set; }
        public RepositoryCommitsViewModel CommitsViewModel { get; set; }

        private readonly string _owner;
        private readonly string _name;

        public RepositoryViewModel(string owner, string name) : base(owner, name)
        {
            _owner = owner;
            _name = name;

            IssuesViewModel = new RepositoryIssuesViewModel(_owner, _name);
            PullRequestsViewModel = new RepositoryPullRequestsViewModel(_owner, _name);
            CommitsViewModel = new RepositoryCommitsViewModel(_owner,_name);

            this.RefreshInternal();
        }

        public RepositoryViewModel() : base(null, null)
        {
        }

        public override async Task Refresh()
        {
            Repository = await GitHubClient.Repository.Get(_owner, _name);
            await  Task.WhenAll(IssuesViewModel.Refresh(), PullRequestsViewModel.Refresh(), CommitsViewModel.Refresh());
        }
    }
}
