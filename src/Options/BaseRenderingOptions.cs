namespace Vertical.SpectreLogger.Options
{
    public abstract class BaseRenderingOptions
    {
        /// <summary>
        /// Gets or sets content to output before rendering. 
        /// </summary>
        public string? PreRenderContent { get; set; }
        
        /// <summary>
        /// Gets or sets content to output after rendering.
        /// </summary>
        public string? PostRenderContent { get; set; }
    }
}