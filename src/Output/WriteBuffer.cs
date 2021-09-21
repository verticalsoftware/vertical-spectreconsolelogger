using System;
using System.Text;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Represents the default implementation of a <see cref="IWriteBuffer"/>.
    /// </summary>
    internal class WriteBuffer : IWriteBuffer
    {
        private readonly IConsoleWriter _consoleWriter;
        private readonly StringBuilder _buffer = new();
        private int _margin;
        private char _lastChar;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="consoleWriter">Underlying console to flush output to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="consoleWriter"/> is null.</exception>
        public WriteBuffer(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter ?? throw new ArgumentNullException(nameof(consoleWriter));
        }

        /// <inheritdoc />
        public int Margin
        {
            get => _margin;
            set => _margin = Math.Max(0, value);
        }

        /// <inheritdoc />
        public int LinePosition { get; private set; }
        
        /// <inheritdoc />
        public void Write(char c)
        {
            if (_lastChar == '\n' && Margin > 0)
            {
                // Set the margin
                _buffer.Append(' ', Margin);
                LinePosition = Margin;
            }
            
            _buffer.Append(c);
            _lastChar = c;

            LinePosition = c == '\n' ? 0 : LinePosition + 1;
        }


        /// <inheritdoc />
        public void Write(string str) => Write(str, 0, str.Length);

        /// <inheritdoc />
        public void Write(string str, int startIndex, int length)
        {
            var pastLastIndex = startIndex + length;

            for (var c = startIndex; c < pastLastIndex; c++)
            {
               Write(str[c]);
            }
        }

        /// <inheritdoc /> 
        public int Length => _buffer.Length;

        /// <inheritdoc />
        public void Flush()
        {
            _consoleWriter.Write(_buffer.ToString());
            _buffer.Clear();
            _margin = 0;
        }

        /// <inheritdoc />
        public override string ToString() => _buffer.ToString();
    }
}