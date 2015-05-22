using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using MyGit.Services;

namespace MyGit.ViewModels.LoginPage
{
    public class LoginViewModel
    {
        private readonly ILoginService _loginService;
        public LoginViewModel()
        {
            _loginService = App.Container.Resolve<ILoginService>();
            _loginService.Logout();
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
                    if(App.Frame != null)
                        App.Frame.Navigate(typeof(Views.MainPage));
#endif
                });
            }
        }
    }
}
