using System;
using System.Threading.Tasks;
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
            var currentUser = new User(null, null, null, 0, null, DateTimeOffset.UtcNow, 0, null, 0, 0, null, null, 0, 0,
                null, null, "mock user", 0, null, 0, 0, 0, null, false);
            GitHubClientMock.Setup(m => m.User.Current()).Returns(() => Task.FromResult(currentUser));

            var vm = new UserDetailsViewModel();
            await vm.Refresh();

            GitHubClientMock.Verify(m => m.User.Current(), Times.AtLeastOnce());
            Assert.AreEqual("mock user", vm.User.Name);
        }
    }
}
