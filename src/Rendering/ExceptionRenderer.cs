using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Exception}")]
    public partial class ExceptionRenderer : ITemplateRenderer
    {
        private static readonly char[] SignatureSplitChars = {' ', ','};
        private static readonly string[] StackFrameSplitStrings = {Environment.NewLine};

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

                var frames = exception.StackTrace.Split(StackFrameSplitStrings, StringSplitOptions.None);
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
            string frame, 
            Options options)
        {
            var frameMatch = Regex.Match(
                frame, 
                @"at (?<_method>[^(]+)\((?<_sig>.+)?\)(?<_src> in (?<_file>.+)(?::line (?<_line>\d+)))?");

            buffer.WriteLine();

            if (!frameMatch.Success)
            {
                buffer.Write(frame);
                return;
            }

            buffer.WriteLogValue(profile, null, new MethodNameValue(frameMatch.Groups["_method"].Value), method =>
            {
                buffer.Write("at ");
                buffer.Write(method);
                buffer.Write("(");

                var signature = frameMatch.Groups["_sig"];
                if (signature.Success)
                {
                    PrintParameters(buffer, profile, signature, options);
                }

                buffer.Write(")");
            });

            var source = frameMatch.Groups["_src"];
            
            if (!(source.Success && options.ShowSourcePaths))
                return;

            PrintSourcePath(buffer, profile, frameMatch, options);
        }

        private static void PrintParameters(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            Group signatureGroup, 
            Options options)
        {
            var signatureWords = signatureGroup.Value.Split(SignatureSplitChars, StringSplitOptions.RemoveEmptyEntries);
            var length = signatureWords.Length;

            for (var c = 0; c < length; c += 2)
            {
                if (c != 0)
                {
                    buffer.Write(", ");    
                }
                
                var spaceChar = false;

                if (options.ShowParameterTypes)
                {
                    buffer.WriteLogValue(profile, null, new ParameterTypeValue(signatureWords[c]));
                    spaceChar = true;
                }

                if (options.ShowParameterNames)
                {
                    if (spaceChar)
                    {
                        buffer.Write(' ');
                    }
                    buffer.WriteLogValue(profile, null, new ParameterNameValue(signatureWords[c+1]));
                }
            }
        }
        
        private static void PrintSourcePath(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            Match frameMatch, 
            Options options)
        {
            var path = frameMatch.Groups["_file"].Value;
            var directory = (Path.GetDirectoryName(path) ?? string.Empty) + Path.DirectorySeparatorChar;
            var file = Path.GetFileName(path);
            var hasLineNumber = int.TryParse(frameMatch.Groups["_line"].Value, out var line);

            buffer.WriteLogValue(profile, null, new SourceDirectoryValue(directory), value =>
            {
                buffer.Write(" in ");
                buffer.Write(value);
                buffer.WriteLogValue(profile, null, new SourceFileValue(file));

                if (hasLineNumber && options.ShowSourceLocations)
                {
                    buffer.Write(":line ");
                }
            });

            if (!(hasLineNumber && options.ShowSourceLocations))
                return;

            buffer.WriteLogValue(profile, null, new SourceLocationValue(line));
        }
    }
}