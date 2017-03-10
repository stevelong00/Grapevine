using System.Net;

namespace Grapevine.Interfaces
{
    /// <summary>
    /// Provides access to the server, request and response objects in context
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IHttpContext<out TContext, out TRequest, out TResponse> : IDynamicProperties
    {
        /// <summary>
        /// Gets the request object that represents a client's request for a resource
        /// </summary>
        IHttpRequest<TRequest> Request { get; }

        /// <summary>
        /// Gets the response object that will be used to respond to the client's request
        /// </summary>
        IHttpResponse<TResponse> Response { get; }

        /// <summary>
        /// Gets the IRestServer object the client request was sent to
        /// </summary>
        IRestServer Server { get; }

        /// <summary>
        /// Returns the underlying context implementation
        /// </summary>
        TContext Advanced { get; }
    }

    public interface IOutboundHttpContext { }

    public interface IOutboundHttpContext<out TContext, out TRequest, out TResponse> : IOutboundHttpContext, IHttpContext<TContext, TRequest, TResponse>
    {
        new IOutboundHttpRequest<TRequest> Request { get; }

        new IInboundHttpResponse<TResponse> Response { get; }
    }

    public interface IInboundHttpContext
    {
        bool WasRespondedTo { get; }
    }

    /// <summary>
    /// Represents an inbound http request context
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IInboundHttpContext<out TContext, out TRequest, out TResponse> : IInboundHttpContext, IHttpContext<TContext, TRequest, TResponse>
    {
        new IInboundHttpRequest<TRequest> Request { get; }

        new IOutboundHttpResponse<TResponse> Response { get; }
    }

    /// <summary>
    /// Provides access to the server, request and response objects for inbound HttpListenerContext
    /// </summary>
    public class HttpContext : DynamicProperties, IInboundHttpContext<HttpListenerContext, HttpListenerRequest, HttpListenerResponse>
    {
        public HttpListenerContext Advanced { get; }

        public IInboundHttpRequest<HttpListenerRequest> Request => new InboundHttpRequest(Advanced.Request);

        public IOutboundHttpResponse<HttpListenerResponse> Response => new OutboundHttpResponse(Advanced.Response);

        public IRestServer Server { get; }

        public bool WasRespondedTo { get; protected internal set; }

        public HttpContext(HttpListenerContext context, IRestServer server)
        {
            Advanced = context;
            Server = server;
        }

        IHttpRequest<HttpListenerRequest> IHttpContext<HttpListenerContext, HttpListenerRequest, HttpListenerResponse>.Request => Request;

        IHttpResponse<HttpListenerResponse> IHttpContext<HttpListenerContext, HttpListenerRequest, HttpListenerResponse>.Response => Response;
    }
}
