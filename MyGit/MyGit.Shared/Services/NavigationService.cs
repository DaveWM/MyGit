using System;

namespace MyGit.Services
{
    public interface INavigationService
    {
        void Navigate(Type page, object args = null);
    }

    public class NavigationService : INavigationService
    {
        public void Navigate(Type page, object args = null)
        {
            App.Frame.Navigate(page, args);
        }
    }
}
