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
        
        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <typeparam name="T">Type to markup</typeparam>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeStyle<T>(this FormattingProfile formattingProfile,
            string markup) => formattingProfile.AddTypeStyle(typeof(T), markup);
        
        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <param name="type">Type to markup</param>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeStyle(this FormattingProfile formattingProfile, 
            Type type, 
            string markup)
        {
            formattingProfile.TypeStyles[type] = markup;
            return formattingProfile;
        }

        /// <summary>
        /// Clears all type style formatting.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ClearTypeStyles(this FormattingProfile formattingProfile)
        {
            formattingProfile.TypeStyles.Clear();
            return formattingProfile;
        }

        /// <summary>
        /// Adds a function that formats the output of a specific value type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <param name="type">The value type to format.</param>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddValueFormatter(this FormattingProfile formattingProfile,
            Type type,
            Func<object?, string?> formatter)
        {
            formattingProfile.ValueFormatters[type] = formatter;
            return formattingProfile;
        }

        /// <summary>
        /// Adds a function that formats the output of a specific value type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <typeparam name="T">The value type to format.</typeparam>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddValueFormatter<T>(this FormattingProfile formattingProfile,
            Func<object?, string?> formatter) => formattingProfile.AddValueFormatter(typeof(T), formatter);

        /// <summary>
        /// Clears all type value formatting functions.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ClearValueFormatters(this FormattingProfile formattingProfile)
        {
            formattingProfile.ValueFormatters.Clear();
            return formattingProfile;
        }

        /// <summary>
        /// Configures options for a specific rendering options type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <param name="configure">Action that configures the given options object.</param>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ConfigureRendererOptions<TOptions>(this FormattingProfile formattingProfile,
            Action<TOptions> configure) where TOptions : class, new()
        {
            if (!formattingProfile.RendererOptions.TryGetValue(typeof(TOptions), out var optionsObj))
            {
                optionsObj = new TOptions();
                formattingProfile.RendererOptions.Add(typeof(TOptions), optionsObj);
            }

            configure((TOptions) optionsObj);

            return formattingProfile;
        }

        /// <summary>
        /// Gets the current rendering options of the specified type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <typeparam name="TOptions">Options type</typeparam>
        /// <returns>The options instance or null if never configured.</returns>
        public static TOptions? GetRenderingOptions<TOptions>(this FormattingProfile formattingProfile) where TOptions: class =>
            formattingProfile.RendererOptions.GetValueOrDefault(typeof(TOptions)) as TOptions;
    }
}