using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Microsoft.Practices.ObjectBuilder2;
using Octokit;

namespace MyGit.Services
{
    public class LoginService : LoginServiceBase
    {
        protected override string ClientSecret
        {
            get { return "fc86b623d6fbd4488723f403178b5ff51f197b94"; }
        }

        protected override string ClientId
        {
            get { return "a3acb76e788862014097"; }
        }

        public override async Task Login()
        {
            var request = new OauthLoginRequest(ClientId);
            this.Scopes.ForEach(s => request.Scopes.Add(s));
            var oauthUrl = this.GitHubClient.Oauth.GetGitHubLoginUrl(request);
            var authResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, oauthUrl, WebAuthenticationBroker.GetCurrentApplicationCallbackUri());
            
            var responseString = authResult.ResponseData;
            var regex = new Regex(@"(?<=\?code=).*");
            await SetTokenFromCode(regex.Match(responseString).Value);
        }
    }
}
