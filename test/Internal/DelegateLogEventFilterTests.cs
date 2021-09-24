using Shouldly;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Internal
{
    public class DelegateLogEventFilterTests
    {
        [Fact]
        public void FilterInvokesDelegate()
        {
            var invoked = false;
            var testInstance = new DelegateLogEventFilter((in LogEventContext _) => invoked = true);

            testInstance.Filter(default);

            invoked.ShouldBeTrue();
        }
    }
}