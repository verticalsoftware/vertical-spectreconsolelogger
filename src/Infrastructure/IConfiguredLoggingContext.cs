using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Infrastructure
{
    public interface IConfiguredLoggingContext
    {
        LogLevel MinimumLevel { get; }

        FormattingProfile GetFormattingProfile(LogLevel logLevel);

        ITemplateRenderer[] GetRenderingPipeline(LogLevel logLevel);
    }
}