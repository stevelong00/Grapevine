using System.Net;

namespace Grapevine.Interfaces
{
    public interface IHttpRequest<out TRequest>
    {
        TRequest Advanced { get; }
    }

    public class HttpRequest : IHttpRequest<HttpListenerRequest>
    {
        public HttpListenerRequest Advanced { get; }

        public HttpRequest(HttpListenerRequest request)
        {
            Advanced = request;
        }
    }
}
