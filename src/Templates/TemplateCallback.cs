namespace Vertical.SpectreLogger.Templates
{
    /// <summary>
    /// Defines a delegate that receives template segments during split operations.
    /// </summary>
    public delegate void TemplateCallback(in TemplateSegment segment);
}