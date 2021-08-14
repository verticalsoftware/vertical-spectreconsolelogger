using System;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Default implementation of <see cref="IWriteBuffer"/> that outputs to
    /// <see cref="IAnsiConsole"/>
    /// </summary>
    internal class DefaultWriteBuffer : IWriteBuffer
    {
        private readonly IAnsiConsole _console;
        private readonly IWriteBufferProvider _provider;
        private readonly StringBuilder _stringBuilder = new StringBuilder(500);
        private int _indent;
        private int _linePosition;

        internal DefaultWriteBuffer(IAnsiConsole console, IWriteBufferProvider provider)
        {
            _console = console;
            _provider = provider;
        }

        public void Dispose() => _provider.WriteDisposed(this);

        /// <inheritdoc />
        public void Write(string str)
        {
            Write(str, 0, str.Length);
        }

        /// <inheritdoc />
        public void Write(string str, int index, int length)
        {
            for (var c = index; c < length; c++)
            {
                Write(str[c], 1);
            }
        }

        /// <inheritdoc />
        public void Write(char c, int count = 1)
        {
            _stringBuilder.Append(c, count);

            switch (c)
            {
                case '\n':
                    if (_indent > 0)
                    {
                        _stringBuilder.Append(' ', _indent);
                    }

                    _linePosition = 0;
                    break;
                
                default:
                    _linePosition += count;
                    break;
            }
        }

        /// <inheritdoc />
        public void Flush()
        {
            _console.Markup(_stringBuilder.ToString());
        }

        /// <inheritdoc />
        public void Clear()
        {
            _stringBuilder.Clear();
            _linePosition = 0;
        }

        /// <inheritdoc />
        public int Indent
        {
            get => _indent;
            set => _indent = Math.Max(0, value);
        }

        /// <inheritdoc />
        public int Length => _stringBuilder.Length;
    }
}