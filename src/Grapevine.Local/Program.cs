using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace Grapevine.Local
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var server = new RestServer())
            {
                server.LogToConsole();
                server.PublicFolder.Prefix = "Grapevine";
                server.PublicFolder.FolderPath = @"C:\source\github\gv-gh-pages";

                server.OnBeforeStart = () => Console.WriteLine("---------------> Starting Server");
                server.OnAfterStart = () => Console.WriteLine($"<--------------- Server Started");

                server.OnBeforeStop = () => Console.WriteLine("---------------> Stopping Server");
                server.OnAfterStop = () =>
                {
                    Console.WriteLine("<--------------- Server Stopped");
                    Console.ReadLine();
                };

                server.Start();
                Console.ReadLine();
                server.Stop();
            }
        }
    }

    [RestResource(BasePath = "test")]
    public class TestResource
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/inorder")]
        public IHttpContext MeFirst(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/inorder")]
        public IHttpContext MeSecond(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/inorder")]
        public IHttpContext MeThird(IHttpContext context)
        {
            return context;
        }

        [RestRoute]
        public IHttpContext HelloWorld(IHttpContext context)
        {
            context.Response.SendResponse("Hello,world.");
            return context;
        }
    }

    [RestResource]
    public class FormData
    {
        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/upload")]
        public IHttpContext ShowMe(IHttpContext context)
        {
            var filename = Guid.NewGuid().ToString();
            var dir = Directory.CreateDirectory(@"c:\Users\scott\Desktop\payloads").FullName;
            var path = Path.Combine(dir, $"{filename}.txt");

            //var FormData = ParseFormData(context.Request.Payload);

            File.WriteAllText(path, context.Request.Headers.ToString());
            File.AppendAllText(path, context.Request.Payload);

            /* RESPONSE */
            var response = (context.Request.ContentType == ContentType.FormUrlEncoded)
                ? "FormUrlEncoded"
                : (context.Request.ContentType == ContentType.MultipartFormData) ? "MultipartFormData" : "UNKNOWN";

            context.Response.SendResponse(HttpStatusCode.Ok, response);
            return context;
        }
    }

    public static class RequestExtensions
    {
        public static IDictionary<string, string> ParseFormUrlEncoded(this IHttpRequest request)
        {
            var data = new Dictionary<string, string>();

            foreach (var tuple in request.Payload.Split('='))
            {
                var parts = tuple.Split('&');
                var key = Uri.UnescapeDataString(parts[0]);
                var val = Uri.UnescapeDataString(parts[1]);
                if (!data.ContainsKey(key)) data.Add(key, val);
            }

            return data;
        }

        public static IDictionary<string, FormElement> ParseFormData(this IHttpRequest request)
        {
            var data = new Dictionary<string, FormElement>();
            var boundary = GetBoundary(request.Headers.Get("Content-Type"));

            if (boundary == null) return data;

            foreach (var part in request.Payload.Split(new[] { boundary }, StringSplitOptions.RemoveEmptyEntries))
            {
                var element = new FormElement(part);
                if (!data.ContainsKey(element.Name)) data.Add(element.Name, element);
            }

            return data;
        }

        private static string GetBoundary(string contenttype)
        {
            if (string.IsNullOrWhiteSpace(contenttype)) return null;

            return (from part in contenttype.Split(';', ',')
                select part.TrimStart().TrimEnd().Split('=')
                into parts
                where parts[0].Equals("boundary", StringComparison.CurrentCultureIgnoreCase)
                select parts[1]).FirstOrDefault();
        }
    }

    public class FormElement
    {
        public string Name => _dispositionParams["name"];
        public string FileName => _dispositionParams["filename"];
        public Dictionary<string, string> Headers { get; private set; }
        public string Value { get; }

        private Dictionary<string, string> _dispositionParams;

        public FormElement(string data)
        {
            var parts = data.Split(new [] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);
            Value = parts[1];

            ParseHeaders(parts[0]);
            ParseParams(Headers["Content-Disposition"]);
        }

        private void ParseHeaders(string data)
        {
            Headers = data.TrimStart().TrimEnd().Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries).Select(header => header.Split(new[] {':'})).ToDictionary(parts => parts[0].TrimStart().TrimEnd(), parts => parts[1].TrimStart().TrimEnd());
        }

        private void ParseParams(string data)
        {
            _dispositionParams = new Dictionary<string, string>();

            foreach (var part in data.Split(new[] {';'}))
            {
                if (part.IndexOf("=") == -1) continue;
                var parts = part.Split(new[] {'='});
                _dispositionParams.Add(parts[0].TrimStart(' '), parts[1].TrimEnd('"').TrimStart('"'));
            }
        }
    }
}
