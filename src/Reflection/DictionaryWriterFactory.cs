using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vertical.SpectreLogger.Destructuring;

namespace Vertical.SpectreLogger.Reflection
{
    internal class DictionaryWriterFactory : CompiledWriterFactory
    {
        internal static bool TryCreate(
            Type type, 
            out CompiledWriter? writer)
        {
            writer = null;

            if (!type.IsGenericType)
                return false;

            var interfaceType = typeof(IDictionary<,>);
            var interfaces = type.GetInterfaces().Where(t => t.IsGenericType);

            if (!interfaces.Any(t => interfaceType.IsAssignableFrom(t.GetGenericTypeDefinition())))
                return false;

            var genericTypes = type.GetGenericArguments();
            var keyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(genericTypes[0], genericTypes[1]);
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(keyValuePairType);

            // Parameters
            var writerParam = Expression.Parameter(typeof(IDestructuringWriter), "writer");
            var valueParam = Expression.Parameter(typeof(object), "value");
            
            // Variables
            var source = Expression.Variable(type, "source");
            var enumerator = Expression.Variable(enumeratorType, "enumerator");
            var hasNext = Expression.Variable(typeof(bool), "next");
            var loop = Expression.Variable(typeof(bool), "loop");
            var current = Expression.Variable(keyValuePairType, "current");
            
            // Expressions
            var getKey = genericTypes[0] == typeof(string)
                ? (Expression)Expression.Property(current, "Key")
                : Expression.Call(Expression.Property(current, "Key"), typeof(object).GetMethod("ToString")!);
            var getValue = Expression.Convert(Expression.Property(current, "Value"), typeof(object));

            // Methods
            var breakTarget = Expression.Label("exit");
            var block = Expression.Block(new[]
                {
                    source,
                    enumerator,
                    hasNext,
                    loop,
                    current
                },
                Expression.Call(writerParam, WriteStartObjectMethod),
                Expression.Assign(source, Expression.Convert(valueParam, type)),
                Expression.Assign(loop, Expression.Constant(true)),
                Expression.Assign(enumerator, Expression.Convert(Expression.Call(source, "GetEnumerator", 
                        EmptyTypes), enumeratorType)),
                Expression.Assign(hasNext, Expression.Call(enumerator, typeof(IEnumerator).GetMethod("MoveNext")!)),
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.And(hasNext, loop),
                        Expression.Block(
                            Array.Empty<ParameterExpression>(), 
                            Expression.Assign(current,
                                Expression.Property(enumerator,
                                    enumeratorType.GetProperty("Current")!)), 
                            Expression.Assign(loop, Expression.Call(writerParam,
                                WritePropertyMethod,
                                getKey,
                                getValue)),
                            Expression.Assign(hasNext,
                                Expression.Call(enumerator, typeof(IEnumerator).GetMethod("MoveNext")!))),
                        Expression.Break(breakTarget))), 
                Expression.Label(breakTarget),
                Expression.Call(writerParam, WriteEndObjectMethod));

            var lambda = Expression.Lambda<CompiledWriter>(
                block,
                writerParam,
                valueParam);

            writer = lambda.Compile();
            
            return true;
        }
    }
}