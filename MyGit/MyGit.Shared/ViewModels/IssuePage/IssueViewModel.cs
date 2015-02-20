using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.IssuePage
{
    public class IssueViewModel : BaseViewModel
    {
        private readonly int _number;
        private readonly string _repoName;
        private readonly string _owner;

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

        private IEnumerable<IssueComment> _comments;

        public IEnumerable<IssueComment> Comments
        {
            get
            {
                return _comments;
            }
            set
            {
                _comments = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<EventInfo> _issueEvents;

        public IEnumerable<EventInfo> IssueEvents
        {
            get { return _issueEvents; }
            set
            {
                _issueEvents = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<object> _issueHistory;

        public IEnumerable<object> IssueHistory
        {
            get { return _issueHistory; }
            set
            {
                _issueHistory = value;
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

            Refresh();
        }
        protected async override Task RefreshInternal()
        {
            Issue = await GitHubClient.Issue.Get(_owner, _repoName, _number);
            Comments = await GitHubClient.Issue.Comment.GetForIssue(_owner, _repoName, _number);
            IssueEvents = await GitHubClient.Issue.Events.GetForIssue(_owner, _repoName, _number);

            IssueHistory = Comments.Select(c => new
            {
                date = c.CreatedAt,
                item = (object)c
            })
                .Union(IssueEvents.Select(i => new
                {
                    date = i.CreatedAt,
                    item = (object)i
                }))
                .OrderByDescending(h => h.date)
                .Take(100)
                .Reverse()
                .Select(h => h.item);
        }
    }
}
