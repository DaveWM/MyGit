using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.IssuePage
{
    public class IssueHistoryViewModel : BaseViewModel
    {
        private readonly string _owner;
        private readonly string _repoName;
        private readonly int _number;

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

        public IssueHistoryViewModel(string owner, string repoName, int number)
        {
            _owner = owner;
            _repoName = repoName;
            _number = number;
        }

        protected async override Task RefreshInternal()
        {
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
                // any more than this crashes the app on my lumia 520 :(
                .Take(100)
                .Reverse()
                .Select(h => h.item);
        }
    }
}
