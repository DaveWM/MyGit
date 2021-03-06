﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Unity;
using MyGit.Services;
using MyGit.Views;
using Octokit;
using Application = Windows.UI.Xaml.Application;
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
#endif

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace MyGit
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        private readonly ILoginService _loginService;
        private readonly IGitHubClient _gitHubClient;

        public static IUnityContainer Container = new UnityContainer();

        public static Frame Frame
        {
            get
            {
                if (Window.Current == null || Window.Current.Content == null)
                    return null;
                return Window.Current.Content as Frame;
            }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            var appName = "MyGit";
#if WINDOWS_PHONE_APP
            appName = "MyGit_Phone";
#endif
            // have to make use a custom connection type otherwise all the http responses get cached (-_-)
            var connection = new Connection(new ProductHeaderValue(appName), new NonCachedHttpClient());
            _gitHubClient = new GitHubClient(connection);
            Container.RegisterInstance<IGitHubClient>(_gitHubClient);
            Container.RegisterInstance<ILocalStorageService>(new LocalStorageService());
            Container.RegisterInstance<INavigationService>(new NavigationService());
            _loginService = new LoginService();
            Container.RegisterInstance<ILoginService>(_loginService);

            this.UnhandledException += async (s, ea) =>
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    new MessageDialog("Please check your internet connection and try again").ShowAsync();
                    ea.Handled = true;
                }
                else
                {
                    try
                    {
                        new MessageDialog(ea.Message).ShowAsync();
                    }
                    catch (Exception e) {
                    }
                    Frame.Navigate(typeof (LoginPage));
                    ea.Handled = true;
                }
            };

#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed += (s, e) =>
            {
                if (Frame == null || !Frame.CanGoBack) return;

                e.Handled = true;
                Frame.GoBack();
            };
#endif
        }


        private async void RegisterBackgroundTask()
        {
            const string taskName = "NotificationsBackgroundTask";

            var alreadyRegistered = BackgroundTaskRegistration.AllTasks.Any((t) => t.Value.Name == taskName);
            if (alreadyRegistered)
            {
                var task = BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == taskName);
                task.Value.Unregister(true);
            }

            var permission = await BackgroundExecutionManager.RequestAccessAsync();
            if (permission == BackgroundAccessStatus.Denied) return;

            var builder = new BackgroundTaskBuilder
            {
                Name = taskName,
                TaskEntryPoint = string.Format("NotificationsBackgroundTask.{0}", taskName)
            };

            builder.SetTrigger(new TimeTrigger(30, false));
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            builder.Register();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            RegisterBackgroundTask();
            BegUserToBuyMe();
#if DEBUG
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                var firstPageType = _loginService.IsLoggedIn ? typeof (MainPage) : typeof (LoginPage);
                if (!rootFrame.Navigate(firstPageType, e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }
        private readonly LicenseInformation _licenseInfo = CurrentApp.LicenseInformation;
        private void BegUserToBuyMe()
        {
            if (_licenseInfo.IsTrial)
            {
                var dialog =
                    new MessageDialog(
                        "You're currently using the trial version of MyGit. If you like it, why not buy it (or at least rate it)? I'm going to keep pestering you until you do :)",
                        "Buy Me");
                dialog.Commands.Add(new UICommand("ok, fine...", command =>
                {
                    Launcher.LaunchUriAsync(new Uri("ms-windows-store:navigate?appid=" + CurrentApp.AppId));
                }));
                dialog.Commands.Add(new UICommand("nope"));
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                dialog.ShowAsync();
            }
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ??
                                           new TransitionCollection() {new NavigationThemeTransition()};
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            //Check if this is a continuation - only happens on phone
#if WINDOWS_PHONE_APP
            var wabArgs = args as WebAuthenticationBrokerContinuationEventArgs;
            if (wabArgs != null)
            {
                var responseUrl = wabArgs.WebAuthenticationResult.ResponseData;
                var regex = new Regex(@"(?<=\?code=).*");
                var code = regex.Match(responseUrl).Value;

                if (!String.IsNullOrEmpty(code))
                {
                    await _loginService.SetTokenFromCode(code);
                    Frame.Navigate(typeof (MainPage));
                }
            }
#endif
            base.OnActivated(args);
        }
    }
}