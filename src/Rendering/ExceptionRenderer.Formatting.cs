using System;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Rendering
{
    public partial class ExceptionRenderer
    {
        /// <summary>
        /// Wraps the exception name emitted value.
        /// </summary>
        public sealed class ExceptionNameValue : ValueWrapper<Type>
        {
            internal ExceptionNameValue(Type exceptionType): base(exceptionType) {}
        }

        /// <summary>
        /// Wraps the exception message emitted value.
        /// </summary>
        public sealed class ExceptionMessageValue : ValueWrapper<string>
        {
            internal ExceptionMessageValue(string message) : base(message) {}
        }

        /// <summary>
        /// Wraps the method name of a stack frame.
        /// </summary>
        public sealed class MethodNameValue : ValueWrapper<string>
        {
            internal MethodNameValue(string name) : base(name) {}
        }

        /// <summary>
        /// Wraps the parameter type name of a stack frame method.
        /// </summary>
        public sealed class ParameterTypeValue : ValueWrapper<string>
        {
            internal ParameterTypeValue(string type) : base(type) {}
        }

        /// <summary>
        /// Wraps the parameter name of a stack frame method.
        /// </summary>
        public sealed class ParameterNameValue : ValueWrapper<string>
        {
            internal ParameterNameValue(string name) : base(name) {}
        }

        /// <summary>
        /// Wraps the directory name of a stack frame method.
        /// </summary>
        public sealed class SourceDirectoryValue : ValueWrapper<string>
        {
            internal SourceDirectoryValue(string path) : base(path) {}
        }

        /// <summary>
        /// Wraps the file name of a stack frame method.
        /// </summary>
        public sealed class SourceFileValue : ValueWrapper<string>
        {
            internal SourceFileValue(string file) : base(file) {}
        }

        /// <summary>
        /// Wraps the source line number of a stack frame method.
        /// </summary>
        public sealed class SourceLocationValue : ValueWrapper<int>
        {
            internal SourceLocationValue(int lineNumber) : base(lineNumber) {}
        }
    }
}