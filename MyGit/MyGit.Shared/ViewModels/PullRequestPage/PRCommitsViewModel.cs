using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.PullRequestPage
{
    public class PRCommitsViewModel : BaseViewModel
    {
        private readonly string _repo;
        private readonly string _owner;
        private readonly int _number;


        private IReadOnlyList<PullRequestCommit> _commits;

        public IReadOnlyList<PullRequestCommit> Commits
        {
            get { return _commits; }
            set
            {
                _commits = value;
                OnPropertyChanged();
            }
        }

        public PRCommitsViewModel(string repo, string owner, int number)
        {
            _repo = repo;
            _owner = owner;
            _number = number;
        }

        protected async override Task RefreshInternal()
        {
            Commits = (await GitHubClient.PullRequest.Commits(_owner, _repo, _number));
        }
    }
}
