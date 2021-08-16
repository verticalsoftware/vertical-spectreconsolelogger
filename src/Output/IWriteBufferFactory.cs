namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Provides <see cref="IWriteBuffer"/> instances.
    /// </summary>
    public interface IWriteBufferFactory
    {
        IWriteBuffer GetInstance();

        void WriteDisposed(IWriteBuffer writeBuffer);
    }
}