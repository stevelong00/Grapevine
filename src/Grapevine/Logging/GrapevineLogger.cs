using System;

namespace Grapevine.Logging
{
    public abstract class GrapevineLogger
    {
        public abstract bool IsEnabled(GrapevineLogLevel level);
        public abstract void Log(GrapevineLogLevel level, string requestId, string msg, Exception exception = null);

        internal void Trace(string msg, string requestId = null) { Log(GrapevineLogLevel.Trace, requestId, msg); }
        internal void Debug(string msg, string requestId = null) { Log(GrapevineLogLevel.Debug, requestId, msg); }
        internal void Info(string msg, string requestId = null) { Log(GrapevineLogLevel.Info, requestId, msg); }
        internal void Warn(string msg, string requestId = null) { Log(GrapevineLogLevel.Warn, requestId, msg); }
        internal void Error(string msg, string requestId = null) { Log(GrapevineLogLevel.Error, requestId, msg); }
        internal void Fatal(string msg, string requestId = null) { Log(GrapevineLogLevel.Fatal, requestId, msg); }

        internal void Trace(string msg, Exception ex, string requestId = null) { Log(GrapevineLogLevel.Trace, requestId, msg, ex); }
        internal void Debug(string msg, Exception ex, string requestId = null) { Log(GrapevineLogLevel.Debug, requestId, msg, ex); }
        internal void Info(string msg, Exception ex, string requestId = null) { Log(GrapevineLogLevel.Info, requestId, msg, ex); }
        internal void Warn(string msg, Exception ex, string requestId = null) { Log(GrapevineLogLevel.Warn, requestId, msg, ex); }
        internal void Error(string msg, Exception ex, string requestId = null) { Log(GrapevineLogLevel.Error, requestId, msg, ex); }
        internal void Fatal(string msg, Exception ex, string requestId = null) { Log(GrapevineLogLevel.Fatal, requestId, msg, ex); }
    }

    public static class LoggerExtensions
    {
        public static void BeginRouting(this GrapevineLogger logger, string message, string requestId)
        {
            logger.Info($"Routing Request  : {message}", requestId);
        }

        public static void EndRouting(this GrapevineLogger logger, string message, string requestId)
        {
            logger.Trace($"Routing Complete : {message}", requestId);
        }

        public static void RouteInvoked(this GrapevineLogger logger, string message, string requestId)
        {
            logger.Trace($"Route Invoked    : {message}", requestId);
        }
    }
}