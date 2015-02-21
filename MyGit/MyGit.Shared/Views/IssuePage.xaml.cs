using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;
using MyGit.ViewModels.IssuePage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyGit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IssuePage : Page
    {
        public IssuePage()
        {
            this.InitializeComponent();
        }

        public class IssuePageParameters
        {
            public string Owner { get; set; }
            public string Repo { get; set; }
            public int Number { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = e.Parameter as IssuePageParameters;
            this.DataContext = new IssueViewModel(parameters.Repo, parameters.Number, parameters.Owner);
        }

        private void ScrollToBottomButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.ScrollableHeight); 
        }

        private void ScrollToTopButton_Click(object sender, RoutedEventArgs e)
        {
            MainScrollViewer.ScrollToVerticalOffset(0); 
        }
    }
}
