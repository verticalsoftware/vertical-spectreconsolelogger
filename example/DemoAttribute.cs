using System;

namespace SpectreLoggerExample
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DemoAttribute : Attribute
    {
        public string Description { get; }

        public DemoAttribute(string description)
        {
            Description = description;
        }
    }
}