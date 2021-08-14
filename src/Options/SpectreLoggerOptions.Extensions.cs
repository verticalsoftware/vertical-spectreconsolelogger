using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Extension methods for <see cref="SpectreConsoleLoggerOptions"/>
    /// </summary>
    public static class SpectreConsoleLoggerOptionsExtensions
    {
        /// <summary>
        /// Configures a formatting profile.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <param name="logLevel">The log level to apply the configuration changes to.</param>
        /// <param name="configure">An action that makes changes to the provided profile.</param>
        /// <returns><paramref name="options"/></returns>
        public static SpectreConsoleLoggerOptions ConfigureProfile(
            this SpectreConsoleLoggerOptions options,
            LogLevel logLevel,
            Action<FormattingProfile> configure)
        {
            configure(options.FormattingProfiles[logLevel]);
            return options;
        }

        /// <summary>
        /// Configures a formatting profile.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <param name="logLevels">The log level(s) to apply the configuration changes to.</param>
        /// <param name="configure">An action that makes changes to the provided profile.</param>
        /// <returns><paramref name="options"/></returns>
        public static SpectreConsoleLoggerOptions ConfigureProfiles(
            this SpectreConsoleLoggerOptions options,
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
        /// Configures all formatting profiles.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <param name="configure">A action that makes changes to all profiles.</param>
        /// <returns><paramref name="options"/></returns>
        public static SpectreConsoleLoggerOptions ConfigureProfiles(
            this SpectreConsoleLoggerOptions options,
            Action<FormattingProfile> configure)
        {
            foreach (var profile in options.FormattingProfiles.Values)
            {
                configure(profile);
            }

            return options;
        }
    }
}