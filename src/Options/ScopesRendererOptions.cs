namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Options that control how scopes are rendered.
    /// </summary>
    public class ScopesRendererOptions
    {
        /// <summary>
        /// Gets or sets the format to write before rendering the scopes.
        /// </summary>
        public string? PreRenderFormat { get; set; }
        
        /// <summary>
        /// Gets or sets the separator to render between each scope.
        /// </summary>
        public string? Separator { get; set; }
        
        /// <summary>
        /// Gets or sets the format to write after rendering scopes.
        /// </summary>
        public string? PostRenderFormat { get; set; }
        
        /// <summary>
        /// Gets or sets the format to write when there are no scopes.
        /// </summary>
        public string? DefaultFormat { get; set; }
        
        /// <summary>
        /// Gets or sets whether to render null scopes.
        /// </summary>
        public bool RenderNullScopes { get; set; }
    }
}