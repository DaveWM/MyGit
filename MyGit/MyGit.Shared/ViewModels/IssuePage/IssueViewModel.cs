using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Octokit;

namespace MyGit.ViewModels.IssuePage
{
    public class IssueViewModel : BaseViewModel
    {
        private readonly int _number;
        private readonly string _repoName;
        private readonly string _owner;

        public string RepoFullName
        {
            get
            {
                return string.Format("{0}/{1}", _owner, _repoName);
            }
        }

        public DelegateCommand GoToRepoCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    App.Frame.Navigate(typeof(Views.RepositoryPage), new Views.RepositoryPage.RepositoryPageParameters
                    {
                        Name = _repoName,
                        Owner = _owner
                    });
                });
            }
        }

        private Issue _issue;

        public Issue Issue
        {
            get { return _issue; }
            set
            {
                _issue = value;
                OnPropertyChanged();
            }
        }

        private IssueHistoryViewModel _historyViewModel;

        public IssueHistoryViewModel HistoryViewModel
        {
            get { return _historyViewModel; }
            set
            {
                _historyViewModel = value;
                OnPropertyChanged();
            }
        }

         public IssueViewModel()
         {
         }

        public IssueViewModel(string repoName, int number, string owner)
        {
            _repoName = repoName;
            _number = number;
            _owner = owner;

            HistoryViewModel = new IssueHistoryViewModel(owner, repoName, number);

            Refresh();
        }
        protected async override Task RefreshInternal()
        {
            Issue = await GitHubClient.Issue.Get(_owner, _repoName, _number);
            await HistoryViewModel.Refresh();
        }
    }
}
