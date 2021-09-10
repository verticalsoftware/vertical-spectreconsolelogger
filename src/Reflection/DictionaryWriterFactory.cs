using System;
using System.Collections;
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

            if (!typeof(IDictionary).IsAssignableFrom(type))
                return false;

            var enumeratorType = typeof(IEnumerator);
            
            // Parameters
            var writerParam = Expression.Parameter(typeof(IDestructuringWriter));
            var valueParam = Expression.Parameter(typeof(object));
            
            // Variables
            var source = Expression.Variable(type);
            var enumerator = Expression.Variable(enumeratorType);
            var hasNext = Expression.Variable(typeof(bool));
            var loop = Expression.Variable(typeof(bool));
            var current = Expression.Variable(typeof(object));
            
            // Expressions
            var getKey = Expression.Property(current, "Key");
            var getValue = Expression.Property(current, "Value");
            
            // Methods
            var breakTarget = Expression.Label("_exit");
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
                            Expression.Assign(hasNext, Expression.Call(enumerator, "MoveNext", EmptyTypes))),
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