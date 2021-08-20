using System;

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
            Func<T, string> formatter)
            where T : notnull
        {
            profile.TypeFormatters[typeof(T)] = obj => formatter((T)obj);
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
    }
}