using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using MyGit.Services;
using Octokit;

namespace MyGit.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly IGitHubClient GitHubClient;
        protected readonly INavigationService NavigationService;

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
            NavigationService = App.Container.Resolve<INavigationService>();
        }

        public virtual async Task Refresh()
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

        public DelegateCommand GoBack
        {
            get
            {
                return new DelegateCommand(App.Frame.GoBack, () => App.Frame.CanGoBack);
            }
        }

        public bool IsWindowsPhone
        {
            get
            {
#if WINDOWS_PHONE_APP
                return true;
#endif
                return false;
            }
        }
    }
}
