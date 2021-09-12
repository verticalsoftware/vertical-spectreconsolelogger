using System;
using System.Collections.Generic;
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
        private static readonly char[] SignatureSplitChars = new[] {' ', ','};
        private static readonly string[] StackFrameSplitStrings = new[] {Environment.NewLine};
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            var rootException = context.Exception;
            
            if (rootException == null)
                return;

            var profile = context.Profile;
            var options = profile.RendererOptions.GetOptions<Options>();
            var stack = new Stack<(Exception exception, int level)>();
            var count = 0;
            
            stack.Push((rootException, 0));

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                switch (current)
                {
                    case { exception: AggregateException ae } when options.UnwindAggregateExceptions:
                        foreach (var item in ae.InnerExceptions)
                        {
                            stack.Push((item, current.level + 1));
                        }

                        break;
                    
                    case { exception: { InnerException: { } }} when options.UnwindInnerExceptions:
                        stack.Push((current.exception.InnerException, current.level));
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
                    
                    PrintNameAndMessage(buffer, profile, exception);

                    if (options.MaxStackFrames <= 0)
                        continue;

                    PrintStackTrace(buffer, profile, exception, options, level);
                }
                finally
                {
                    buffer.Margin -= options.StackFrameIndent * level;
                }

                count++;
            }
        }

        private static void PrintNameAndMessage(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            Exception exception)
        {
            buffer.WriteLogValue(profile, null, new ExceptionNameValue(exception.GetType()), value =>
            {
                buffer.Write(value);
                buffer.Write(": ");
            });
            
            buffer.WriteLogValue(profile, null, new ExceptionMessageValue(exception.Message));
        }

        private static void PrintStackTrace(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            Exception exception, 
            Options options, 
            int level)
        {
            if (string.IsNullOrWhiteSpace(exception.StackTrace))
                return;
            
            try
            {
                buffer.Margin += options.StackFrameIndent;

                var frames = exception.StackTrace.Split(StackFrameSplitStrings, StringSplitOptions.None);
                var length = Math.Min(frames.Length, options.MaxStackFrames);

                for (var c = 0; c < length; c++)
                {
                    PrintStackFrame(buffer, profile, frames[c], options);
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
            var frameMatch = Regex.Match(frame, @"at (?<_method>[^(]+)\((?<_sig>.+)?\)(?<_src> in (?<_file>.+)(?::line (?<_line>\d+)))?");

            buffer.WriteLine();
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

            PrintSource(buffer, profile, frameMatch, options);
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
        
        private static void PrintSource(
            IWriteBuffer buffer, 
            LogLevelProfile profile, 
            Match frameMatch, 
            Options options)
        {
            var file = frameMatch.Groups["_file"].Value;
            var hasLineNumber = int.TryParse(frameMatch.Groups["_line"].Value, out var line);
            
            buffer.WriteLogValue(profile, null, new SourcePathValue(file), value =>
            {
                buffer.Write(" in ");
                buffer.Write(value);

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