using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Extension methods for <see cref="SpectreLoggerOptions"/>
    /// </summary>
    public static class SpectreLoggerOptionsExtensions
    {
        /// <summary>
        /// Configures a formatting profile.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <param name="logLevel">The log level to apply the configuration changes to.</param>
        /// <param name="configure">An action that makes changes to the provided profile.</param>
        /// <returns><paramref name="options"/></returns>
        public static SpectreLoggerOptions ConfigureProfile(
            this SpectreLoggerOptions options,
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
        public static SpectreLoggerOptions ConfigureProfiles(
            this SpectreLoggerOptions options,
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
        public static SpectreLoggerOptions ConfigureProfiles(
            this SpectreLoggerOptions options,
            Action<FormattingProfile> configure)
        {
            foreach (var profile in options.FormattingProfiles.Values)
            {
                configure(profile);
            }

            return options;
        }

        /// <summary>
        /// Adds a renderer type.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <param name="type">The type of renderer.</param>
        /// <returns><paramref name="options"/></returns>
        internal static SpectreLoggerOptions AddRenderer(
            this SpectreLoggerOptions options,
            Type type)
        {
            options.RendererTypes.Add(type);
            return options;
        }

        /// <summary>
        /// Adds a renderer type.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <typeparam name="T">The type of renderer to add.</typeparam>
        /// <returns><paramref name="options"/></returns>
        /// <remarks>
        /// Renderers must implement <see cref="ITemplateRenderer"/> decorated with the <see cref="ITemplateRenderer"/> attribute.
        /// </remarks>
        public static SpectreLoggerOptions AddRenderer<T>(
            this SpectreLoggerOptions options) where T : ITemplateRenderer
        {
            return options.AddRenderer(typeof(T));
        }

        /// <summary>
        /// Adds all compatible renderers in an assembly.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <param name="assembly">The assembly where the types are located. If null, the calling assembly is used.</param>
        /// <returns><paramref name="options"/></returns>
        /// <remarks>
        /// Renderers must implement <see cref="ITemplateRenderer"/> decorated with the <see cref="ITemplateRenderer"/> attribute.
        /// </remarks>
        public static SpectreLoggerOptions AddRenderers(
            this SpectreLoggerOptions options,
            Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            var compatibleTypes = assembly
                .GetTypes()
                .Where(type => type.IsPublic && type.IsClass && !type.IsAbstract && typeof(ITemplateRenderer).IsAssignableFrom(type));

            foreach (var compatibleType in compatibleTypes)
            {
                options.AddRenderer(compatibleType);
            }

            return options;
        }
    }
}