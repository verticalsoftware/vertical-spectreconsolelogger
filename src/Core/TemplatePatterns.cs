namespace Vertical.SpectreLogger.Core
{
    public static class TemplatePatterns
    {
        public const string WidthCaptureGroup = "width";
        public const string CompositeFormatCaptureGroup = "format";
        public const string WidthCapturePattern = @"(?:,(?<"+ WidthCaptureGroup + @">-?\d+))?";
        public const string CompositeFormatPattern = @"(?::(?<" + CompositeFormatCaptureGroup + @">[^\}]+))?";
    }
}