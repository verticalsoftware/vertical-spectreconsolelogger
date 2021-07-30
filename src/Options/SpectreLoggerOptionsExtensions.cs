using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Internal;

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
            foreach (var logLevel in ConfigurableLogLevels)
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
            if (!options.FormattingProfiles.TryGetValue(logLevel, out var profile))
            {
                options.FormattingProfiles.Add(logLevel, profile = new FormattingProfile());
            }

            configure(profile);
            return options;
        }
        
        
    }
}