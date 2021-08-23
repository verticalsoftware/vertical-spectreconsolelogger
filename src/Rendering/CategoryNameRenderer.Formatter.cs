using System;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Rendering
{
    public partial class CategoryNameRenderer
    {
        private class Formatter : IFormatter
        {
            internal static readonly IFormatter Instance = new Formatter();
            
            /// <inheritdoc />
            public string Format(string format, object value)
            {
                var category = (string) value;

                switch (format)
                {
                    case "C":
                        // Class only
                        var lastDotIndex = category.LastIndexOf(category, '.');
                        return lastDotIndex > -1 ? category.Substring(lastDotIndex + 1) : category;
                    
                    case "P":
                        // Direct parent + class
                        var count = 0;
                        
                        for (var c = category.Length - 1; c >= 0; c--)
                        {
                            if (c == '.')
                            {
                                count++;
                            }

                            if (count == 2)
                            {
                                return category.Substring(c + 1);
                            }
                        }

                        return category;
                    
                    case "F":
                        // Full name
                        return category;
                    
                    default:
                        throw new InvalidOperationException($"Invalid format for CategoryName: '{format}'");
                }
            }
        }
    }
}