using System.Collections.Specialized;
using System.Net;

namespace Grapevine.Interfaces
{
    public interface IHttpRequest<out TRequest>
    {
        TRequest Advanced { get; }

        NameValueCollection Headers { get; }

        string PathInfo { get; }
    }

    public interface IInboundHttpRequest
    {
    }

    public interface IInboundHttpRequest<out TRequest> : IInboundHttpRequest, IHttpRequest<TRequest>
    {
    }

    public interface IOutboundHttpRequest
    {
    }

    public interface IOutboundHttpRequest<out TRequest> : IOutboundHttpRequest, IHttpRequest<TRequest>
    {
    }

    public class InboundHttpRequest : IInboundHttpRequest<HttpListenerRequest>
    {
        public HttpListenerRequest Advanced { get; }

        public InboundHttpRequest(HttpListenerRequest request)
        {
            Advanced = request;
        }

        public NameValueCollection Headers { get; }

        public string PathInfo { get; }
    }
}
