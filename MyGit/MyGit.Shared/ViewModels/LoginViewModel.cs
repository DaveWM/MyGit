using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using MyGit.Services;
using MyGit.Views;

namespace MyGit.ViewModels
{
    public class LoginViewModel
    {
        private readonly ILoginService _loginService;
        public LoginViewModel()
        {
            _loginService = App.Container.Resolve<ILoginService>();
            _loginService.ClearToken();
        }
        public DelegateCommand LoginCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await _loginService.Login();
                    // wp app will close and re-open, only continue if we're on windows desktop
#if WINDOWS_APP
                    App.Frame.Navigate(typeof(MainPage));
#endif
                });
            }
        }
    }
}
