﻿using System;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Octokit;

namespace MyGit.ViewModels.RepositoryPage
{
    public class RepositoryViewModel : BaseRepoViewModel
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

        private bool _isWatched;

        public bool IsWatched
        {
            get { return _isWatched; }
            set
            {
                _isWatched = value;
                OnPropertyChanged();
                ToggleWatch.RaiseCanExecuteChanged();
            }
        }

        private bool _isStarred;

        public bool IsStarred
        {
            get { return _isStarred; }
            set
            {
                _isStarred = value;
                OnPropertyChanged();
                ToggleStarred.RaiseCanExecuteChanged();
            }
        }

        private Uri _readmeUri;

        public Uri ReadmeUri
        {
            get { return _readmeUri; }
            set
            {
                _readmeUri = value;
                OnPropertyChanged();
                OnPropertyChanged("HasReadme");
            }
        }

        public bool HasReadme
        {
            get { return ReadmeUri != null; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public RepositoryIssuesViewModel IssuesViewModel { get; set; }
        public RepositoryPullRequestsViewModel PullRequestsViewModel { get; set; }
        public RepositoryCommitsViewModel CommitsViewModel { get; set; }

        private readonly string _owner;
        private readonly string _name;

        public RepositoryViewModel(string owner, string name) : base(owner, name)
        {
            _owner = owner;
            _name = name;

            IssuesViewModel = new RepositoryIssuesViewModel(_owner, _name);
            PullRequestsViewModel = new RepositoryPullRequestsViewModel(_owner, _name);
            CommitsViewModel = new RepositoryCommitsViewModel(_owner,_name);

            this.TryGetReadme();
            this.Refresh();
        }

        public RepositoryViewModel() : base(null, null)
        {
        }

        protected override async Task RefreshInternal()
        {
            Repository = await GitHubClient.Repository.Get(_owner, _name);
            await CheckSubscriptionStatus();
            await Task.WhenAll(IssuesViewModel.Refresh(), PullRequestsViewModel.Refresh(), CommitsViewModel.Refresh());
        }

        private async Task TryGetReadme()
        {
            try
            {
                var readme = await GitHubClient.Repository.Content.GetReadme(_owner, _name);
                ReadmeUri = readme.HtmlUrl;
            }
            catch (Exception e)
            {
                ReadmeUri = null;
            }
        }

        public async Task CheckSubscriptionStatus()
        {
            SubscriptionInfoUpdating = true;
            IsWatched = await GitHubClient.Activity.Watching.CheckWatched(Owner, Repo);
            IsStarred = await GitHubClient.Activity.Starring.CheckStarred(Owner, Repo);
            SubscriptionInfoUpdating = false;
        }

        private bool _subscriptionInfoUpdating = true;

        public bool SubscriptionInfoUpdating
        {
            get { return _subscriptionInfoUpdating; }
            set
            {
                _subscriptionInfoUpdating = value;
                OnPropertyChanged();
                ToggleWatch.RaiseCanExecuteChanged();
                ToggleStarred.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _toggleWatch;
        public DelegateCommand ToggleWatch
        {
            get
            {
                if (_toggleWatch == null)
                {
                    _toggleWatch = new DelegateCommand(async () =>
                    {
                        SubscriptionInfoUpdating = true;
                        if (IsWatched)
                        {
                            await
                                GitHubClient.Activity.Watching.UnwatchRepo(Owner, Repo)
                                    .ContinueWith(t => CheckSubscriptionStatus());
                        }
                        else
                        {
                            await
                                GitHubClient.Activity.Watching.WatchRepo(Owner, Repo, new NewSubscription())
                                    .ContinueWith(t => CheckSubscriptionStatus());
                        }

                        SubscriptionInfoUpdating = false;
                        IsWatched = !IsWatched;
                    },
                        () => !SubscriptionInfoUpdating);
                }
                return _toggleWatch;
            }
        }

        private DelegateCommand _toggleStarred;
        public DelegateCommand ToggleStarred
        {
            get
            {
                if (_toggleStarred == null)
                {
                    _toggleStarred = new DelegateCommand(async () =>
                    {
                        SubscriptionInfoUpdating = true;
                        if (IsStarred)
                        {
                            await
                                GitHubClient.Activity.Starring.RemoveStarFromRepo(Owner, Repo)
                                    .ContinueWith(t => CheckSubscriptionStatus());
                        }
                        else
                        {
                            await
                                GitHubClient.Activity.Starring.StarRepo(Owner, Repo)
                                    .ContinueWith(t => CheckSubscriptionStatus());
                        }

                        SubscriptionInfoUpdating = false;
                        IsStarred = !IsStarred;
                    },
                        () => !SubscriptionInfoUpdating);
                }
                return _toggleStarred;
            }
        }
    }
}
