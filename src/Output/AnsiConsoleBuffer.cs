using System;
using System.Text;
using Spectre.Console;
using Vertical.SpectreLogger.Memory;

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
        /// <param name="bufferFactory"></param>
        /// <param name="ansiConsole">Ansi console target</param>
        public AnsiConsoleBuffer(DefaultWriteBufferFactory bufferFactory, IAnsiConsole ansiConsole)
        {
            _stringBuilderPool = stringBuilderPool;
            _ansiConsole = ansiConsole;
            _stringBuilder = stringBuilderPool.Get();
        }

        /// <inheritdoc />
        public void Append(string str) => _stringBuilder.Append(str);

        /// <inheritdoc />
        public void Append(char c) => _stringBuilder.Append(c);

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

        /// <summary>
        /// Gets the current content in the internal string builder.
        /// </summary>
        public string Content => _stringBuilder.ToString();
    }
}