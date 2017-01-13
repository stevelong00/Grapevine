using Grapevine.Interfaces.Server;
using Grapevine.Server;

namespace Grapevine.Logging
{
    public static class LoggerExtensions
    {
        public static void RoutingRequest(this GrapevineLogger logger, IHttpRequest request, int totalRoutes)
        {
            logger.Info($"Routing Request  : {request.Id} - {request.Name} has {totalRoutes} routes");
        }

        public static void RoutingComplete(this GrapevineLogger logger, IHttpRequest request, int totalRoutes, int routeCounter)
        {
            logger.Trace($"Routing Complete : {request.Id} - {routeCounter} of {totalRoutes} routes invoked");
        }

        public static void RouteInvoked(this GrapevineLogger logger, IHttpRequest request, IRoute route, int totalRoutes, int routeCounter)
        {
            logger.Trace($"Route Invoked    : {request.Id} - {routeCounter}/{totalRoutes} {route.Name}");
        }
    }
}
