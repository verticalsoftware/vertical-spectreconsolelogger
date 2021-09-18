using System;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Rendering
{
    public partial class ExceptionRenderer
    {
        public sealed class ExceptionNameValue : ValueWrapper<Type>
        {
            internal ExceptionNameValue(Type exceptionType): base(exceptionType) {}
        }

        public sealed class ExceptionMessageValue : ValueWrapper<string>
        {
            internal ExceptionMessageValue(string message) : base(message) {}
        }

        public sealed class MethodNameValue : ValueWrapper<string>
        {
            internal MethodNameValue(string name) : base(name) {}
        }

        public sealed class ParameterTypeValue : ValueWrapper<string>
        {
            internal ParameterTypeValue(string type) : base(type) {}
        }

        public sealed class ParameterNameValue : ValueWrapper<string>
        {
            internal ParameterNameValue(string name) : base(name) {}
        }

        public sealed class SourceDirectoryValue : ValueWrapper<string>
        {
            internal SourceDirectoryValue(string path) : base(path) {}
        }

        public sealed class SourceFileValue : ValueWrapper<string>
        {
            internal SourceFileValue(string file) : base(file) {}
        }

        public sealed class SourceLocationValue : ValueWrapper<int>
        {
            internal SourceLocationValue(int lineNumber) : base(lineNumber) {}
        }
    }
}