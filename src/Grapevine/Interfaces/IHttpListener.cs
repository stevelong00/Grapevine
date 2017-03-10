using System;
using System.Collections.Generic;

namespace Grapevine.Interfaces
{
    public interface IHttpListener<out TListener, out TContext>
    {
        TListener Advanced { get; }

        bool IsListening { get; }

        ICollection<string> Prefixes { get; }

        IAsyncResult BeginGetContext(AsyncCallback callback, object state);

        void Close();

        TContext EndGetContext(IAsyncResult asyncResult);

        void Start();

        void Stop();
    }

    public class HttpListener : IHttpListener<System.Net.HttpListener, System.Net.HttpListenerContext>
    {
        public System.Net.HttpListener Advanced { get; }

        public bool IsListening => Advanced.IsListening;

        public ICollection<string> Prefixes => Advanced.Prefixes;

        public HttpListener()
        {
            Advanced = new System.Net.HttpListener();
        }

        public IAsyncResult BeginGetContext(AsyncCallback callback, object state)
        {
            return Advanced.BeginGetContext(callback, state);
        }

        public void Close()
        {
            Advanced.Close();
        }

        public System.Net.HttpListenerContext EndGetContext(IAsyncResult asyncResult)
        {
            return Advanced.EndGetContext(asyncResult);
        }

        public void Start()
        {
            Advanced.Start();
        }

        public void Stop()
        {
            Advanced.Stop();
        }
    }
}
