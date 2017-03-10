using System.Net;

namespace Grapevine.Interfaces
{
    public interface IHttpContext<out TContext, out TRequest, out TResponse>
    {
        IHttpRequest<TRequest> Request { get; }

        IHttpResponse<TResponse> Response { get; }

        IRestServer Server { get; }

        TContext Advanced { get; }
    }

    public class HttpContext : IHttpContext<HttpListenerContext, HttpListenerRequest, HttpListenerResponse>
    {
        public IHttpRequest<HttpListenerRequest> Request => new HttpRequest(Advanced.Request);

        public IHttpResponse<HttpListenerResponse> Response => new HttpResponse(Advanced.Response);

        public IRestServer Server { get; }

        public HttpListenerContext Advanced { get; }

        public HttpContext(HttpListenerContext context, IRestServer server)
        {
            Advanced = context;
            Server = server;
        }
    }
}
