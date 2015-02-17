using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Octokit;

namespace MyGit.ViewModels.MainPage
{
    public class UserIssuesViewModel : BaseViewModel
    {
        public ObservableCollection<Issue> AssignedIssues { get; set; }
        public ObservableCollection<Issue> SubscribedIssues { get; set; }

        public UserIssuesViewModel()
        {
            AssignedIssues = new ObservableCollection<Issue>();
            SubscribedIssues = new ObservableCollection<Issue>();
        }

        protected override async Task RefreshInternal()
        {
            AssignedIssues.Clear();
            SubscribedIssues.Clear();

            (await GitHubClient.Issue.GetAllForCurrent(new IssueRequest
            {
                Filter = IssueFilter.Assigned,
                State = ItemState.Open
            })).ForEach(i => AssignedIssues.Add(i));
            (await GitHubClient.Issue.GetAllForCurrent(new IssueRequest
            {
                Filter = IssueFilter.Subscribed,
                State = ItemState.Open
            }))
            .Where(i => AssignedIssues.All(a => a.HtmlUrl != i.HtmlUrl))
            .ForEach(i => SubscribedIssues.Add(i));
        }
    }
}
