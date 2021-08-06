using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Options
{
    public static class OptionsExtensions
    {
        private static readonly IEnumerable<LogLevel> ConfigurableLogLevels = Enum
            .GetValues(typeof(LogLevel))
            .Cast<LogLevel>()
            .Where(level => level != LogLevel.None);
        
        /// <summary>
        /// Configures all log level profiles.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="configure">An action that is given a profile for configuration.</param>
        /// <returns><see cref="SpectreLoggerOptions"/></returns>
        public static SpectreLoggerOptions ConfigureProfiles(this SpectreLoggerOptions options,
            Action<FormattingProfile> configure)
        {
            return options.ConfigureProfiles(ConfigurableLogLevels, configure);
        }

        /// <summary>
        /// Configures multiple log level profiles.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="logLevels">A sequence of log levels to configure.</param>
        /// <param name="configure">An action that is given a profile for configuration.</param>
        /// <returns><see cref="SpectreLoggerOptions"/></returns>
        public static SpectreLoggerOptions ConfigureProfiles(this SpectreLoggerOptions options,
            IEnumerable<LogLevel> logLevels,
            Action<FormattingProfile> configure)
        {
            foreach (var logLevel in logLevels)
            {
                options.ConfigureProfile(logLevel, configure);
            }

            return options;
        }

        /// <summary>
        /// Configures a single log level profile.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="logLevel">Log level to configure.</param>
        /// <param name="configure">An action that is given a profile for configuration.</param>
        public static SpectreLoggerOptions ConfigureProfile(this SpectreLoggerOptions options,
            LogLevel logLevel,
            Action<FormattingProfile> configure)
        {
            if (logLevel == LogLevel.None)
            {
                throw new ArgumentException("There is no formatting profile for LogLevel.None", nameof(logLevel));
            }
            
            configure(options.FormattingProfiles[logLevel]);
            return options;
        }

        /// <summary>
        /// Adds a filter to the configuration.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="filter">The filter to evaluate.</param>
        /// <returns><see cref="SpectreLoggerOptions"/></returns>
        public static SpectreLoggerOptions AddFilter(this SpectreLoggerOptions options, LogEventFilter filter)
        {
            options.Filters.Add(filter);
            return options;
        }

        /// <summary>
        /// Adds a filter that suppresses events of specific categories where the log level is below the given
        /// value. 
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="categoryName">The category name that is matched</param>
        /// <param name="minimumLevel">The minimum level event to render.</param>
        /// <returns><see cref="SpectreLoggerOptions"/></returns>
        public static SpectreLoggerOptions AddFilter(this SpectreLoggerOptions options,
            string categoryName,
            LogLevel minimumLevel)
        {
            return options.AddFilter((in LogEventInfo e) => e.LogLevel < minimumLevel 
                                                            && Regex.IsMatch(e.CategoryName, categoryName));
        }

        /// <summary>
        /// Adds a filter that suppresses events of loggers where the category name matches a type name.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="minimumLevel">The minimum level event to render.</param>
        /// <typeparam name="T">The logger type</typeparam>
        /// <returns><see cref="SpectreLoggerOptions"/></returns>
        public static SpectreLoggerOptions AddFilter<T>(this SpectreLoggerOptions options,
            LogLevel minimumLevel)
        {
            return options.AddFilter($"^{typeof(T).FullName}$", minimumLevel);
        }

        /// <summary>
        /// Adds a filter that suppresses events of a specific event id where the log level is below the given
        /// value.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="eventId">Event id to match.</param>
        /// <param name="minimumLevel">The minimum level event to render.</param>
        /// <returns><see cref="SpectreLoggerOptions"/></returns>
        public static SpectreLoggerOptions AddFilter(this SpectreLoggerOptions options,
            EventId eventId,
            LogLevel minimumLevel)
        {
            return options.AddFilter((in LogEventInfo e) => e.LogLevel < minimumLevel && eventId.Equals(eventId));
        }
    }
}