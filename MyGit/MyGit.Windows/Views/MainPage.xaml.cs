using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyGit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void RepoChoiceBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            // this is completely retarded, but apparently comboboxes don't update their bindings until they're opened
            // well done xaml
            var combo = sender as ComboBox;
            combo.IsDropDownOpen = true;
            combo.IsDropDownOpen = false;
        }

        private void IssueTypeChoiceBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            var combo = sender as ComboBox;
            combo.IsDropDownOpen = true;
            combo.IsDropDownOpen = false;
        }
    }
}
