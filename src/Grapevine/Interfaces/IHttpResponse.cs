using System.IO;
using Grapevine.Common;

namespace Grapevine.Interfaces
{
    public interface IHttpResponse<out TResponse>
    {
        TResponse Advanced { get; }
    }

    public interface IInboundHttpResponse
    {

    }

    public interface IInboundHttpResponse<out TResponse> : IInboundHttpResponse, IHttpResponse<TResponse>
    {
        
    }

    public interface IOutboundHttpResponse
    {
        void AddHeader(string name, string value);

        void SendResponse(byte[] contents);
    }

    public interface IOutboundHttpResponse<out TResponse> : IOutboundHttpResponse, IHttpResponse<TResponse>
    {
        
    }

    public class OutboundHttpResponse : IOutboundHttpResponse<System.Net.HttpListenerResponse>
    {
        public System.Net.HttpListenerResponse Advanced { get; }

        public OutboundHttpResponse(System.Net.HttpListenerResponse response)
        {
            Advanced = response;
        }

        public void AddHeader(string name, string value)
        {
            Advanced.AddHeader(name, value);
        }

        public void SendResponse(byte[] contents)
        {

        }
    }

    public static class OutboundHttpResponseExtensions
    {
        public static void SendResponse(this IOutboundHttpResponse response, HttpStatusCode statusCode)
        {
            
        }

        public static void SendResponse(this IOutboundHttpResponse response, Stream stream)
        {

        }
    }
}