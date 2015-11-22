using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Octokit.Internal;

namespace NotificationsBackgroundTask
{
    internal class NonCachedHttpClient : HttpClientAdapter
    {
        protected override HttpRequestMessage BuildRequestMessage(IRequest request)
        {
            request.Headers["Cache-control"] = "no-cache";
            request.Headers["Pragma"] = "no-cache";
            return base.BuildRequestMessage(request);
        }

        public NonCachedHttpClient() : base(HttpMessageHandlerFactory.CreateDefault)
        {
        }
    }
}
