using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using MyGit.Views;
using Octokit;

namespace MyGit.ViewModels
{
    public class UserRepositoriesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Repository> OwnedRepos { get; set; }

        public ObservableCollection<Repository> StarredRepos { get; set; }

        public ObservableCollection<Repository> WatchedRepos { get; set; }

        private readonly IGitHubClient _gitHubClient;

        public UserRepositoriesViewModel()
        {
            _gitHubClient = App.Container.Resolve<IGitHubClient>();

            OwnedRepos = new ObservableCollection<Repository>();
            StarredRepos = new ObservableCollection<Repository>();
            WatchedRepos = new ObservableCollection<Repository>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task Refresh()
        {
            IsLoading = true;

            OwnedRepos.Clear();
            StarredRepos.Clear();
            WatchedRepos.Clear();

            (await _gitHubClient.Repository.GetAllForCurrent()).OrderByDescending(r => r.UpdatedAt).ForEach(r => OwnedRepos.Add(r));
            (await _gitHubClient.Activity.Starring.GetAllForCurrent()).OrderByDescending(r => r.StargazersCount).ForEach(r => StarredRepos.Add(r));
            (await _gitHubClient.Activity.Watching.GetAllForCurrent()).OrderByDescending(r => r.StargazersCount).ForEach(r => WatchedRepos.Add(r));

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


        public DelegateCommand<Repository> OpenRepo
        {
            get
            {
                return new DelegateCommand<Repository>(repo =>
                {
                    App.Frame.Navigate(typeof(RepositoryPage), new RepositoryPage.RepositoryPageParameters
                    {
                        Name = repo.Name,
                        Owner = repo.Owner.Login
                    });
                });
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
