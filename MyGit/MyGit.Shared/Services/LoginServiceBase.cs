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

        bool TestLoggedIn();
        Task<string> SetTokenFromCode(string code);
        void ClearToken();
    }
    public abstract class LoginServiceBase : ILoginService
    {
        protected readonly IGitHubClient GitHubClient;

        protected const string TokenSettingKey = "token";
        protected abstract string ClientSecret { get; }
        protected abstract string ClientId { get; }
        protected readonly List<string> Scopes = new List<string>{ "notifications", "user" };

        public string Token
        {
            get
            {
                var settings = ApplicationData.Current.LocalSettings;
                return settings.Values.ContainsKey(TokenSettingKey) ? settings.Values[TokenSettingKey] as string : null;
            }
            private set
            {
                var settings = ApplicationData.Current.LocalSettings;
                settings.Values[TokenSettingKey] = value;
                RefreshCredentialsFromStorage();
            }
        }

        private void RefreshCredentialsFromStorage()
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(TokenSettingKey))
            {
                (this.GitHubClient as GitHubClient).Credentials = new Credentials(settings.Values[TokenSettingKey] as string);
            }
        }
        public async Task<string> SetTokenFromCode(string code)
        {
            var token = await this.GitHubClient.Oauth.CreateAccessToken(new OauthTokenRequest(ClientId, ClientSecret, code));
            this.Token = token.AccessToken;
            return token.AccessToken;
        }

        public void ClearToken()
        {
            this.Token = null;
        }

        protected LoginServiceBase()
        {
            this.GitHubClient = App.Container.Resolve<IGitHubClient>();
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

        public bool TestLoggedIn()
        {
            throw new NotImplementedException();
        }
    }
}
