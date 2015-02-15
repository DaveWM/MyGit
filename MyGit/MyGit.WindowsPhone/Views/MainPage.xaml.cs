﻿using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MyGit.ViewModels.MainPage;
using Octokit;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

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

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            (this.DataContext as MainViewModel).ReposViewModel.OpenRepo.Execute(e.ClickedItem as Repository);
        }
    }
}
