using System;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Options
{
    public static class FormattingProfileExtensions
    {
        /// <summary>
        /// Adds a display representation for a specific value type.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="value">The value to format</param>
        /// <param name="displayValue">The string value to render</param>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddValueFormatter<T>(
            this FormattingProfile profile, 
            T value, 
            string displayValue)
            where T : notnull
        {
            profile.ValueFormatters[value] = displayValue;
            return profile;
        }

        /// <summary>
        /// Adds a function that formats values of specific types.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="formatter">A function that receives the value and returns the string representation to render.</param>
        /// <typeparam name="T">The value type to format.</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeFormatter<T>(
            this FormattingProfile profile,
            IFormatter formatter)
            where T : notnull
        {
            profile.TypeFormatters[typeof(T)] = formatter;
            return profile;
        }

        /// <summary>
        /// Adds markup to apply to specific values.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="value">The specific value to apply styling markup to</param>
        /// <param name="style">The markup to apply</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddValueStyle<T>(
            this FormattingProfile profile,
            T value,
            string style)
            where T : notnull
        {
            profile.ValueStyles[value] = style;
            return profile;
        }

        /// <summary>
        /// Adds markup to apply to values of a specific type.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="style">The markup to apply</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeStyle<T>(
            this FormattingProfile profile,
            string style)
            where T : notnull
        {
            profile.TypeStyles[typeof(T)] = style;
            return profile;
        }

        /// <summary>
        /// Configures a renderer.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="configureOptions">A delegate used to configure the options instance.</param>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ConfigureRenderer<TOptions>(
            this FormattingProfile profile,
            Action<TOptions> configureOptions)
            where TOptions : class, new()
        {
            if (!profile.OptionsDictionary.TryGetValue(typeof(TOptions), out var options))
            {
                options = new TOptions();
                profile.OptionsDictionary.Add(typeof(TOptions), options);
            }

            configureOptions((TOptions) options);

            return profile;
        }

        /// <summary>
        /// Gets a specific instance of an options type.
        /// </summary>
        /// <param name="profile">Formatting profile.</param>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <returns>The options instance or null if the options were never configured.</returns>
        public static TOptions? GetRendererOptions<TOptions>(this FormattingProfile profile)
            where TOptions : class
        {
            return profile.OptionsDictionary.TryGetValue(typeof(TOptions), out var options)
                ? (TOptions) options
                : default;
        }
    }
}