using System;
using System.Linq;
using Shouldly;
using Vertical.SpectreLogger.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public class ScopeManagerTests
    {
        private readonly IScopeManager _instance = new ScopeManager();

        [Fact]
        public void DefaultStateReturnsEmptyArray()
        {
            _instance.Scopes.ShouldBe(Array.Empty<object?>());
        }

        [Fact]
        public void ScopesReturnsSingleValue()
        {
            _instance.BeginScope("test").ShouldNotBeNull();
            _instance.Scopes.ShouldBe(new object?[]{"test"});
        }

        [Fact]
        public void ScopePopped()
        {
            using (_instance.BeginScope("test"))
            {
            }
            _instance.Scopes.ShouldBe(Array.Empty<object?>());
        }

        [Fact]
        public void ScopePoppedAfterMultipleBeginScopes()
        {
            _instance.BeginScope("test-1");
            using (_instance.BeginScope("test-2"))
            {
            }
            _instance.Scopes.ShouldBe(new object?[]{"test-1"});
        }

        [Fact]
        public void ScopesUnwindWhenDisposedOutOfOrder()
        {
            var scopes = Enumerable.Range(1, 5).Select(i => _instance.BeginScope($"test-{i}")).ToArray();

            scopes[2].Dispose();
            
            _instance.Scopes.ShouldBe(new object?[]{"test-1", "test-2"});
        }
    }
}