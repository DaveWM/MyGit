using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Octokit;
using Octokit.Internal;

namespace MyGit
{
    public class NonCachedHttpClient : HttpClientAdapter
    {
        protected override HttpRequestMessage BuildRequestMessage(IRequest request)
        {
            request.Headers["Cache-control"] = "no-cache";
            request.Headers["Pragma"] = "no-cache";
            
            // part 1 of horrible hack for news section
            // in the github api, events are split up into pages of 30 events each
            // octokit tries to get every single page, which causes an exception deep in windows RT somewhere (I'm assuming some sort of OutOfMemoryException)
            // need to manually add paging parameters to both the /events and /received_events requests
            if (request.Endpoint.OriginalString.Contains("events") && !request.Endpoint.IsAbsoluteUri)
            {
                var httpRequest = new HttpRequestMessage(request.Method,
                    new Uri(request.BaseAddress.OriginalString + request.Endpoint.OriginalString + "?page=1&per_page=50"));
                request.Headers.ForEach(h => httpRequest.Headers.Add(h.Key, h.Value));
                return httpRequest;
            }
            else return base.BuildRequestMessage(request);
        }

        protected override Task<IResponse> BuildResponse(HttpResponseMessage responseMessage)
        {
            // part 2 of horrible hack for news section
            // octokit looks for the "Links" header on the response from the events api, and gets the url for the next page from it
            // need to set this to null to stop octokit making any more requests
            if (responseMessage.RequestMessage.RequestUri.OriginalString.Contains("events"))
            {
                responseMessage.Headers.Remove("Link");
            }
            return base.BuildResponse(responseMessage);
        }

        public NonCachedHttpClient() : base(HttpMessageHandlerFactory.CreateDefault)
        {
        }
    }
}
