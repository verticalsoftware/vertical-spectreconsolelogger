using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Types;

namespace Vertical.SpectreLogger.Utilities
{
    public static partial class WriteBufferExtensions
    {
        private static readonly HashSet<Type> IntegralTypes = new(
            TypeConstants.BooleanTypes
                .Concat(TypeConstants.CharTypes)
                .Concat(TypeConstants.NumericTypes)
                .Concat(TypeConstants.PointerTypes)
                .Concat(TypeConstants.TemporalTypes)
                .Concat(TypeConstants.UniqueIdentifierTypes));

        private static readonly ConcurrentDictionary<Type, Func<object, object>> 
            DestructuredValueFactories = new();

        private static readonly ConcurrentDictionary<Type, Func<IEnumerable, List<KeyValuePair<object, object>>>>
            DictionaryReaders = new();
        
        private static void WriteDestructuredValue(this IWriteBuffer buffer,
            object value,
            FormattingProfile profile,
            TemplateContext? templateContext,
            IFormatter? formatter,
            FormattingOptions options)
        {
            buffer.WriteDestructuredValue(
                value,
                profile,
                templateContext,
                formatter,
                options,
                0);
        }
        
        private static void WriteDestructuredValue(this IWriteBuffer buffer,
            object? value,
            FormattingProfile profile,
            TemplateContext? templateContext,
            IFormatter? formatter,
            FormattingOptions options,
            int depth)
        {
            var type = value?.GetType() ?? typeof(NullLogValue);
            
            switch (value)
            {
                case null:
                    buffer.WriteFormattedValue(
                        NullLogValue.Default, 
                        profile, 
                        templateContext, 
                        formatter, 
                        options);
                    break;
                
                case { } when IntegralTypes.Contains(type!):
                    var destructuredFactory = DestructuredValueFactories.GetOrAdd(
                        value.GetType(),
                        ExpressionFactories.CreateDestructedObjectFactory);
                    buffer.WriteFormattedValue(
                        destructuredFactory(value),
                        profile,
                        templateContext,
                        formatter,
                        options);
                    break;
                
                case { } when depth < profile.MaxDestructuringDepth && IsDictionary(type):
                    buffer.WriteDictionary(
                        (IEnumerable)value,
                        profile,
                        templateContext,
                        formatter,
                        options,
                        depth+1);
                    break;
            }
        }

        private static void WriteDictionary(this IWriteBuffer buffer,
            IEnumerable dictionary,
            FormattingProfile profile,
            TemplateContext? templateContext,
            IFormatter? formatter,
            FormattingOptions options,
            int depth)
        {
            var reader = DictionaryReaders.GetOrAdd(
                dictionary.GetType(),
                ExpressionFactories.CreateDictionaryReader(dictionary.GetType()));

            var entries = reader(dictionary);

            foreach (var (key, value) in entries)
            {
                buffer.WriteFormattedValue(
                    new DestructuredKey(key),
                    profile,
                    templateContext,
                    formatter,
                    options);
                
                
            }
        }

        private static bool IsDictionary(Type type) => 
            type.IsGenericType 
            && typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition());
    }
}