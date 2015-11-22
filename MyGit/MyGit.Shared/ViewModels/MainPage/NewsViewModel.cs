using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.MainPage
{
    public class NewsViewModel : BaseViewModel
    {
        private IEnumerable<Activity> _newsItems;

        public IEnumerable<Activity> NewsItems
        {
            get { return _newsItems; }
            set
            {
                _newsItems = value;
                OnPropertyChanged();
            }
        }

        protected override async Task RefreshInternal()
        {
            NewsItems = null;
            var user = await GitHubClient.User.Current();
            var receivedTask = GitHubClient.Activity.Events.GetAllUserReceived(user.Login);
            var performedTask = GitHubClient.Activity.Events.GetAllUserPerformed(user.Login);
            var received = await receivedTask;
            var performed = await performedTask;
            NewsItems = received.Union(performed).OrderByDescending(a => a.CreatedAt);
        }
    }
}
