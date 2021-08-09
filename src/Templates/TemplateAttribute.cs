using System;

namespace Vertical.SpectreLogger.Templates
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="template">The template.</param>
        public TemplateAttribute(string template)
        {
            Template = template;
        }

        /// <summary>
        /// Gets the template value.
        /// </summary>
        public string Template { get; }
    }
}