using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Practices.Unity;
using Octokit;

namespace MyGit.Services
{
    public interface ILoginService
    {
        Task Login();
        void Logout();
        bool IsLoggedIn { get;}
        Task<string> SetTokenFromCode(string code);
    }
    public abstract class LoginServiceBase : ILoginService
    {
        protected readonly IGitHubClient GitHubClient;
        protected readonly ILocalStorageService LocalStorageService;

        protected const string TokenSettingKey = "token";
        protected abstract string ClientSecret { get; }
        protected abstract string ClientId { get; }
        protected readonly List<string> Scopes = new List<string>{ "notifications", "user", "repo", "gist" };

        public string Token
        {
            get { return LocalStorageService.Get<string>(TokenSettingKey); }
            private set
            {
                LocalStorageService.Set(TokenSettingKey, value);
                RefreshCredentialsFromStorage();
            }
        }

        private void RefreshCredentialsFromStorage()
        {
            if (Token != null)
            {
                // hack to make tests pass - IGitHubClient should have credentials on it
                var clientConcrete = (this.GitHubClient as GitHubClient);
                if (clientConcrete != null)
                {
                    clientConcrete.Credentials = new Credentials(Token);
                }
            }
        }
        public async Task<string> SetTokenFromCode(string code)
        {
            var token = await this.GitHubClient.Oauth.CreateAccessToken(new OauthTokenRequest(ClientId, ClientSecret, code));
            this.Token = token.AccessToken;
            return token.AccessToken;
        }

        protected LoginServiceBase()
        {
            this.GitHubClient = App.Container.Resolve<IGitHubClient>();
            LocalStorageService = App.Container.Resolve<ILocalStorageService>();
            RefreshCredentialsFromStorage();
        }

        public abstract Task Login();

        public void Logout()
        {
            this.Token = null;
        }

        public bool IsLoggedIn
        {
            get { return this.Token != null; }
        }
    }
}
