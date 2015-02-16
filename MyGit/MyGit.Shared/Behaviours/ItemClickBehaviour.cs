using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MyGit.Views;
using Octokit;

namespace MyGit.Behaviours
{
    public class ItemClickBehaviour : DependencyObject
    {
        public static readonly DependencyProperty IsClickable = DependencyProperty.RegisterAttached("ShouldGoToRepo", typeof (bool), typeof (ItemClickBehaviour), new PropertyMetadata(false));

        public static bool GetShouldGoToRepo(Grid grid)
        {
            return (bool) grid.GetValue(IsClickable);
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
    }
}
