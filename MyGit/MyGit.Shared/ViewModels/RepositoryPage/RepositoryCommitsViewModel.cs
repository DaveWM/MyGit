using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.RepositoryPage
{
    public class RepositoryCommitsViewModel : BaseRepoViewModel
    {
        private IEnumerable<GitHubCommit> _commits;

        public IEnumerable<GitHubCommit> Commits
        {
            get { return _commits; }
            set
            {
                _commits = value;
                OnPropertyChanged();
            }
        } 
        public RepositoryCommitsViewModel(string owner, string repo) : base(owner, repo)
        {
        }

        protected async override Task RefreshInternal()
        {
            Commits = await GitHubClient.Repository.Commits.GetAll(Owner, Repo, new CommitRequest
            {
                Since = DateTimeOffset.UtcNow.AddMonths(-1)
            });
        }
    }
}
