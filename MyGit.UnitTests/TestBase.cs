using Microsoft.Practices.Unity;
using Moq;
using MyGit;
using MyGit.Services;
using MyGitTests.Mocks;
using NUnit.Framework;
using Octokit;

namespace MyGitTests
{
    public class TestBase
    {
        protected Mock<ILoginService> LoginServiceMock;
        protected Mock<IGitHubClient> GitHubClientMock;
        protected MockLocalStorageService LocalStorageServiceMock;
        protected Mock<INavigationService> MockNavigationService;
        
        [SetUp]
        public void Init()
        {
            LoginServiceMock = new Mock<ILoginService>();
            GitHubClientMock = new Mock<IGitHubClient>();
            LocalStorageServiceMock = new MockLocalStorageService();
            MockNavigationService = new Mock<INavigationService>();

            App.Container.RegisterInstance(GitHubClientMock.Object);
            App.Container.RegisterInstance(LoginServiceMock.Object);
            App.Container.RegisterInstance<ILocalStorageService>(LocalStorageServiceMock);
            App.Container.RegisterInstance(MockNavigationService.Object);
        }
    }
}
