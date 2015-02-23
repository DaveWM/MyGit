using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Moq;
using MyGit;
using MyGit.Services;
using MyGit.ViewModels.IssuePage;
using NUnit.Framework;
using Octokit;

namespace MyGitTests
{
    [TestFixture]
    public class IssueTests : TestBase
    {
        [SetUp]
        public void Init()
        {
            GitHubClientMock.Setup(ghc => ghc.Issue.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() =>
                {
                    var t = new Task<Issue>(() => new Issue());
                    t.Start();
                    return t;
                });
            GitHubClientMock.Setup(
                ghc => ghc.Issue.Comment.GetForIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() =>
                {
                    var t =
                        new Task<IReadOnlyList<IssueComment>>(
                            () => new ReadOnlyCollection<IssueComment>(new List<IssueComment>()));
                    t.Start();
                    return t;
                });
            GitHubClientMock.Setup(
                ghc => ghc.Issue.Events.GetForIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() =>
                {
                    var t =
                        new Task<IReadOnlyList<EventInfo>>(
                            () => new ReadOnlyCollection<EventInfo>(new List<EventInfo>()));
                    t.Start();
                    return t;
                });
        }

        [Test]
        public void TestRepoFullName()
        {
            var vm = new IssueViewModel("repo", 1, "owner");
            Assert.AreEqual("owner/repo", vm.RepoFullName);
        }

        [Test]
        public async void CheckRefreshMakesCorrectRequests()
        {
            var vm = new IssueViewModel("repo", 1, "owner");

            await vm.Refresh();

            GitHubClientMock.Verify(ghc => ghc.Issue.Get("owner", "repo", 1), Times.AtLeastOnce());
            GitHubClientMock.Verify(ghc => ghc.Issue.Comment.GetForIssue("owner", "repo", 1), Times.AtLeastOnce());
            GitHubClientMock.Verify(ghc => ghc.Issue.Events.GetForIssue("owner", "repo", 1), Times.AtLeastOnce());
        }

        [Test]
        public async void TestIssueHistory()
        {
            var firstComment = new IssueComment
            {
                Id = 1,
                CreatedAt = DateTimeOffset.FromFileTime(0)
            };
            var secondComment = new IssueComment
            {
                Id = 3,
                CreatedAt = DateTimeOffset.FromFileTime(100)
            };
            var firstEvent = new EventInfo
            {
                Id = 2,
                CreatedAt = DateTimeOffset.FromFileTime(50)
            };

            GitHubClientMock.Setup(
                ghc => ghc.Issue.Comment.GetForIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() =>
                {
                    var t =
                        new Task<IReadOnlyList<IssueComment>>(
                            () => new ReadOnlyCollection<IssueComment>(new List<IssueComment>
                            {firstComment,secondComment}));
                    t.Start();
                    return t;
                });
            GitHubClientMock.Setup(
                ghc => ghc.Issue.Events.GetForIssue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() =>
                {
                    var t =
                        new Task<IReadOnlyList<EventInfo>>(() => new ReadOnlyCollection<EventInfo>(new List<EventInfo>
                        {
                            firstEvent
                        }));
                    t.Start();
                    return t;
                });

            var vm = new IssueViewModel("repo", 1, "owner");
            await vm.Refresh();
            var history = vm.HistoryViewModel.IssueHistory.ToList();
            Assert.AreEqual(firstComment, history[0]);
            Assert.AreEqual(firstEvent, history[1]);
            Assert.AreEqual(secondComment, history[2]);
        }
    }
}
