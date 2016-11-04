using System;
using Grapevine.Logging;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Logging
{
    public class NoOpLoggingProviderFacts
    {
        [Fact]
        public void NoOpDoesNothing()
        {
            var logger = new NoOpLoggingProvider().CreateLogger("x");

            logger.IsEnabled(GrapevineLogLevel.Trace).ShouldBeFalse();
            logger.IsEnabled(GrapevineLogLevel.Debug).ShouldBeFalse();
            logger.IsEnabled(GrapevineLogLevel.Info).ShouldBeFalse();
            logger.IsEnabled(GrapevineLogLevel.Warn).ShouldBeFalse();
            logger.IsEnabled(GrapevineLogLevel.Error).ShouldBeFalse();
            logger.IsEnabled(GrapevineLogLevel.Fatal).ShouldBeFalse();

            logger.Log(GrapevineLogLevel.Debug, "1234", "message", new Exception());
        }
    }
}
