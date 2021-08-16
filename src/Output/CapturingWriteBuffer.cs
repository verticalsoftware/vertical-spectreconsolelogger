namespace Vertical.SpectreLogger.Output
{
    public class CapturingWriteBuffer : WriteBuffer
    {
        public CapturingWriteBuffer() : base(100)
        {
        }
        
        /// <inheritdoc />
        public override void Dispose()
        {
        }

        /// <inheritdoc />
        public override void Flush()
        {
        }
    }
}