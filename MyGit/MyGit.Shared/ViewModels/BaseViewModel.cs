using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using Octokit;

namespace MyGit.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly IGitHubClient GitHubClient;

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

         protected BaseViewModel()
        {
            GitHubClient = App.Container.Resolve<IGitHubClient>();
        }

        public async Task Refresh()
        {
            IsLoading = true;
            try
            {
                await this.RefreshInternal();
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected abstract Task RefreshInternal();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
