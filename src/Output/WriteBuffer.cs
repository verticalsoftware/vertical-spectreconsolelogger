using System;
using System.Text;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Represents the default implementation of a <see cref="IWriteBuffer"/>.
    /// </summary>
    internal class WriteBuffer : IWriteBuffer
    {
        private readonly IConsoleWriter _consoleWriter;
        private readonly WriteBufferPool _owner;
        private readonly StringBuilder _queue = new();
        private readonly StringBuilder _buffer = new();

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="consoleWriter">Underlying console to flush output to.</param>
        /// <param name="owner">Owning buffer pool</param>
        /// <exception cref="ArgumentNullException"><paramref name="consoleWriter"/> is null.</exception>
        public WriteBuffer(IConsoleWriter consoleWriter, WriteBufferPool owner)
        {
            _consoleWriter = consoleWriter ?? throw new ArgumentNullException(nameof(consoleWriter));
            _owner = owner;
        }

        /// <summary>
        /// Releases the write buffer back to the pool.
        /// </summary>
        public void Dispose()
        {
            _owner.Disposed(this);
        }
        
        /// <inheritdoc />
        public int Margin { get; set; }

        /// <inheritdoc />
        public int LinePosition { get; private set; }

        /// <inheritdoc />
        public void Enqueue(string str)
        {
            _queue.Append(str);
        }

        /// <inheritdoc />
        public void Write(string str) => Write(str, 0, str.Length);

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

                _buffer.Append(ch);
                ++LinePosition;
                
                if (ch == '\n')
                {
                    _buffer.Append(' ', Margin);
                    LinePosition = 0;
                } 
            }
        }

        /// <inheritdoc /> 
        public int Length => _buffer.Length;

        /// <inheritdoc />
        public void Flush()
        {
            _consoleWriter.Write(_buffer.ToString());
            Clear();
            Enqueue(MarginStrings.Instance[Margin]);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _buffer.Clear();
            _queue.Clear();
            LinePosition = 0;
        }

        /// <inheritdoc />
        public override string ToString() => _buffer.ToString();
    }
}