using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using Octokit;

namespace MyGit.ViewModels
{
    public class RepositoryViewModel : INotifyPropertyChanged
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


        private bool _isLoading;

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

        private readonly IGitHubClient _gitHubClient;

        private readonly string _owner;
        private readonly string _name;

        public RepositoryViewModel()
        {
            _gitHubClient = App.Container.Resolve<IGitHubClient>();
        }
        public RepositoryViewModel(string owner, string name) : this()
        {
            _owner = owner;
            _name = name;

            Refresh();
        }

        private async void Refresh()
        {
            IsLoading = true;

            Repository = await _gitHubClient.Repository.Get(_owner, _name);

            IsLoading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
