using System;
using System.Collections.Concurrent;
using System.Threading;
using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    internal class BackgroundConsoleWriter : ConsoleWriter, IConsoleWriter, IDisposable
    {
        private const int MaxQueuedMessages = 1024;
        
        private readonly BlockingCollection<string> _queue = new(MaxQueuedMessages);
        private readonly Thread _outputThread;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="ansiConsole">AnsiConsole</param>
        public BackgroundConsoleWriter(IAnsiConsole ansiConsole) : base(ansiConsole)
        {
            _outputThread = new Thread(MessagePump)
            {
                IsBackground = true,
                Name = "SpectreConsole message processing thread"
            };
            _outputThread.Start();
        }

        private void MessagePump()
        {
            try
            {
                foreach (var entry in _queue.GetConsumingEnumerable())
                {
                    WriteToConsole(entry);
                }
            }
            catch
            {
                try
                {
                    _queue.CompleteAdding();
                }
                catch
                {
                    // Ignored
                }
            }
        }

        public void Dispose()
        {
            _queue.CompleteAdding();

            try
            {
                _outputThread.Join(1500);
            }
            catch (ThreadStateException)
            {
                // Ignored because there is no recovery
            }
        }

        /// <inheritdoc />
        public void Write(string content)
        {
            if (_queue.IsAddingCompleted)
            {
                WriteToConsole(content);
                return;
            }
            
            _queue.Add(content);
        }
    }
}