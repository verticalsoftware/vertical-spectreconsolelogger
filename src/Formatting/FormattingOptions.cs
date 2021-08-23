using System;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Defines how formatting and styling is applied.
    /// </summary>
    [Flags]
    public enum FormattingOptions
    {
        None = 0,
        
        ValueStyle = 1,
        
        TypeStyle = 2 << 0,
        
        DefaultStyle = 2 << 1,
        
        ValueFormat = 2 << 2,
        
        TypeFormat = 2 << 3,
        
        DefaultFormat = 2 << 4,
        
        TemplateFormat = 2 << 5,
        
        TemplateWidth = 2 << 6,
        
        AnyStyle = ValueStyle | TypeStyle | DefaultStyle,
        
        AnyFormat = ValueFormat | TypeFormat | TemplateFormat | DefaultFormat | TemplateWidth,
        
        CompositeFormat = TemplateFormat | TemplateWidth,
        
        All = AnyStyle | AnyFormat | CompositeFormat
    }
}