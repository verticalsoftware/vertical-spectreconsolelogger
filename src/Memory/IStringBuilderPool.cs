using System.Text;

namespace Vertical.SpectreLogger.Memory
{
    /// <summary>
    /// Object that pool instances of StringBuilder.
    /// </summary>
    public interface IStringBuilderPool
    {
        StringBuilder Get();
        void Return(StringBuilder builder);
    }
}