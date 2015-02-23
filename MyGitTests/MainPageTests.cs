using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Moq;
using MyGit.ViewModels;
using MyGit.ViewModels.IssuePage;
using MyGit.ViewModels.MainPage;
using MyGit.Views;
using NUnit.Framework;
using Octokit;

namespace MyGitTests
{
    [TestFixture]
    public class MainPageTests : TestBase
    {
        [SetUp]
        public new void Init()
        {
            GitHubClientMock.Setup(m => m.User.Current()).Returns(() => Task.FromResult(new User
            {
                Name = "Bob"
            }));
            GitHubClientMock.Setup(m => m.Issue.GetAllForCurrent(It.IsAny<IssueRequest>()))
                .Returns(() => Task.FromResult(new List<Issue>() as IReadOnlyList<Issue>));
            GitHubClientMock.Setup(m => m.Repository.GetAllForCurrent())
                .Returns(() => Task.FromResult(new List<Repository>() as IReadOnlyList<Repository>));
            GitHubClientMock.Setup(m => m.Activity.Starring.GetAllForCurrent())
                 .Returns(() => Task.FromResult(new List<Repository>() as IReadOnlyList<Repository>));
            GitHubClientMock.Setup(m => m.Activity.Watching.GetAllForCurrent())
                .Returns(() => Task.FromResult(new List<Repository>() as IReadOnlyList<Repository>));
            GitHubClientMock.Setup(m => m.Notification.GetAllForCurrent())
                .Returns(() => Task.FromResult(new List<Notification>() as IReadOnlyList<Notification>));
            GitHubClientMock.Setup(m => m.Activity.Events.GetUserPerformed(It.IsAny<string>()))
                .Returns(() => Task.FromResult(new List<Activity>() as IReadOnlyList<Activity>));
            GitHubClientMock.Setup(m => m.Activity.Events.GetUserReceived(It.IsAny<string>()))
                .Returns(() => Task.FromResult(new List<Activity>() as IReadOnlyList<Activity>));
        }
        [Test]
        public async void ShouldGetCurrentUserOnLoad()
        {
            var vm = new MainViewModel();
            // once in main vm, once in notifications vm
            GitHubClientMock.Verify(m => m.User.Current(), Times.Exactly(2));
        }

        [Test]
        public async void RefreshShouldRefreshChildViewModels()
        {
            var main = new MainViewModel();

            var newsMock = new Mock<NewsViewModel>();
            var issuesMock = new Mock<UserIssuesViewModel>();
            var notificationsMock = new Mock<NotificationsViewModel>();
            var reposMock = new Mock<UserRepositoriesViewModel>();

            newsMock.Setup(m => m.Refresh()).Returns(Task.Run(() => { }));
            issuesMock.Setup(m => m.Refresh()).Returns(Task.Run(() => { }));
            notificationsMock.Setup(m => m.Refresh()).Returns(Task.Run(() => { }));
            reposMock.Setup(m => m.Refresh()).Returns(Task.Run(() => { }));

            main.NewsViewModel = newsMock.Object;
            main.IssuesViewModel = issuesMock.Object;
            main.NotificationsViewModel = notificationsMock.Object;
            main.ReposViewModel = reposMock.Object;

            await main.Refresh();
            
            newsMock.Verify(m => m.Refresh(), Times.Once());
            issuesMock.Verify(m => m.Refresh(), Times.Once());
            notificationsMock.Verify(m => m.Refresh(), Times.Once());
            reposMock.Verify(m => m.Refresh(), Times.Once());
        }

        [Test]
        public async void RefreshCommandShouldCallRefresh()
        {
            var mock = new Mock<MainViewModel>
            {
                CallBase = true
            };
            var vm = mock.Object;
            mock.Verify(m => m.Refresh(), Times.Once());
            await vm.RefreshCommand.Execute();
            mock.Verify(m => m.Refresh(), Times.Exactly(2));
        }

        [Test]
        public async void NavigationCommandsShouldCallNavigationService()
        {
            var vm = new MainViewModel();
            await vm.GoToSearch.Execute();
            MockNavigationService.Verify(ns => ns.Navigate(typeof(SearchPage), null), Times.Once());
            await vm.GoToUserDetails.Execute();
            MockNavigationService.Verify(ns => ns.Navigate(typeof(UserDetailsPage), null), Times.Once());
            await vm.Logout.Execute();
            MockNavigationService.Verify(ns => ns.Navigate(typeof(LoginPage), null), Times.Once());
        }

        [Test]
        public async void NotificationsVMShouldGetAllNotifications()
        {
            GitHubClientMock.Setup(m => m.Notification.GetAllForCurrent(It.IsAny<NotificationsRequest>())).Returns(() => Task.FromResult(new List<Notification>{new Notification()} as IReadOnlyList<Notification>));

            var vm = new NotificationsViewModel();
            await vm.Refresh();
            GitHubClientMock.Verify(m => m.Notification.GetAllForCurrent(It.Is<NotificationsRequest>(nr => nr.All)), Times.Once());
            Assert.AreEqual(1, vm.Notifications.Count());
        }

        [Test]
        public async void NewsVMShouldGetAllReceivedAndPerformedEvents()
        {
            GitHubClientMock.Setup(m => m.Activity.Events.GetUserPerformed(It.IsAny<string>())).Returns(() => Task.FromResult(new List<Activity> { new Activity() } as IReadOnlyList<Activity>));
            GitHubClientMock.Setup(m => m.Activity.Events.GetUserReceived(It.IsAny<string>())).Returns(() => Task.FromResult(new List<Activity> { new Activity() } as IReadOnlyList<Activity>));
            GitHubClientMock.Setup(m => m.User.Current()).Returns(() => Task.FromResult(new User
            {
                Login = "login"
            }));

            var vm = new NewsViewModel();
            await vm.Refresh();
            GitHubClientMock.Verify(m => m.Activity.Events.GetUserPerformed("login"), Times.Once());
            GitHubClientMock.Verify(m => m.Activity.Events.GetUserReceived("login"), Times.Once());
            Assert.AreEqual(2, vm.NewsItems.Count());
        }

        [Test]
        public async void RepoVMShouldGetOwnedStarredAndWatched()
        {
            GitHubClientMock.Setup(m => m.Repository.GetAllForCurrent())
                .Returns(() => Task.FromResult(new List<Repository> {new Repository()} as IReadOnlyList<Repository>));
            GitHubClientMock.Setup(m => m.Activity.Starring.GetAllForCurrent())
                .Returns(() => Task.FromResult(new List<Repository> { new Repository() } as IReadOnlyList<Repository>));
            GitHubClientMock.Setup(m => m.Activity.Watching.GetAllForCurrent())
                .Returns(() => Task.FromResult(new List<Repository> { new Repository() } as IReadOnlyList<Repository>));

            var vm = new UserRepositoriesViewModel();
            await vm.Refresh();
            GitHubClientMock.Verify(m => m.Repository.GetAllForCurrent(), Times.Once());
            GitHubClientMock.Verify(m => m.Activity.Starring.GetAllForCurrent(), Times.Once());
            GitHubClientMock.Verify(m => m.Activity.Watching.GetAllForCurrent(), Times.Once());
            Assert.AreEqual(1, vm.OwnedRepos.Count);
            Assert.AreEqual(1, vm.StarredRepos.Count);
            Assert.AreEqual(1, vm.WatchedRepos.Count);
        }

        [Test]
        public async void IssuesVMShouldGetAssignedAndSubscribed()
        {
            int i = 0;
            GitHubClientMock.Setup(m => m.Issue.GetAllForCurrent(It.IsAny<IssueRequest>()))
                .Returns(() => Task.FromResult(new List<Issue>{ new Issue
                {
                    HtmlUrl = new Uri(string.Format("http://www.google.com/{0}", i++))
                } } as IReadOnlyList<Issue>));

            var vm = new UserIssuesViewModel();
            await vm.Refresh();
            GitHubClientMock.Verify(m => m.Issue.GetAllForCurrent(It.Is<IssueRequest>(r => r.Filter == IssueFilter.Assigned)), Times.Once());
            GitHubClientMock.Verify(m => m.Issue.GetAllForCurrent(It.Is<IssueRequest>(r => r.Filter == IssueFilter.Subscribed)), Times.Once());
            Assert.AreEqual(1, vm.AssignedIssues.Count);
            Assert.AreEqual(1, vm.SubscribedIssues.Count);
        }

        [Test]
        public async void SubscribedIssuesShouldNotIncludeAssigned()
        {
            GitHubClientMock.Setup(m => m.Issue.GetAllForCurrent(It.IsAny<IssueRequest>()))
                .Returns(() => Task.FromResult(new List<Issue>{ new Issue
                {
                    HtmlUrl = new Uri("http://www.google.com")
                } } as IReadOnlyList<Issue>));

            var vm = new UserIssuesViewModel();
            await vm.Refresh();
            Assert.AreEqual(1, vm.AssignedIssues.Count);
            Assert.AreEqual(0, vm.SubscribedIssues.Count);
        }
    }
}
