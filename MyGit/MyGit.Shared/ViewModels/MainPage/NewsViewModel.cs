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
            var received = (await GitHubClient.Activity.Events.GetUserReceived(user.Login)).Take(100);
            //var performed = (await GitHubClient.Activity.Events.GetUserPerformed(user.Login)).Take(10);

            NewsItems = received;
            //.Union(performed).OrderByDescending(a => a.CreatedAt);
        }
    }
}
