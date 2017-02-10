using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Interfaces.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace Grapevine.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRoutingTable
    {
        /// <summary>
        /// 
        /// </summary>
        HttpMethod DefaultHttpMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string DefaultPathInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IList<IRoute> Routes { get; }

        /// <summary>
        /// 
        /// </summary>
        Type RouteType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        void Import(IRoutingTable routingTable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        int InsertAt(int index, IRoute route);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        bool Register(IRoute route);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IList<IRoute> FindMatchingRoutes(IHttpContext context);
    }

    public class RoutingTable : IRoutingTable
    {
        protected internal readonly IList<IRoute> RegisteredRoutes;
        protected internal ConcurrentDictionary<string, IList<IRoute>> RouteCache;

        public IList<IRoute> Routes => RegisteredRoutes.ToList().AsReadOnly();

        public HttpMethod DefaultHttpMethod { get; set; }

        public string DefaultPathInfo { get; set; }

        public Type RouteType { get; protected set; }

        public RoutingTable()
        {
            DefaultHttpMethod = HttpMethod.ALL;
            DefaultPathInfo = string.Empty;

            RegisteredRoutes = new List<IRoute>();
            RouteCache = new ConcurrentDictionary<string, IList<IRoute>>();

            RouteType = typeof(Route);
        }

        public static RoutingTable For<T>() where T : IRoute
        {
            return new RoutingTable {RouteType = typeof(T)};
        }

        public void Import(IRoutingTable routingTable)
        {
            throw new NotImplementedException();
        }

        public int InsertAt(int index, IRoute route)
        {
            throw new NotImplementedException();
        }

        public bool Register(IRoute route)
        {
            throw new NotImplementedException();
        }

        public IList<IRoute> FindMatchingRoutes(IHttpContext context)
        {
            throw new NotImplementedException();
        }
    }

    public static class RoutingTableInsertExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public static int InsertAfter(this IRoutingTable routingTable, int index, IRoute route)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="afterRoute"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public static int InsertAfter(this IRoutingTable routingTable, IRoute afterRoute, IRoute route)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="index"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        public static int InsertAfter(this IRoutingTable routingTable, int index, IList<IRoute> routes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="afterRoute"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        public static int InsertAfter(this IRoutingTable routingTable, IRoute afterRoute, IList<IRoute> routes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="index"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public static int InsertBefore(this IRoutingTable routingTable, int index, IRoute route)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="beforeRoute"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public static int InsertBefore(this IRoutingTable routingTable, IRoute beforeRoute, IRoute route)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="index"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        public static int InsertBefore(this IRoutingTable routingTable, int index, IList<IRoute> routes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="beforeRoute"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        public static int InsertBefore(this IRoutingTable routingTable, IRoute beforeRoute, IList<IRoute> routes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public static int InsertFirst(this IRoutingTable routingTable, IRoute route)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="routes"></param>
        /// <returns></returns>
        public static int InsertFirst(this IRoutingTable routingTable, IList<IRoute> routes)
        {
            throw new NotImplementedException();
        }
    }

    public static class RoutingTableImportExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingTable"></param>
        public static void Import<T>(this IRoutingTable routingTable) where T : IRoutingTable, new()
        {
            routingTable.Import((IRoutingTable)Activator.CreateInstance(typeof(T)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="type"></param>
        public static void Import(this IRoutingTable routingTable, Type type)
        {
            if (!type.IsClass) throw new ArgumentException($"Cannot Import: {type.FullName} type is not a class");
            if (type.IsAbstract) throw new ArgumentException($"Cannot Import: {type.FullName} is an abstract class");
            if (!type.Implements<IRoutingTable>()) throw new ArgumentException($"Cannot Import: {type.FullName} does not implement {typeof(IRoutingTable).FullName}");
            if (!type.HasParameterlessConstructor()) throw new ArgumentException($"Cannot Import: {type.FullName} does not have parameterless constructor");
            routingTable.Import((IRoutingTable)Activator.CreateInstance(type));
        }
    }

    public static class RoutingTableRegisterExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, MethodInfo method)
        {
            return routingTable.Register(method, routingTable.DefaultHttpMethod, routingTable.DefaultPathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="method"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, MethodInfo method, HttpMethod httpMethod)
        {
            return routingTable.Register(method, httpMethod, routingTable.DefaultPathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="method"></param>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, MethodInfo method, string pathInfo)
        {
            return routingTable.Register(method, routingTable.DefaultHttpMethod, pathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="method"></param>
        /// <param name="httpMethod"></param>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, MethodInfo method, HttpMethod httpMethod, string pathInfo)
        {
            return routingTable.Register(new Route(method, httpMethod, pathInfo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, Func<IHttpContext, IHttpContext> func)
        {
            return routingTable.Register(func, routingTable.DefaultHttpMethod, routingTable.DefaultPathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="func"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod)
        {
            return routingTable.Register(func, httpMethod, routingTable.DefaultPathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="func"></param>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, Func<IHttpContext, IHttpContext> func, string pathInfo)
        {
            return routingTable.Register(func, routingTable.DefaultHttpMethod, pathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="func"></param>
        /// <param name="httpMethod"></param>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, Func<IHttpContext, IHttpContext> func, HttpMethod httpMethod, string pathInfo)
        {
            return routingTable.Register(new Route(func, httpMethod, pathInfo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, Action<IHttpContext> action)
        {
            return routingTable.Register(action, routingTable.DefaultHttpMethod, routingTable.DefaultPathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="action"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, Action<IHttpContext> action, HttpMethod httpMethod)
        {
            return routingTable.Register(action, httpMethod, routingTable.DefaultPathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="action"></param>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, Action<IHttpContext> action, string pathInfo)
        {
            return routingTable.Register(action, routingTable.DefaultHttpMethod, pathInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="action"></param>
        /// <param name="httpMethod"></param>
        /// <param name="pathInfo"></param>
        /// <returns></returns>
        public static bool Register(this IRoutingTable routingTable, Action<IHttpContext> action, HttpMethod httpMethod, string pathInfo)
        {
            throw new NotImplementedException();
            //return routingTable.Register(new Route(action, httpMethod, pathInfo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingTable"></param>
        public static void Register<T>(this IRoutingTable routingTable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="type"></param>
        public static void Register(this IRoutingTable routingTable, Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routingTable"></param>
        /// <param name="assembly"></param>
        public static void Register(this IRoutingTable routingTable, Assembly assembly)
        {
            throw new NotImplementedException();
        }
    }
}
