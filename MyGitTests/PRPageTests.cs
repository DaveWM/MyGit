using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MyGit.ViewModels.PullRequestPage;
using NUnit.Framework;
using Octokit;

namespace MyGitTests
{
    [TestFixture]
    public class PRPageTests : TestBase
    {
        private string _repo;
        private string _owner;
        private int _number;
        
        [SetUp]
        public new void Init()
        {
            GitHubClientMock.Setup(m => m.PullRequest.Get(_owner, _repo, _number))
                .Returns(() => Task.FromResult(new PullRequest()));
            GitHubClientMock.Setup(m => m.PullRequest.Commits(_owner, _repo, _number))
                .Returns(() => Task.FromResult(new List<PullRequestCommit>() as IReadOnlyList<PullRequestCommit>));
            GitHubClientMock.Setup(m => m.Issue.Comment.GetForIssue(_owner, _repo, _number))
                .Returns(() => Task.FromResult(new List<IssueComment>() as IReadOnlyList<IssueComment>));
        }

        [Test]
        public void ShouldGetPR()
        {
            var pr = new PullRequest
            {
                Number = 1337
            };
            GitHubClientMock.Setup(m => m.PullRequest.Get(_owner, _repo, _number))
                .Returns(() => Task.FromResult(pr));

            var vm = new PullRequestViewModel(_repo, _owner, _number);
            GitHubClientMock.Verify(m => m.PullRequest.Get(_owner,_repo,_number), Times.Once());
            Assert.AreEqual(pr, vm.PR);
        }

        [Test]
        public async void ShouldRefreshChildVMs()
        {
            var vm = new PullRequestViewModel(_repo, _owner, _number);

            var commitsVMMock = new Mock<PRCommitsViewModel>(_repo, _owner, _number);
            var commentsVMMock = new Mock<PRCommentsViewModel>(_repo, _owner, _number);
            commitsVMMock.Setup(m => m.Refresh()).Returns(() => Task.Run(() => { }));
            commentsVMMock.Setup(m => m.Refresh()).Returns(() => Task.Run(() => { }));

            vm.CommentsViewModel = commentsVMMock.Object;
            vm.CommitsViewModel = commitsVMMock.Object;

            await vm.Refresh();
            commentsVMMock.Verify(m => m.Refresh(), Times.Once());
            commitsVMMock.Verify(m => m.Refresh(), Times.Once());
        }

        [Test]
        public async void PRCommentsRefreshShouldGetComments()
        {
            GitHubClientMock.Setup(m => m.Issue.Comment.GetForIssue(_owner, _repo, _number))
                .Returns(() => Task.FromResult(new List<IssueComment>{new IssueComment()} as IReadOnlyList<IssueComment>));

            var vm = new PRCommentsViewModel(_repo, _owner, _number);
            await vm.Refresh();
            GitHubClientMock.Verify(m => m.Issue.Comment.GetForIssue(_owner,_repo,_number), Times.Once());
            Assert.AreEqual(1, vm.Comments.Count());
        }

        [Test]
        public async void PRCommitsRefreshShouldGetCommits()
        {
            GitHubClientMock.Setup(m => m.PullRequest.Commits(_owner, _repo, _number))
                .Returns(() => Task.FromResult(new List<PullRequestCommit> { new PullRequestCommit() } as IReadOnlyList<PullRequestCommit>));

            var vm = new PRCommitsViewModel(_repo, _owner, _number);
            await vm.Refresh();
            GitHubClientMock.Verify(m => m.PullRequest.Commits(_owner, _repo, _number), Times.Once());
            Assert.AreEqual(1, vm.Commits.Count());
        }
    }
}
