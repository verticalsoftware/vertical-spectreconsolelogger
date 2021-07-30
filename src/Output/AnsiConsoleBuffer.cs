using System;
using System.Text;
using Spectre.Console;
using Vertical.SpectreLogger.Memory;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Default <see cref="IWriteBuffer"/> that outputs to the console.
    /// </summary>
    public class AnsiConsoleBuffer : IWriteBuffer
    {
        private readonly IStringBuilderPool _stringBuilderPool;
        private readonly IAnsiConsole _ansiConsole;
        private readonly StringBuilder _stringBuilder;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="stringBuilderPool">String builder pool.</param>
        /// <param name="ansiConsole">Ansi console target</param>
        public AnsiConsoleBuffer(IStringBuilderPool stringBuilderPool, IAnsiConsole ansiConsole)
        {
            _stringBuilderPool = stringBuilderPool;
            _ansiConsole = ansiConsole;
            _stringBuilder = stringBuilderPool.Get();
        }
        
        /// <inheritdoc />
        public void Append(FormattingProfile profile, string str)
        {
            // Indented write
            foreach (var c in str)
            {
                switch (c)
                {
                    case '\r':
                        break;
                    
                    case '\n':
                        AppendLine(profile);
                        break;
                    
                    case '[':
                    case ']':
                        _stringBuilder.Append(c, 2);
                        break;
                    
                    default:
                        _stringBuilder.Append(c);
                        break;
                }
            }
        }

        /// <inheritdoc />
        public void AppendWhitespace(int count = 1)
        {
            _stringBuilder.Append(' ', count);
        }

        /// <inheritdoc />
        public void AppendLine(FormattingProfile profile)
        {
            _stringBuilder.AppendLine();
        }

        /// <inheritdoc />
        public void AppendUnescaped(string content)
        {
            _stringBuilder.Append(content);
        }

        /// <inheritdoc />
        public void Flush()
        {
            try
            {
                _ansiConsole.Markup(_stringBuilder.ToString());
            }
            catch
            {
                Console.WriteLine("An exception was thrown while rendering markup to IAnsiConsole."
                                  + Environment.NewLine
                                  + "Buffer content being rendered is:"
                                  + Environment.NewLine
                                  + "   " + _stringBuilder);
                throw;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _stringBuilderPool.Return(_stringBuilder);
        }

        internal string Content => _stringBuilder.ToString();
    }
}