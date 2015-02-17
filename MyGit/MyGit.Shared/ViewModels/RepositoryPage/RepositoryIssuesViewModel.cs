using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Octokit;

namespace MyGit.ViewModels.RepositoryPage
{
    public class RepositoryIssuesViewModel : BaseRepoViewModel
    {
        private IEnumerable<Issue> _issues;

        public IEnumerable<Issue> Issues
        {
            get
            {
                return _issues;
            }
            set
            {
                _issues = value;
                OnPropertyChanged();
            }
        }

        protected async override Task RefreshInternal()
        {
            Issues = (await GitHubClient.Issue.GetForRepository(this.Owner, this.Repo)).OrderByDescending(i => i.CreatedAt);
        }

        public RepositoryIssuesViewModel(string owner, string repo) : base(owner,repo)
        {
        }
    }
}
