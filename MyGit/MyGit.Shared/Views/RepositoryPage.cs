using Windows.UI.Xaml.Documents;
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
    }
}
