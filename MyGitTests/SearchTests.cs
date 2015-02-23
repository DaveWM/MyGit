using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MyGit.ViewModels;
using NUnit.Framework;
using Octokit;

namespace MyGitTests
{
    [TestFixture]
    public class SearchTests : TestBase
    {
        [SetUp]
        public new void Init()
        {
            GitHubClientMock.Setup(m => m.Search.SearchRepo(It.IsAny<SearchRepositoriesRequest>()))
                .Returns(() => Task.FromResult(new SearchRepositoryResult
                {
                    Items = new ReadOnlyCollection<Repository>(new List<Repository>
                    {
                        new Repository
                        {
                            Id = 1337
                        }
                    })
                }));
        }

        [Test]
        public async void ShouldIgnoreEmptyOrNullSearchString()
        {
            var vm = new RepoSearchViewModel();
            await vm.Refresh();
            GitHubClientMock.Verify(m => m.Search.SearchRepo(It.IsAny<SearchRepositoriesRequest>()), Times.Never());
            Assert.AreEqual(0, vm.SearchResults.Count());
        }

        [Test]
        public void ShouldRefreshOnSearchStringChangeAndUpdateSearchResults()
        {
            var vm = new RepoSearchViewModel();
            vm.SearchString = "search";
            GitHubClientMock.Verify(m => m.Search.SearchRepo(It.Is<SearchRepositoriesRequest>(sr => sr.Term == "search")), Times.Once());
            Assert.AreEqual(1, vm.SearchResults.Count());
            Assert.AreEqual(1337, vm.SearchResults.FirstOrDefault().Id);
        }

        [Test]
        public async void ShouldSearchOnSearchCommand()
        {
            var vm = new RepoSearchViewModel
            {
                SearchString = "search"
            };
            await vm.SearchCommand.Execute();

            // refreshes once when search string changed, then again when command executed
            GitHubClientMock.Verify(m => m.Search.SearchRepo(It.IsAny<SearchRepositoriesRequest>()), Times.Exactly(2));
            Assert.AreEqual(1, vm.SearchResults.Count());
            Assert.AreEqual(1337, vm.SearchResults.FirstOrDefault().Id);
        }
    }
}
