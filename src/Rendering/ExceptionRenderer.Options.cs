using System;

namespace Vertical.SpectreLogger.Rendering
{
    public partial class ExceptionRenderer
    {
        public class Options
        {
            private readonly string _instanceId = Guid.NewGuid().ToString("N").Substring(0, 4);
            
            /// <summary>
            /// Gets the number of character to indent stack frame output.
            /// </summary>
            public int StackFrameIndentChars { get; set; } = 2;

            /// <summary>
            /// Gets the maximum number of stack frames.
            /// </summary>
            public int MaxStackFrames { get; set; } = 5;

            /// <summary>
            /// Gets whether to show the hidden stack frame count.
            /// </summary>
            public bool ShowHiddenStackFrameCount { get; set; } = true;

            /// <summary>
            /// Gets whether to display all exceptions in an <see cref="System.AggregateException"/>
            /// </summary>
            public bool UnwindAggregateExceptions { get; set; } = true;

            /// <summary>
            /// Gets whether to recursively display all base exceptions.
            /// </summary>
            public bool UnwindInnerExceptions { get; set; } = true;

            /// <summary>
            /// Gets or sets a function that formats the exception type name.
            /// </summary>
            public Func<Type, string>? ExceptionNameFormatter { get; set; }

            /// <summary>
            /// Gets or sets a function that formats the source path.
            /// </summary>
            public Func<string, string>? SourcePathFormatter { get; set; }

            /// <summary>
            /// Gets or sets the markup to apply when rendering the exception name.
            /// </summary>
            public string? ExceptionNameStyle { get; set; }

            /// <summary>
            /// Gets or sets the markup to apply when rendering the exception message.
            /// </summary>
            public string? ExceptionMessageStyle { get; set; }

            /// <summary>
            /// Gets or sets the markup to apply to a stack trace frame.
            /// </summary>
            public string? StackFrameStyle { get; set; }

            /// <summary>
            /// Gets or sets whether to render source paths in the stack trace.
            /// </summary>
            public bool RenderSourcePaths { get; set; } = true;

            /// <summary>
            /// Gets or sets the mark to apply when rendering a stack frame source path.
            /// </summary>
            public string? SourcePathStyle { get; set; }

            /// <summary>
            /// Gets or sets a function that formats stack trace method names.
            /// </summary>
            public Func<string, string>? MethodNameFormatter { get; set; }

            /// <summary>
            /// Gets or sets the markup to apply when rendering a stack frame method name.
            /// </summary>
            public string? MethodNameStyle { get; set; }

            /// <summary>
            /// Gets or sets whether to render parameter types.
            /// </summary>
            public bool RenderParameterTypes { get; set; } = true;

            /// <summary>
            /// Gets or sets whether to render parameter names.
            /// </summary>
            public bool RenderParameterNames { get; set; } = true;

            /// <summary>
            /// Gets or sets the markup to apply when rendering a stack frame parameter type.
            /// </summary>
            public string? ParameterTypeStyle { get; set; }

            /// <summary>
            /// Gets or sets the markup to apply when rendering a stack frame parameter name.
            /// </summary>
            public string? ParameterNameStyle { get; set; }

            public bool RenderSourceLineNumbers { get; set; } = true;
            
            /// <summary>
            /// Gets or sets the markup to apply when rendering a stack frame line number.
            /// </summary>
            public string? SourceLineNumberStyle { get; set; }
        }
    }
}