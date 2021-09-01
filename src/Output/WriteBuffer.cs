using System;
using System.Text;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Represents the default implementation of a <see cref="IWriteBuffer"/>.
    /// </summary>
    public class WriteBuffer : IWriteBuffer
    {
        private readonly IAnsiConsoleWriter _consoleWriter;
        private readonly StringBuilder _queue = new();
        private readonly StringBuilder _buffer = new();

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="consoleWriter">Underlying console to flush output to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="consoleWriter"/> is null.</exception>
        public WriteBuffer(IAnsiConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter ?? throw new ArgumentNullException(nameof(consoleWriter));
        }
        
        /// <inheritdoc />
        public int Margin { get; set; }

        /// <inheritdoc />
        public int LinePosition { get; private set; }

        /// <inheritdoc />
        public void Enqueue(string str, int startIndex, int length)
        {
            _queue.Append(str, startIndex, length);
        }

        /// <inheritdoc />
        public void Write(string str, int startIndex, int length)
        {
            if (_queue.Length > 0)
            {
                _buffer.Append(_queue);
                _queue.Clear();
            }

            var pastLastIndex = startIndex + length;

            for (var c = startIndex; c < pastLastIndex; c++)
            {
                var ch = str[c];

                if (ch == '\n')
                {
                    _buffer.AppendLine();
                    _buffer.Append(' ', Margin);
                    LinePosition = 0;
                }

                _buffer.Append(ch);
                ++LinePosition;
            }
        }

        /// <inheritdoc />
        public int Length => _buffer.Length;

        /// <inheritdoc />
        public void Flush()
        {
            _consoleWriter.Write(_buffer.ToString());
            Clear();
        }

        /// <inheritdoc />
        public void Clear()
        {
            _buffer.Clear();
            _queue.Clear();
            LinePosition = 0;
            Enqueue(MarginStrings.Instance[Margin], 0, Margin);
        }

        /// <inheritdoc />
        public override string ToString() => _buffer.ToString();
    }
}