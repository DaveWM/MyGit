using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;
using MyGit.ViewModels.RepositoryPage;

namespace MyGit.Views
{
    public partial class RepositoryPage
    {
        public class RepositoryPageParameters
        {
            public string Owner;
            public string Name;
        }

        private void ForkedFromLink_OnClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            var parent = (this.DataContext as RepositoryViewModel).Repository.Parent;
            App.Frame.Navigate(typeof (RepositoryPage), new RepositoryPageParameters
            {
                Name = parent.Name,
                Owner = parent.Owner.Login
            });
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = e.Parameter as RepositoryPageParameters;
            this.DataContext = new RepositoryViewModel(parameters.Owner, parameters.Name);
        }
    }
}
