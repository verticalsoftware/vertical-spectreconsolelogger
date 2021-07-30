using System.Collections.Generic;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Represents a structured view of a stack frame.
    /// </summary>
    public class StackFrameInfo
    {
        internal StackFrameInfo(
            string methodName,
            IReadOnlyCollection<(string Type, string Name)> parameters,
            string sourcePath,
            int sourceLineNumber)
        {
            MethodName = methodName;
            Parameters = parameters;
            SourcePath = sourcePath;
            SourceLineNumber = sourceLineNumber;
        }

        /// <summary>
        /// Gets the method name.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Gets a collection of tuples that contain the ordered collection
        /// of method parameters.
        /// </summary>
        public IReadOnlyCollection<(string Type, string Name)> Parameters { get; }

        /// <summary>
        /// Gets the source path.
        /// </summary>
        public string SourcePath { get; }

        /// <summary>
        /// Gets the line number.
        /// </summary>
        public int SourceLineNumber { get; }
    }
}