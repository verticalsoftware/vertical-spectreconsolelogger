using System;

namespace Vertical.SpectreLogger.Rendering
{
    public partial class ExceptionRenderer
    {
        /// <summary>
        /// Options for <see cref="ExceptionRenderer"/>
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Gets or sets whether to display sub exceptions within an <see cref="AggregateException"/>
            /// </summary>
            public bool UnwindAggregateExceptions { get; set; } = true;

            /// <summary>
            /// Gets or sets whether to unwind inner exceptions.
            /// </summary>
            public bool UnwindInnerExceptions { get; set; } = true;

            /// <summary>
            /// Gets or sets the maximum number of stack frames to show per exception.
            /// </summary>
            public int MaxFrames { get; set; } = 5;
            
            /// <summary>
            /// Gets or sets markup to apply when rendering the exception name.
            /// </summary>
            public string? ExceptionNameStyle { get; set; }
            
            /// <summary>
            /// Gets or sets markup to apply when rendering the exception message.
            /// </summary>
            public string? MessageStyle { get; set; }
            
            /// <summary>
            /// Gets or sets markup to apply when rendering a stack frame.
            /// </summary>
            public string? StackFrameStyle { get; set; }
            
            /// <summary>
            /// Gets or sets markup to apply when rendering the method name of a stack frame. 
            /// </summary>
            public string? MethodNameStyle { get; set; }
            
            /// <summary>
            /// Gets or sets markup to apply when rendering the parameter type of a method of
            /// a stack frame.
            /// </summary>
            public string? ParameterTypeStyle { get; set; }
            
            /// <summary>
            /// Gets or sets markup to apply when rendering the parameter name of a method of
            /// a stack frame.
            /// </summary>
            public string? ParameterNameStyle { get; set; }

            /// <summary>
            /// Gets or sets whether to display the parameter type in stack frame methods.
            /// </summary>
            public bool ShowParameterTypes { get; set; } = true;

            /// <summary>
            /// Gets or sets whether to display the parameter name in stack frame methods.
            /// </summary>
            public bool ShowParameterNames { get; set; } = true;

            /// <summary>
            /// Gets or sets whether to display the file name of a stack frame method.
            /// </summary>
            public bool ShowSourcePaths { get; set; } = true;

            /// <summary>
            /// Gets or sets whether to display the line number in files names of a stack frame
            /// method.
            /// </summary>
            public bool ShowSourceLocations { get; set; } = true;
            
            /// <summary>
            /// Gets or sets markup to apply when rendering a source path.
            /// </summary>
            public string? SourcePathStyle { get; set; }
            
            /// <summary>
            /// Gets or sets markup to apply when rendering the line number of a source.
            /// </summary>
            public string? SourceLocationStyle { get; set; }
            
            /// <summary>
            /// Gets or sets a function that formats the method name to display.
            /// </summary>
            public Func<string, string>? MethodNameFormatter { get; set; }
            
            /// <summary>
            /// Gets or sets a function that formats the source path to display. 
            /// </summary>
            public Func<string, string>? SourcePathFormatter { get; set; }
        }
    }
}