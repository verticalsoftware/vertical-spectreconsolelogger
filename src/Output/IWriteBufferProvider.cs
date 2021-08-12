namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Provides <see cref="IWriteBuffer"/> instances.
    /// </summary>
    public interface IWriteBufferProvider
    {
        IWriteBuffer GetInstance();

        void WriteDisposed(IWriteBuffer writeBuffer);
    }
}