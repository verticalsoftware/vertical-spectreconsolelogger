using System;

namespace Vertical.SpectreLogger.Templates
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateParserAttribute : Attribute
    {
        public TemplateParserAttribute(string propertyOrFieldName)
        {
            PropertyOrFieldName = propertyOrFieldName;
        }

        public string PropertyOrFieldName { get; }
    }
}