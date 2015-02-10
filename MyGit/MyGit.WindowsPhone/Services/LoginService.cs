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
            get { return "baaca06e87557d30141e02c87a65e563a13f8ae1"; }
        }

        protected override string ClientId
        {
            get { return "5a3ab104db4f2ef01daf"; }
        }

        public override async Task Login()
        {
            var request = new OauthLoginRequest(ClientId);
            this.Scopes.ForEach(s => request.Scopes.Add(s));
            var oauthUrl = GitHubClient.Oauth.GetGitHubLoginUrl(request);

            WebAuthenticationBroker.AuthenticateAndContinue(oauthUrl, WebAuthenticationBroker.GetCurrentApplicationCallbackUri());
        }
    }
}
