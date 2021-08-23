using System;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Controls how exceptions are rendered.
    /// </summary>
    public class ExceptionRendererOptions
    {
        /// <summary>
        /// Gets or sets the maximum number of stack frames to show.
        /// </summary>
        public int MaxStackFrames { get; set; } = 5;
        
        /// <summary>
        /// Gets or sets a function that is provided with the remaining number of stack
        /// frames and returns what to display as a continuation message.
        /// </summary>
        public Func<int, string>? HiddenStackFramesFormatter { get; set; }
        
        /// <summary>
        /// Gets or sets whether to unwind <see cref="AggregateException"/>.
        /// </summary>
        public bool UnwindAggregateExceptions { get; set; }
        
        /// <summary>
        /// Gets or sets whether to recursively unwind inner exceptions.
        /// </summary>
        public bool UnwindInnerExceptions { get; set; }
        
        /// <summary>
        /// Gets or sets a function that is provided with an exception instance and returns
        /// the string representation to display.
        /// </summary>
        public Func<Exception, string>? ExceptionFormatter { get; set; }
        
        /// <summary>
        /// Gets or sets a function that is provided with the method name and returns
        /// the string representation to display.
        /// </summary>
        public Func<string, string>? MethodFormatter { get; set; }
        
        /// <summary>
        /// Gets or sets a function that is provided with a stack frame parameter type
        /// and name tuple and returns the string representation to display.
        /// </summary>
        public Func<(string type, string name), string>? MethodParameterFormatter { get; set; }
        
        /// <summary>
        /// Gets or sets a function that is provided with a stack frame file location and
        /// returns the string representation to display.
        /// </summary>
        public Func<string, string>? FilePathFormatter { get; set; }
        
        /// <summary>
        /// Gets or sets a function that is provided with a stack frame file line number
        /// and returns the string representation to display.
        /// </summary>
        public Func<int, string>? FileLineNumberFormatter { get; set; }
        
        /// <summary>
        /// Gets or sets a function that is provided with the method name, file, and line number
        /// and returns the string representation to display.
        /// </summary>
        public Func<(string method, string file, string line), string>? StackFrameFormatter { get; set; }
    }
}