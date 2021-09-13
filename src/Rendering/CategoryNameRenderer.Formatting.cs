using System;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Rendering
{
    public partial class CategoryNameRenderer
    {
        /// <summary>
        /// Wrapping type for the category name.
        /// </summary>
        public class Value : ValueWrapper<string>
        {
            internal Value(string value) : base(value)
            {
            }
        }
        
        /// <summary>
        /// Custom formatter for category name.
        /// </summary>
        [TypeFormatter(typeof(Value))]
        public class DefaultFormatter : ICustomFormatter
        {
            /// <inheritdoc />
            public virtual string Format(string? format, object? arg, IFormatProvider? formatProvider)
            {
                if (arg == null)
                {
                    return string.Empty;
                }

                var categoryName = ((Value) arg).Value;

                if (format == null)
                {
                    return categoryName;
                }

                var formatPattern = Regex.Match(format, @"([CS])(\d+)?");
                
                var countParam = formatPattern.Groups[2].Success
                    ? int.Parse(formatPattern.Groups[2].Value)
                    : (int?) null;

                if (!formatPattern.Success)
                {
                    return categoryName;
                }

                switch (formatPattern.Groups[1].Value)
                {
                    case "C" when !countParam.HasValue:
                        // Compact formatting - last component only
                        var dot = categoryName.LastIndexOf('.');
                        return dot > -1 && dot < categoryName.Length ? categoryName.Substring(dot+1) : categoryName;
                    
                    case "S" when countParam.HasValue:
                        // Compact formatting with max parts
                        var index = categoryName.Length;
                        while (countParam!.Value > 0 && index >= 0)
                        {
                            index = categoryName.LastIndexOf('.', index-1);
                            
                            if (index != -1)
                            {
                                countParam--;
                            }
                        }

                        if (index > -1) index++;

                        return categoryName.Substring(Math.Max(index, 0));
                }

                return categoryName;
            }
        }
    }
}