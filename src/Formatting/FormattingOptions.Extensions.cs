namespace Vertical.SpectreLogger.Formatting
{
    internal static class FormattingOptionsExtensions
    {
        public static bool ApplyValueStyle(this FormattingOptions options) => (options & FormattingOptions.ValueStyle) == FormattingOptions.ValueStyle;
        public static bool ApplyTypeStyle(this FormattingOptions options) => (options & FormattingOptions.TypeStyle) == FormattingOptions.TypeStyle;
        public static bool ApplyDefaultStyle(this FormattingOptions options) => (options & FormattingOptions.DefaultStyle) == FormattingOptions.DefaultStyle;
        public static bool ApplyValueFormat(this FormattingOptions options) => (options & FormattingOptions.ValueFormat) == FormattingOptions.ValueFormat;
        public static bool ApplyTypeFormat(this FormattingOptions options) => (options & FormattingOptions.TypeFormat) == FormattingOptions.TypeFormat;
        public static bool ApplyDefaultFormat(this FormattingOptions options) => (options & FormattingOptions.DefaultFormat) == FormattingOptions.DefaultFormat;
        public static bool ApplyTemplateFormat(this FormattingOptions options) => (options & FormattingOptions.TemplateFormat) == FormattingOptions.TemplateFormat;
        public static bool ApplyTemplateWidth(this FormattingOptions options) => (options & FormattingOptions.TemplateWidth) == FormattingOptions.TemplateWidth;
    }
}