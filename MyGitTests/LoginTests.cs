﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MyGit.Services;
using MyGit.ViewModels.LoginPage;
using NUnit.Framework;
using Octokit;

namespace MyGitTests
{
    [TestFixture]
    public class LoginTests : TestBase
    {
        [SetUp]
        public void Init()
        {
            LoginServiceMock.Setup(l => l.Login()).Returns(() => Task.Run(() => { }));
        }
        [Test]
        public void ShouldLogoutWhenLoginPageNavigatedTo()
        {
            var vm = new LoginViewModel();
            LoginServiceMock.Verify(l => l.Logout(), Times.Once());
        }

        [Test]
        public async void LoginCommandShouldCallLoginService()
        {
            var vm = new LoginViewModel();
            await vm.LoginCommand.Execute();
            LoginServiceMock.Verify(m => m.Login(), Times.Once());
        }

        [Test]
        public async void TestSetTokenFromCode()
        {
            GitHubClientMock.Setup(m => m.Oauth.CreateAccessToken(It.IsAny<OauthTokenRequest>()))
                .Returns(() => Task.Run(() => new OauthToken("oauth", "token", new List<string>())));
            var service = new LoginService();
            var token = await service.SetTokenFromCode("code");
            
            Assert.AreEqual("token", token);
            Assert.AreEqual("token", service.Token);
            Assert.AreEqual("token", LocalStorageServiceMock.Get<string>("token"));
        }
    }
}
