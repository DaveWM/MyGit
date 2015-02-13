using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using MyGit.Annotations;
using Octokit;

namespace MyGit.ViewModels
{
    public class UserIssuesViewModel : INotifyPropertyChanged
    {
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

        public ObservableCollection<Issue> AssignedIssues { get; set; }
        public ObservableCollection<Issue> SubscribedIssues { get; set; }
 

        private readonly IGitHubClient _gitHubClient;
        public UserIssuesViewModel()
        {
            _gitHubClient = App.Container.Resolve<IGitHubClient>();
            AssignedIssues = new ObservableCollection<Issue>();
            SubscribedIssues = new ObservableCollection<Issue>();
        }

        public async Task Refresh()
        {
            IsLoading = true;

            AssignedIssues.Clear();
            SubscribedIssues.Clear();

            (await _gitHubClient.Issue.GetAllForCurrent(new IssueRequest
            {
                Filter = IssueFilter.Assigned,
                State = ItemState.Open
            })).ForEach(i => AssignedIssues.Add(i));
            (await _gitHubClient.Issue.GetAllForCurrent(new IssueRequest
            {
                Filter = IssueFilter.Subscribed,
                State = ItemState.Open
            }))
            .Where(i => AssignedIssues.All(a => a.HtmlUrl != i.HtmlUrl))
            .ForEach(i => SubscribedIssues.Add(i));

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
