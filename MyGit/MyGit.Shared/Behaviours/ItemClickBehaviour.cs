using System.Text.RegularExpressions;
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

                    switch (notification.Subject.Type.ToLower())
                    {
                        case "issue":
                            // notification doesn't get issue number, have to parse from url
                            var numberRegex = new Regex(@"(?<=\/)[^\/]*\Z");
                            var number = int.Parse(numberRegex.Match(notification.Subject.Url).Value);
                            var issueParams = new IssuePage.IssuePageParameters
                            {
                                Number = number,
                                Owner = notification.Repository.Owner.Login,
                                Repo = notification.Repository.Name
                            };
                            App.Frame.Navigate(typeof (IssuePage), issueParams);
                            break;
                    }
                };
            }
        }


        public static readonly DependencyProperty NavigateToIssue = DependencyProperty.RegisterAttached(
            "ShouldGoToIssue", typeof(bool), typeof(ItemClickBehaviour), new PropertyMetadata(false));

        public static bool GetShouldGoToIssue(Grid grid)
        {
            return (bool)grid.GetValue(NavigateToIssue);
        }

        public static void SetShouldGoToIssue(Grid grid, bool value)
        {
            if (value)
            {
                grid.Tapped += (s, e) =>
                {
                    var issue = ((FrameworkElement)s).DataContext as Issue;

                    // issue doesn't have repo info in the api, even though we need the repo name and owner to get the single issue ಠ_ಠ
                    // have to parse repo name from issue url ಠ_ಠ
                    var nameRegex = new Regex(@"(?<=\/)[^\/]*(?=\/issues)");
                    var repoName = nameRegex.Match(issue.Url.AbsolutePath).Value;
                    var ownerRegex = new Regex(@"(?<=\/)[^\/]*(?=\/[^\/]*\/issues)");
                    var owner = ownerRegex.Match(issue.Url.AbsolutePath).Value;

                    App.Frame.Navigate(typeof(IssuePage), new IssuePage.IssuePageParameters
                    {
                        Repo = repoName,
                        Number = issue.Number,
                        Owner = owner
                    });
                };
            }
        }
    }
}
