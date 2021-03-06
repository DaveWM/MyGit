﻿using System.Threading.Tasks;
using Moq;
using MyGit.ViewModels.UserDetailsPage;
using NUnit.Framework;
using Octokit;

namespace MyGitTests
{
    [TestFixture]
    public class UserTests : TestBase
    {
        [Test]
        public async void TestUserLoadedCorrectly()
        {
            var currentUser = new Mock<User>();
            currentUser.SetupProperty(u => u.Name, "mock user");
            GitHubClientMock.Setup(m => m.User.Current()).Returns(() => Task.FromResult(currentUser.Object));

            var vm = new UserDetailsViewModel();
            await vm.Refresh();

            GitHubClientMock.Verify(m => m.User.Current(), Times.AtLeastOnce());
            Assert.AreEqual("mock user", vm.User.Name);
        }
    }
}
