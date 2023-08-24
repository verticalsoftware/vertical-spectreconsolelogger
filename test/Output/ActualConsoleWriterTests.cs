using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Output
{
    public class ActualConsoleWriterTests
    {
        [Fact]
        public async Task WriteInBackground()
        {
            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddSpectreConsole(
                    config =>
                    {
                        config.WriteInBackground();
                    }))
                .BuildServiceProvider();

            var logger = services.GetRequiredService<ILogger<ActualConsoleWriterTests>>();
            
            logger.LogInformation("Test event successful");

            await services.DisposeAsync();
        }
        
        [Fact]
        public async Task WriteInForeground()
        {
            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddSpectreConsole(
                    config =>
                    {
                        config.WriteInForeground();
                    }))
                .BuildServiceProvider();

            var logger = services.GetRequiredService<ILogger<ActualConsoleWriterTests>>();
            
            logger.LogInformation("Test event successful");

            await services.DisposeAsync();
        }
        
        [Fact]
        public async Task WriteErrorInBackground()
        {
            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddSpectreConsole(
                    config =>
                    {
                        config.WriteInBackground();
                    }))
                .BuildServiceProvider();

            var logger = services.GetRequiredService<ILogger<ActualConsoleWriterTests>>();

            logger.LogError(new ArgumentException(), "Oops");

            await services.DisposeAsync();
        }
        
        [Fact]
        public async Task WriteErrorInForeground()
        {
            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddSpectreConsole(
                    config =>
                    {
                        config.WriteInForeground();
                    }))
                .BuildServiceProvider();

            var logger = services.GetRequiredService<ILogger<ActualConsoleWriterTests>>();
            
            logger.LogError(new ArgumentException(), "Oops");

            await services.DisposeAsync();
        }
    }
}