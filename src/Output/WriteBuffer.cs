using System;
using System.Text;

namespace Vertical.SpectreLogger.Output
{
    public abstract class WriteBuffer : IWriteBuffer
    {
        private readonly StringBuilder _stringBuilder;
        private int _margin;

        protected WriteBuffer(int capacity)
        {
            _stringBuilder = new StringBuilder(capacity);
        }

        public abstract void Dispose();

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
                    if (_margin > 0)
                    {
                        _stringBuilder.Append(' ', _margin);
                    }

                    CharPosition = 0;
                    break;
                
                default:
                    CharPosition += count;
                    break;
            }
        }

        /// <inheritdoc />
        public abstract void Flush();

        /// <inheritdoc />
        public void Clear()
        {
            _stringBuilder.Clear();
            CharPosition = 0;
        }

        /// <inheritdoc />
        public int CharPosition { get; private set; }

        /// <inheritdoc />
        public int Margin
        {
            get => _margin;
            set => _margin = Math.Max(0, value);
        }

        /// <inheritdoc />
        /// <inheritdoc />
        public int Length => _stringBuilder.Length;

        /// <inheritdoc />
        public string ToString(int startIndex, int length)
        {
            return _stringBuilder.ToString(startIndex, length);
        }

        /// <summary>
        /// Gets the current value in the buffer.
        /// </summary>
        /// <inheritdoc />
        public override string ToString() => _stringBuilder.ToString();
    }
}