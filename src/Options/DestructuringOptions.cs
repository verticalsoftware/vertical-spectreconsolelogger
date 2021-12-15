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
        /// Gets or sets the maximum number of items to show from a collection.
        /// </summary>
        public int MaxCollectionItems { get; set; } = 10;

        /// <summary>
        /// Gets or sets the maximum number of properties to show of an object.
        /// </summary>
        public int MaxProperties { get; set; } = 10;
        
        /// <summary>
        /// Gets or sets whether to pretty-print (e.g. indenting)
        /// </summary>
        public bool WriteIndented { get; set; }

        /// <summary>
        /// Gets or sets the number of spaces that comprise an indent.
        /// </summary>
        public int IndentSpaces { get; set; } = 4;
    }
}