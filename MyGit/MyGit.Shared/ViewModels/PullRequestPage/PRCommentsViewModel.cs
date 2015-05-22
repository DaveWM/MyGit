using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.PullRequestPage
{
    public class PRCommentsViewModel : BaseViewModel
    {
        private readonly string _repo;
        private readonly string _owner;
        private readonly int _number;

        private IEnumerable<IssueComment> _comments;

        public IEnumerable<IssueComment> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                OnPropertyChanged();
            }
        }

        public PRCommentsViewModel(string repo, string owner, int number)
        {
            _repo = repo;
            _owner = owner;
            _number = number;
        }

        protected async override Task RefreshInternal()
        {
            Comments = await GitHubClient.Issue.Comment.GetAllForIssue(_owner, _repo, _number);
        }
    }
}
