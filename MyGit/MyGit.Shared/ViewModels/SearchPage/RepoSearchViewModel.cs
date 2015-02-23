using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Octokit;

namespace MyGit.ViewModels
{
    public class RepoSearchViewModel : BaseViewModel
    {
        private string _searchString = String.Empty;

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged();
                Refresh();
            }
        }

        public DelegateCommand SearchCommand
        {
            get
            {
                return new DelegateCommand(() => this.Refresh());
            }
        }

        private IEnumerable<Repository> _searchResults = new List<Repository>();

        public IEnumerable<Repository> SearchResults
        {
            get { return _searchResults; }
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        } 

        protected async override Task RefreshInternal()
        {
            if (!String.IsNullOrWhiteSpace(SearchString))
            {
                var results = await GitHubClient.Search.SearchRepo(new SearchRepositoriesRequest(this.SearchString));
                SearchResults = results.Items;
            }
        }
    }
}
