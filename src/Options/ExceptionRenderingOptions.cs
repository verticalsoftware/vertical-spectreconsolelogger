using System;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines options for rendering exceptions.
    /// </summary>
    public class ExceptionRenderingOptions
    {
        /// <summary>
        /// Gets the number of character to indent stack frame output.
        /// </summary>
        public int StackFrameIndentChars { get; set; } = 2;
        
        /// <summary>
        /// Gets the maximum number of stack frames.
        /// </summary>
        public int MaxStackFrames { get; set; } = 5;

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
        public Func<Type, string>? ExceptionNameFormatter { get; set; } = type => type.FullName!;

        /// <summary>
        /// Gets or sets a function that formats the source path.
        /// </summary>
        public Func<string, string>? SourcePathFormatter { get; set; } = path => path;
        
        /// <summary>
        /// Gets or sets the markup to apply when rendering the exception name.
        /// </summary>
        public string? ExceptionNameMarkup { get; set; }

        /// <summary>
        /// Gets or sets the markup to apply when rendering the exception message.
        /// </summary>
        public string? ExceptionMessageMarkup { get; set; }
        
        /// <summary>
        /// Gets or sets the markup to apply to a stack trace frame.
        /// </summary>
        public string? StackFrameMarkup { get; set; }

        /// <summary>
        /// Gets or sets whether to render source paths in the stack trace.
        /// </summary>
        public bool RenderSourcePaths { get; set; } = true;
        
        /// <summary>
        /// Gets or sets the mark to apply when rendering a stack frame source path.
        /// </summary>
        public string? SourcePathMarkup { get; set; }
        
        /// <summary>
        /// Gets or sets a function that formats stack trace method names.
        /// </summary>
        public Func<string, string>? MethodNameFormatter { get; set; }
        
        /// <summary>
        /// Gets or sets the markup to apply when rendering a stack frame method name.
        /// </summary>
        public string? MethodNameMarkup { get; set; }

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
        public string? ParameterTypeMarkup { get; set; }
        
        /// <summary>
        /// Gets or sets the markup to apply when rendering a stack frame parameter name.
        /// </summary>
        public string? ParameterNameMarkup { get; set; }
        
        /// <summary>
        /// Gets or sets the markup to apply when rendering a stack frame line number.
        /// </summary>
        public string? SourceLineNumberMarkup { get; set; }
    }
}