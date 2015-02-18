﻿using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MyGit.ViewModels.RepositoryPage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyGit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RepositoryPage : Page
    {
        public RepositoryPage()
        {
            this.InitializeComponent();
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
