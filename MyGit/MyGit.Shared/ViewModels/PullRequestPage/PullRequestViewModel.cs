using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.PullRequestPage
{
    public class PullRequestViewModel : BaseViewModel
    {
        private readonly string _repo;
        private readonly string _owner;
        private readonly int _number;

        private PullRequest _pr;

        public PullRequest PR
        {
            get { return _pr; }
            set
            {
                _pr = value;
                OnPropertyChanged();
            }
        }

        private PRCommentsViewModel _commentsViewModel;

        public PRCommentsViewModel CommentsViewModel
        {
            get { return _commentsViewModel; }
            set
            {
                _commentsViewModel = value;
                OnPropertyChanged();
            }
        }

        private PRCommitsViewModel _commitsViewModel;

        public PRCommitsViewModel CommitsViewModel
        {
            get { return _commitsViewModel; }
            set
            {
                _commitsViewModel = value;
                OnPropertyChanged();
            }
        }

        public PullRequestViewModel(string repo, string owner, int number)
        {
            _repo = repo;
            _owner = owner;
            _number = number;

            CommentsViewModel = new PRCommentsViewModel(repo,owner,number);
            CommitsViewModel = new PRCommitsViewModel(repo, owner, number);

            this.Refresh();
        }

        public PullRequestViewModel()
        {
            
        }

        protected async override Task RefreshInternal()
        {
            PR = await GitHubClient.PullRequest.Get(_owner, _repo, _number);
            await Task.WhenAll(CommentsViewModel.Refresh(), CommitsViewModel.Refresh());
        }
    }
}
