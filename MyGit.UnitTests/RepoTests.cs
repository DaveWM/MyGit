using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MyGit.ViewModels.RepositoryPage;
using NUnit.Framework;
using Octokit;

namespace MyGitTests
{
    // TODO: add test for readme request - doesn't work in current version of octokit because readme constructor is sealed
    [TestFixture]
    public class RepoTests : TestBase
    {
        private const string Owner = "owner";
        private const string Repo = "repo";

        [SetUp]
        public new void Init()
        {
            GitHubClientMock.Setup(m => m.Repository.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => Task.FromResult(new Repository()));
            GitHubClientMock.Setup(m => m.Activity.Watching.CheckWatched(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => Task.FromResult(false));
            GitHubClientMock.Setup(m => m.Activity.Starring.CheckStarred(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => Task.FromResult(false));
            GitHubClientMock.Setup(m => m.PullRequest.GetAllForRepository(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult(new List<PullRequest>() as IReadOnlyList<PullRequest>));
            GitHubClientMock.Setup(m => m.Issue.GetAllForRepository(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult(new List<Issue>() as IReadOnlyList<Issue>));
            GitHubClientMock.Setup(m => m.Repository.Commits.GetAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommitRequest>())).Returns(() => Task.FromResult(new List<GitHubCommit>() as IReadOnlyList<GitHubCommit>));
        }

        [Test]
        public async void RepoViewModelShouldRefreshLoad()
        {
            var vmMock = new Mock<RepositoryViewModel> (Owner,Repo){CallBase = true};
            var obj = vmMock.Object;
            vmMock.Verify(m => m.Refresh(), Times.Once());
        }

        [Test]
        public async void RepoVMShouldRefreshChildVMs()
        {
            var vm = new RepositoryViewModel(Owner, Repo);

            var prsMock = new Mock<RepositoryPullRequestsViewModel>(Owner, Repo);
            var issuesMock = new Mock<RepositoryIssuesViewModel>(Owner, Repo);
            var commitsMock = new Mock<RepositoryCommitsViewModel>(Owner, Repo);
            prsMock.Setup(m => m.Refresh()).Returns(() => Task.Run(() => { }));
            issuesMock.Setup(m => m.Refresh()).Returns(() => Task.Run(() => { }));
            commitsMock.Setup(m => m.Refresh()).Returns(() => Task.Run(() => { }));

            vm.IssuesViewModel = issuesMock.Object;
            vm.PullRequestsViewModel = prsMock.Object;
            vm.CommitsViewModel = commitsMock.Object;
            await vm.Refresh();

            prsMock.Verify(m => m.Refresh(), Times.Once());
            prsMock.Verify(m => m.Refresh(), Times.Once());
            prsMock.Verify(m => m.Refresh(), Times.Once());
        }

        [Test]
        public async void ShouldCheckStarredAndWatchedStatusOnRefresh()
        {
            var vm = new RepositoryViewModel(Owner, Repo);
            GitHubClientMock.Verify(m => m.Activity.Starring.CheckStarred(Owner, Repo), Times.Once());
            GitHubClientMock.Verify(m => m.Activity.Watching.CheckWatched(Owner, Repo), Times.Once());
            
            // should also check on refresh
            await vm.Refresh();
            GitHubClientMock.Verify(m => m.Activity.Starring.CheckStarred(Owner, Repo), Times.Exactly(2));
            GitHubClientMock.Verify(m => m.Activity.Watching.CheckWatched(Owner, Repo), Times.Exactly(2));
        }

        [Test]
        public async void TestStarringToggleCommand()
        {
            GitHubClientMock.Setup(m => m.Activity.Starring.StarRepo(Owner, Repo)).Returns(() => Task.FromResult(true));
            GitHubClientMock.Setup(m => m.Activity.Starring.RemoveStarFromRepo(Owner, Repo)).Returns(() => Task.FromResult(true));

            // should star when un-starred (if that's a word)
            GitHubClientMock.Setup(m => m.Activity.Starring.CheckStarred(Owner, Repo)).Returns(() => Task.FromResult(false));

            var vm = new RepositoryViewModel(Owner, Repo);
            Assert.AreEqual(false, vm.IsStarred);
            await vm.ToggleStarred.Execute();
            GitHubClientMock.Verify(m => m.Activity.Starring.StarRepo(Owner,Repo), Times.Once());

            // should un-star when starred
            GitHubClientMock.Setup(m => m.Activity.Starring.CheckStarred(Owner, Repo)).Returns(() => Task.FromResult(true));

            vm = new RepositoryViewModel(Owner, Repo);
            Assert.AreEqual(true, vm.IsStarred);
            await vm.ToggleStarred.Execute();
            GitHubClientMock.Verify(m => m.Activity.Starring.RemoveStarFromRepo(Owner, Repo), Times.Once());
        }

        [Test]
        public async void TestWatchingToggleCommand()
        {
            GitHubClientMock.Setup(m => m.Activity.Watching.WatchRepo(Owner, Repo, It.IsAny<NewSubscription>())).Returns(() => Task.FromResult(new Subscription()));
            GitHubClientMock.Setup(m => m.Activity.Watching.UnwatchRepo(Owner, Repo)).Returns(() => Task.FromResult(true));

            // should watch when not watched
            GitHubClientMock.Setup(m => m.Activity.Watching.CheckWatched(Owner, Repo)).Returns(() => Task.FromResult(false));

            var vm = new RepositoryViewModel(Owner, Repo);
            Assert.AreEqual(false, vm.IsWatched);
            await vm.ToggleWatch.Execute();
            GitHubClientMock.Verify(m => m.Activity.Watching.WatchRepo(Owner, Repo, It.IsAny<NewSubscription>()), Times.Once());

            // should un-watch when watched
            GitHubClientMock.Setup(m => m.Activity.Watching.CheckWatched(Owner, Repo)).Returns(() => Task.FromResult(true));

            vm = new RepositoryViewModel(Owner, Repo);
            Assert.AreEqual(true, vm.IsWatched);
            await vm.ToggleWatch.Execute();
            GitHubClientMock.Verify(m => m.Activity.Watching.UnwatchRepo(Owner, Repo), Times.Once());
        }

        [Test]
        public async void TestRepoPRViewModel()
        {
            var vm = new RepositoryPullRequestsViewModel(Owner, Repo);
            GitHubClientMock.Setup(m => m.PullRequest.GetAllForRepository(Owner, Repo)).Returns(() => Task.FromResult(new List<PullRequest> { new PullRequest()} as IReadOnlyList<PullRequest>));
            await vm.Refresh();
            Assert.AreEqual(1, vm.PullRequests.Count());
            GitHubClientMock.Verify(m => m.PullRequest.GetAllForRepository(Owner, Repo), Times.Once());
        }

        [Test]
        public async void TestRepoIssuesVM()
        {
            var vm = new RepositoryIssuesViewModel(Owner, Repo);
            GitHubClientMock.Setup(m => m.Issue.GetAllForRepository(Owner, Repo))
                .Returns(() => Task.FromResult(new List<Issue> {new Issue()} as IReadOnlyList<Issue>));
            await vm.Refresh();
            Assert.AreEqual(1, vm.Issues.Count());
            GitHubClientMock.Verify(m => m.Issue.GetAllForRepository(Owner, Repo));
        }

        [Test]
        public async void TestRepoCommitsVM()
        {
            var vm = new RepositoryCommitsViewModel(Owner, Repo);
            GitHubClientMock.Setup(m => m.Repository.Commits.GetAll(Owner, Repo, It.IsAny<CommitRequest>()))
                .Returns(() => Task.FromResult(new List<GitHubCommit> { new GitHubCommit() } as IReadOnlyList<GitHubCommit>));
            await vm.Refresh();
            Assert.AreEqual(1, vm.Commits.Count());
            GitHubClientMock.Verify(m => m.Repository.Commits.GetAll(Owner, Repo, It.IsAny<CommitRequest>()));
        }
    }
}
