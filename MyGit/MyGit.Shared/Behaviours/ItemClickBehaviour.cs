using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.Unity;
using MyGit.Views;
using Octokit;

namespace MyGit.Behaviours
{
    public class ItemClickBehaviour : DependencyObject
    {
        public static readonly DependencyProperty NavigateToRepo = DependencyProperty.RegisterAttached(
            "ShouldGoToRepo", typeof (bool), typeof (ItemClickBehaviour), new PropertyMetadata(false));

        public static bool GetShouldGoToRepo(Grid grid)
        {
            return (bool) grid.GetValue(NavigateToRepo);
        }

        public static void SetShouldGoToRepo(Grid grid, bool value)
        {
            if (value)
            {
                grid.Tapped += (s, e) =>
                {
                    var repo = ((FrameworkElement) s).DataContext as Repository;
                    App.Frame.Navigate(typeof (RepositoryPage), new RepositoryPage.RepositoryPageParameters
                    {
                        Name = repo.Name,
                        Owner = repo.Owner.Login
                    });
                };
            }
        }

        public static readonly DependencyProperty NavigateToNotification =
            DependencyProperty.RegisterAttached("ShouldGoToNotification", typeof (bool), typeof (ItemClickBehaviour),
                new PropertyMetadata(false));
        
        public static bool GetShouldGoToNotification(Grid grid)
        {
            return (bool) grid.GetValue(NavigateToNotification);
        }

        public static void SetShouldGoToNotification(Grid grid, bool value)
        {
            if (value)
            {
                grid.Tapped += (s, e) =>
                {
                    var notification = ((FrameworkElement) s).DataContext as Notification;
                    var client = App.Container.Resolve<IGitHubClient>();

                    if (notification.Unread)
                    {
                        client.Notification.MarkAsRead(int.Parse(notification.Id));
                    }
                };
            }
        }
    }
}
