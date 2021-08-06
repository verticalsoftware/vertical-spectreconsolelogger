using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
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

        private const string MyTemplate = @"{Exception}";

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
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

            if (options.StackFrameMarkup != null)
            {
                buffer.WriteMarkup(options.StackFrameMarkup);
            }
            
            RenderInternal(buffer, options, 1, new[]{exception});

            if (options.StackFrameMarkup != null)
            {
                buffer.WriteMarkupClose();
            }
        }

        private void RenderInternal(IWriteBuffer buffer,
            ExceptionRenderingOptions options,
            int indent,
            IEnumerable<Exception> exceptions,
            bool writeNewLine = false)
        {
            foreach (var exception in exceptions)
            {
                RenderInternal(buffer, options, indent, exception, writeNewLine);

                switch (exception)
                {
                    case AggregateException aggregateException when options.UnwindAggregateExceptions:
                        RenderInternal(buffer, 
                            options, 
                            indent + 1, 
                            aggregateException.InnerExceptions,
                            true);
                        break;
                    
                    case { InnerException: {} } when options.UnwindInnerExceptions:
                        RenderInternal(buffer,
                            options,
                            indent,
                            exception.InnerException,
                            true);
                        break;
                }

                writeNewLine = true;
            }
        }

        private void RenderInternal(IWriteBuffer buffer, 
            ExceptionRenderingOptions options,
            int indent,
            Exception exception,
            bool writeNewLine)
        {
            if (writeNewLine)
            {
                buffer.WriteLine();
            }
            
            var type = exception.GetType();
            var exceptionName = options.ExceptionNameFormatter?.Invoke(type) ?? type.FullName!;
            
            // Render the exception name
            buffer.Write(exceptionName.EscapeMarkup(), options.ExceptionNameMarkup);
            
            buffer.Write(": ");
            
            // Render the message
            buffer.Write(exception.Message.EscapeMarkup(), options.ExceptionMessageMarkup);

            var stackFrames = exception
                .StackTrace?
                .Split(StackFrameSplitValues, StringSplitOptions.None);

            if (stackFrames == null)
                return;

            foreach (var stackFrame in stackFrames.Take(options.MaxStackFrames))
            { 
                buffer.WriteLine();
                buffer.WriteWhitespace(options.StackFrameIndentChars * indent);
                RenderStackFrame(buffer, options, stackFrame);
            }
        }

        private void RenderStackFrame(IWriteBuffer buffer, 
            ExceptionRenderingOptions options,
            string stackFrame)
        {
            var stackFrameInfo = StackFrameParser.Parse(stackFrame);
            var methodName = stackFrameInfo.MethodName.Replace('[', '<').Replace(']', '>');
            var formattedMethodName = options.MethodNameFormatter?.Invoke(methodName) ?? methodName;

            buffer.Write("  at ");
            buffer.Write(formattedMethodName.EscapeMarkup(), options.MethodNameMarkup);

            RenderParameters(buffer, options, stackFrameInfo);

            if (!options.RenderSourcePaths)
                return;

            if (stackFrameInfo.SourcePath.Length != 0)
            {
                buffer.Write(" in ");

                var path = options.SourcePathFormatter?.Invoke(stackFrameInfo.SourcePath) ?? stackFrameInfo.SourcePath;

                buffer.Write(path.EscapeMarkup(), options.SourcePathMarkup);
            
                buffer.Write(":line ");
                buffer.Write(stackFrameInfo.SourceLineNumber.ToString(), options.SourceLineNumberMarkup);
            }
        }

        private static void RenderParameters(IWriteBuffer buffer, 
            ExceptionRenderingOptions options,
            StackFrameInfo stackFrameInfo)
        {
            if (!options.RenderParameterNames && !options.RenderParameterTypes)
                return;
            
            buffer.Write('(');

            var separator = string.Empty;

            foreach (var (type, name) in stackFrameInfo.Parameters)
            {
                buffer.Write(separator);
                
                if (options.RenderParameterTypes)
                {
                    buffer.Write(type.EscapeMarkup(), options.ParameterTypeMarkup);
                }

                if (options.RenderParameterNames)
                {
                    if (options.RenderParameterTypes)
                    {
                        buffer.WriteWhitespace();    
                    }
                    
                    buffer.Write(name.EscapeMarkup(), options.ParameterNameMarkup);
                }

                separator = ", ";
            }
            
            buffer.Write(')');
        }
    }
}