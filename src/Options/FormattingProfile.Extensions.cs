using System;

namespace Vertical.SpectreLogger.Options
{
    public static class FormattingProfileExtensions
    {
        /// <summary>
        /// Adds a function that formats a specific value.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <param name="value">The value to format</param>
        /// <param name="formatter">A function that receives the object and returns the string representation to render.</param>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddValueFormatter<T>(
            this FormattingProfile formattingProfile, 
            T value, 
            Func<T, string> formatter)
            where T : notnull
        {
            formattingProfile.ValueFormatters[value] = obj => formatter((T)obj);
            return formattingProfile;
        }
    }
}