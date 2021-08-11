using System;

namespace Vertical.SpectreLogger.Templates
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TemplateProviderAttribute : Attribute
    {
    }
}