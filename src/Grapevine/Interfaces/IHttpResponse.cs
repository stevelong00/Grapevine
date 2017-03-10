using System.Net;

namespace Grapevine.Interfaces
{
    public interface IHttpResponse<out TResponse>
    {
        TResponse Advanced { get; }
    }

    public class HttpResponse : IHttpResponse<HttpListenerResponse>
    {
        public HttpListenerResponse Advanced { get; }

        public HttpResponse(HttpListenerResponse response)
        {
            Advanced = response;
        }
    }
}
