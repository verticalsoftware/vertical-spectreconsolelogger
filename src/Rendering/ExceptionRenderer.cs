using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders exceptions.
    /// </summary>
    [Template(MyTemplate)]
    public class ExceptionRenderer : ITemplateRenderer
    {
        private static readonly string[] StackFrameSplitValues = {Environment.NewLine};

        private const string NewLineFormat = "NewLine";
        private const string MyTemplate = "{Exception(:(?<options>[^}]+))?}";

        private readonly string? _templateContext;

        public ExceptionRenderer(string? templateContext = null)
        {
            _templateContext = templateContext;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            var exception = eventInfo.Exception;

            if (exception == null)
                return;
            
            var profile = eventInfo.FormattingProfile;
            var options = profile.GetRenderingOptions<ExceptionRenderingOptions>()
                          ?? SpectreLoggerOptions
                              .Default
                              .FormattingProfiles[profile.LogLevel]
                              .GetRenderingOptions<ExceptionRenderingOptions>()!;
            var format = Regex
                .Match(profile.OutputTemplate!, MyTemplate)
                .Groups["options"]
                .Value;

            if (format.Contains("NewLine"))
            {
                buffer.AppendLine(profile);
            }

            if (options.StackFrameMarkup != null)
            {
                buffer.AppendMarkup(options.StackFrameMarkup);
            }
            
            RenderInternal(buffer, profile, options, 0, new[]{exception});

            if (options.StackFrameMarkup != null)
            {
                buffer.AppendMarkupCloseTag();
            }
        }

        private void RenderInternal(IWriteBuffer buffer,
            FormattingProfile profile,
            ExceptionRenderingOptions options,
            int indent,
            IEnumerable<Exception> exceptions)
        {
            foreach (var exception in exceptions)
            {
                RenderInternal(buffer, profile, options, indent, exception);

                if (options.UnwindAggregateExceptions && exception is AggregateException aggregateException)
                {
                    RenderInternal(buffer, 
                        profile, 
                        options, 
                        indent + 2, 
                        aggregateException.InnerExceptions);
                }
            }
        }

        private void RenderInternal(IWriteBuffer buffer, 
            FormattingProfile profile, 
            ExceptionRenderingOptions options,
            int indent,
            Exception exception)
        {
            var type = exception.GetType();
            var indentString = new string(' ', indent);
            
            // Render the exception name
            buffer.Append(profile, 
                options.ExceptionNameFormatter?.Invoke(type) ?? type.FullName!,
                options.ExceptionNameMarkup);
            
            buffer.Append(profile, ": ");
            
            // Render the message
            buffer.Append(profile,
                exception.Message,
                options.ExceptionMessageMarkup);

            var stackFrames = exception
                .StackTrace?
                .Split(StackFrameSplitValues, StringSplitOptions.None);

            if (stackFrames == null)
                return;

            foreach (var stackFrame in stackFrames.Take(options.MaxStackFrames))
            {
                buffer.AppendLine(profile);
                buffer.Append(profile, indentString);
                RenderStackFrame(buffer, profile, options, stackFrame);
            }
        }

        private void RenderStackFrame(IWriteBuffer buffer, 
            FormattingProfile profile, 
            ExceptionRenderingOptions options,
            string stackFrame)
        {
            var stackFrameInfo = StackFrameParser.Parse(stackFrame);
            var methodName = stackFrameInfo.MethodName.Replace('[', '<').Replace(']', '>');
            
            buffer.Append(profile, "  at ");
            buffer.Append(profile, 
                options.MethodNameFormatter?.Invoke(methodName) ?? methodName, 
                options.MethodNameMarkup);

            RenderParameters(buffer, profile, options, stackFrameInfo);

            if (!options.RenderSourcePaths)
                return;
            
            buffer.Append(profile, " in ");

            buffer.Append(profile, 
                options.SourcePathFormatter?.Invoke(stackFrameInfo.SourcePath) ?? stackFrameInfo.SourcePath,
                options.SourcePathMarkup);
            
            buffer.Append(profile, ":line ");
            buffer.Append(profile, stackFrameInfo.SourceLineNumber.ToString(), options.SourceLineNumberMarkup);
        }

        private static void RenderParameters(IWriteBuffer buffer, 
            FormattingProfile profile, 
            ExceptionRenderingOptions options,
            StackFrameInfo stackFrameInfo)
        {
            if (!options.RenderParameterNames && !options.RenderParameterTypes)
                return;
            
            buffer.Append(profile, "(");

            var separator = string.Empty;

            foreach (var (type, name) in stackFrameInfo.Parameters)
            {
                buffer.Append(profile, separator);
                
                if (options.RenderParameterTypes)
                {
                    buffer.Append(profile, type, options.ParameterTypeMarkup);
                }

                if (options.RenderParameterNames)
                {
                    if (options.RenderParameterTypes)
                    {
                        buffer.AppendWhitespace();    
                    }
                    
                    buffer.Append(profile, name, options.ParameterNameMarkup);
                }

                separator = ", ";
            }
            
            buffer.Append(profile, ")");
        }
    }
}