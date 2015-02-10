using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using Octokit;

namespace MyGit.ViewModels
{
    public class UserRepositoriesViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Repository> _ownedRepos;
        public IEnumerable<Repository> OwnedRepos
        {
            get { return _ownedRepos; }
            set
            {
                _ownedRepos = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Repository> _starredRepos;
        public IEnumerable<Repository> StarredRepos
        {
            get { return _starredRepos; }
            set
            {
                _starredRepos = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Repository> _watchedRepos;
        public IEnumerable<Repository> WatchedRepos
        {
            get { return _watchedRepos; }
            set
            {
                _watchedRepos = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, IEnumerable<Repository>> RepoListChoices
        {
            get
            {
                return new Dictionary<string, IEnumerable<Repository>>
                {
                    {"Owned", OwnedRepos},
                    {"Watched", WatchedRepos},
                    {"Starred", StarredRepos}
                };
            }
            set
            {
                throw new NotImplementedException();
            }
        } 

        private readonly IGitHubClient _gitHubClient;

        public UserRepositoriesViewModel()
        {
            _gitHubClient = App.Container.Resolve<IGitHubClient>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task Refresh()
        {
            IsLoading = true;

            OwnedRepos = null;
            StarredRepos = null;
            WatchedRepos = null;

            var tasks = new List<Task>
            {
                _gitHubClient.Repository.GetAllForCurrent()
                    .ContinueWith(async task => OwnedRepos = (await task).OrderByDescending(r => r.UpdatedAt).ToList()),
                _gitHubClient.Activity.Starring.GetAllForCurrent()
                    .ContinueWith(async task => StarredRepos = (await task).OrderByDescending(r => r.UpdatedAt).ToList()),
                _gitHubClient.Activity.Watching.GetAllForCurrent()
                    .ContinueWith(async task => WatchedRepos = (await task).OrderByDescending(r => r.UpdatedAt).ToList())
            };

            await Task.WhenAll(tasks);
            IsLoading = false;
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
