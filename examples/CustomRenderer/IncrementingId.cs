using System;
using System.Text;
using System.Threading;

namespace CustomRenderer
{
    public readonly struct IncrementingId : IFormattable
    {
        private readonly int _increment;
        private readonly int _thread;
        private readonly int _process;
        private readonly int _epoch;
        private static int globalIncrement;
        
        private IncrementingId(int increment, int thread, int process, int epoch)
        {
            _increment = increment;
            _thread = thread;
            _process = process;
            _epoch = epoch;
        }

        public static IncrementingId Create() => new(
            Interlocked.Increment(ref globalIncrement),
            Thread.CurrentThread.ManagedThreadId,
            Environment.ProcessId,
            (int)DateTimeOffset.UtcNow.Subtract(DateTimeOffset.UnixEpoch).TotalSeconds);

        /// <inheritdoc />
        public override string ToString() => ToString("", null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var space = format == "N" ? string.Empty : "-";
            
            return new StringBuilder()
                .AppendFormat("{0:x}", (byte) (_epoch >> 24))
                .AppendFormat("{0:x}", (byte) (_epoch >> 16))
                .AppendFormat("{0:x}", (byte) (_epoch >> 8))
                .AppendFormat("{0:x}", (byte) _epoch)
                .AppendFormat(space)
                .AppendFormat("{0:x}", (byte) (_process >> 24))
                .AppendFormat("{0:x}", (byte) (_process >> 16))
                .AppendFormat("{0:x}", (byte) (_process >> 8))
                .AppendFormat("{0:x}", (byte) _process)
                .AppendFormat("{0:x}", (byte) (_thread >> 24))
                .AppendFormat("{0:x}", (byte) (_thread >> 16))
                .AppendFormat("{0:x}", (byte) (_thread >> 8))
                .AppendFormat("{0:x}", (byte) _thread)
                .AppendFormat(space)
                .AppendFormat("{0:x}", (byte) (_increment >> 24))
                .AppendFormat("{0:x}", (byte) (_increment >> 16))
                .AppendFormat("{0:x}", (byte) (_increment >> 8))
                .AppendFormat("{0:x}", (byte) _increment)
                .ToString();
        }
    }
}