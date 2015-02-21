using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;
using MyGit.ViewModels.PullRequestPage;

namespace MyGit.Views
{
    public partial class PullRequestPage
    {
        private void BaseRepoLink_OnClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            var repo = (this.DataContext as PullRequestViewModel).PR.Base.Repository;
            var repoParams = new RepositoryPage.RepositoryPageParameters
            {
                Name = repo.Name,
                Owner = repo.Owner.Login
            };
            App.Frame.Navigate(typeof(RepositoryPage), repoParams);
        }

        private void HeadRepoLink_OnClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            var repo = (this.DataContext as PullRequestViewModel).PR.Head.Repository;
            var repoParams = new RepositoryPage.RepositoryPageParameters
            {
                Name = repo.Name,
                Owner = repo.Owner.Login
            };
            App.Frame.Navigate(typeof (RepositoryPage), repoParams);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var args = e.Parameter as PullRequestParams;
            this.DataContext = new PullRequestViewModel(args.Repo, args.Owner, args.Number);
        }

        public class PullRequestParams
        {
            public string Repo { get; set; }
            public string Owner { get; set; }
            public int Number { get; set; }
        }
    }
}
