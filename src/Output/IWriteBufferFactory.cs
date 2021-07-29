namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Manages instances of <see cref="IWriteBufferFactory"/>
    /// </summary>
    public interface IWriteBufferFactory
    {
        /// <summary>
        /// Gets a <see cref="IWriteBuffer"/> instance
        /// </summary>
        /// <returns><see cref="IWriteBuffer"/></returns>
        IWriteBuffer GetInstance();
    }
}