using System;
using System.Drawing;
using System.Text;
using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Default <see cref="IWriteBuffer"/> that outputs to the console.
    /// </summary>
    internal class AnsiConsoleBuffer : IWriteBuffer
    {
        private readonly DefaultWriteBufferFactory _factory;
        private readonly IAnsiConsole _ansiConsole;
        private readonly StringBuilder _stringBuilder = new(1024);

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="ansiConsole">Ansi console target</param>
        public AnsiConsoleBuffer(DefaultWriteBufferFactory factory, IAnsiConsole ansiConsole)
        {
            _factory = factory;
            _ansiConsole = ansiConsole;
        }

        /// <inheritdoc />
        public void Append(string str)
        {
            foreach (var c in str)
            {
                Append(c);
            }
        }

        /// <inheritdoc />
        public void Append(char c)
        {
            _stringBuilder.Append(c);

            if (c == '\n' && Margin > 0)
            {
                _stringBuilder.Append(' ', Margin);
            }

            AtMargin = c == '\n';
        }

        /// <inheritdoc />
        public void Clear() => _stringBuilder.Clear();

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
        public bool AtMargin { get; private set; }


        /// <inheritdoc />
        public int Margin { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            Clear();
            Margin = 0;
            _factory.BufferDisposed(this);
        }

        /// <summary>
        /// Gets the current content in the internal string builder.
        /// </summary>
        public string Content => _stringBuilder.ToString();
    }
}