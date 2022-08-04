using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the event exception.
    /// </summary>
    [Template("{Exception}")]
    public partial class ExceptionRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            var rootException = context.Exception;
            
            if (rootException == null)
                return;
            
            var profile = context.Profile;
            var options = profile.ConfiguredOptions.GetOptions<Options>();
            var stack = new Stack<(Exception exception, int level, int aggregateChildId)>();
            var count = 0;
            
            stack.Push((rootException, 0, 0));

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                switch (current)
                {
                    case { exception: AggregateException ae } when options.UnwindInnerExceptions: 
                        var childId = 0;
                        foreach (var item in ae.InnerExceptions)
                        {
                            stack.Push((item, current.level + 1, ++childId));
                        }
                        break;
                    
                    case { exception: { InnerException: { } }} when options.UnwindInnerExceptions:
                        stack.Push((current.exception.InnerException, current.level, 0));
                        break;
                }

                var exception = current.exception;
                var level = current.level;

                buffer.Margin += options.StackFrameIndent * level;

                try
                {
                    if (count > 0)
                    {
                        buffer.WriteLine();
                    }
                    
                    PrintNameAndMessage(buffer, profile, exception, current.aggregateChildId);

                    if (options.MaxStackFrames <= 0)
                        continue;

                    PrintStackTrace(buffer, profile, exception, options);
                }
                finally
                {
                    buffer.Margin -= options.StackFrameIndent * level;
                }

                count++;
            }
            
            buffer.WriteLine();
        }

        private static void PrintNameAndMessage(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            Exception exception,
            int aggregateChildId)
        {
            buffer.WriteLogValue(profile, null, new ExceptionNameValue(exception.GetType()), value =>
            {
                if (aggregateChildId > 0)
                {
                    buffer.Write("-> ");    
                }
                
                buffer.Write(value);
                buffer.Write(": ");
            });
            
            buffer.WriteLogValue(profile, null, new ExceptionMessageValue(exception.Message));
        }

        private static void PrintStackTrace(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            Exception exception, 
            Options options)
        {
            if (string.IsNullOrWhiteSpace(exception.StackTrace))
                return;
            
            try
            {
                buffer.Margin += options.StackFrameIndent;

                var trace = new StackTrace(exception, fNeedFileInfo: true);
                var frames = trace.GetFrames();

                if (frames == null)
                    return;
                
                var length = Math.Min(frames.Length, options.MaxStackFrames);
                var hiddenCount = frames.Length - options.MaxStackFrames;

                for (var c = 0; c < length; c++)
                {
                    PrintStackFrame(buffer, profile, frames[c], options);
                }

                if (hiddenCount > 0)
                {
                    buffer.WriteLine();
                    buffer.WriteStyledValue(profile, new MethodNameValue($"+{hiddenCount} more..."));
                }
            }
            finally
            {
                buffer.Margin -= options.StackFrameIndent;
            }
        }
      
        private static void PrintStackFrame(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            StackFrame frame, 
            Options options)
        {
            buffer.WriteLine();

            var method = frame.GetMethod();

            if (method == null)
                return;

            buffer.WriteLogValue(profile, null, new MethodNameValue(method.Name), name =>
            {
                var formattedMethodType = TypeNameFormatter.Format(method.DeclaringType!);

                buffer.Write("at ");
                buffer.Write(formattedMethodType);
                buffer.Write('.');
                buffer.Write(name);

                PrintParameters(buffer, profile, frame, options);
            });

            PrintSourcePath(buffer, profile, frame, options);
        }

        private static void PrintParameters(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            StackFrame stackFrame, 
            Options options)
        {
            buffer.Write('(');
            
            var parameters = stackFrame.GetMethod()?.GetParameters();
            var c = 0;

            if (parameters == null)
                return;

            foreach (var parameter in parameters)
            {
                if (c != 0)
                {
                    buffer.Write(", ");
                }

                var spaceChar = false;

                if (options.ShowParameterTypes)
                {
                    var formattedTypeName = TypeNameFormatter.Format(parameter.ParameterType);
                    buffer.WriteLogValue(profile, null, new ParameterTypeValue(formattedTypeName));
                    spaceChar = true;
                }

                if (options.ShowParameterNames && !string.IsNullOrWhiteSpace(parameter.Name))
                {
                    if (spaceChar)
                    {
                        buffer.Write(' ');
                    }

                    buffer.WriteLogValue(profile, null, new ParameterNameValue(parameter.Name));
                }

                c++;
            }

            buffer.Write(')');
        }
        
        private static void PrintSourcePath(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            StackFrame frame, 
            Options options)
        {
            if (!options.ShowSourcePaths)
                return;

            var path = frame.GetFileName();

            if (string.IsNullOrWhiteSpace(path))
                return;
            
            var directory = (Path.GetDirectoryName(path) ?? string.Empty) + Path.DirectorySeparatorChar;
            var file = Path.GetFileName(path);
            var lineNumber = frame.GetFileLineNumber();

            if (string.IsNullOrWhiteSpace(file))
                return;

            buffer.WriteLogValue(profile, null, new SourceDirectoryValue(directory), value =>
            {
                buffer.Write(" in ");
                buffer.Write(value);
                buffer.WriteLogValue(profile, null, new SourceFileValue(file));

                if (options.ShowSourceLocations)
                {
                    buffer.Write(":line ");
                }
            });

            if (!options.ShowSourceLocations)
                return;

            buffer.WriteLogValue(profile, null, new SourceLocationValue(lineNumber));
        }
    }
}