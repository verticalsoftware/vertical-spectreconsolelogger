namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Options used to render destructured items.
    /// </summary>
    public class DestructuringOptions
    {
        /// <summary>
        /// Gets or sets the max depth at which to descend in an object's
        /// property graph.
        /// </summary>
        /// <remarks>Defaults to a depth of 5.</remarks>
        public int MaxDepth { get; set; } = 5;

        /// <summary>
        /// Gets or sets the maximum number of items show from a collection.
        /// </summary>
        public int MaxCollectionItems { get; set; } = 10;
    }
}