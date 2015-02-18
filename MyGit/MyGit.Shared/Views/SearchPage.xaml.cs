using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyGit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void SearchBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            // if the user presses enter, make the search box lose focus. This will cause the viewmodel to update and then run a search.
            if (e.Key == VirtualKey.Enter)
            {
                SearchButton.Focus(FocusState.Programmatic);
            }
        }
    }
}
