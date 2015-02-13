using System.Net.Http;
using Octokit.Internal;

namespace MyGit
{
    public class NonCachedHttpClient : HttpClientAdapter
    {
        protected override HttpRequestMessage BuildRequestMessage(IRequest request)
        {
            request.Headers["Cache-control"] = "no-cache";
            request.Headers["Pragma"] = "no-cache";
            return base.BuildRequestMessage(request);
        }
    }
}
