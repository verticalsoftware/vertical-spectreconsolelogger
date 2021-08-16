namespace Vertical.SpectreLogger.Core
{
    public class CompositeFormatter : IFormatter
    {
        public static readonly IFormatter Default = new CompositeFormatter();
        
        private CompositeFormatter()
        {
        }

        /// <inheritdoc />
        public string Format(string format, object value)
        {
            var formatString = $"{{0{format}}}";

            return string.Format(formatString, value);
        }
    }
}