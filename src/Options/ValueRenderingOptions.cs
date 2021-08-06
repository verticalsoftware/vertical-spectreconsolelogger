using System;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines common options for a renderer.
    /// </summary>
    public abstract class ValueRenderingOptions<T> where T : notnull
    {
        /// <summary>
        /// Gets or sets a function that formats a value.
        /// </summary>
        public Func<T, string?>? Formatter { get; set; }
        
        /// <summary>
        /// Gets or sets markup that is applied to the output before rendering a value.
        /// </summary>
        public string? Style { get; set; }
    }
}