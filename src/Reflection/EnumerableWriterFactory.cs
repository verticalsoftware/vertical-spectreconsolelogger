using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vertical.SpectreLogger.Destructuring;

namespace Vertical.SpectreLogger.Reflection
{
    internal class EnumerableWriterFactory : CompiledWriterFactory
    {
        internal static bool TryCreate(Type type, out CompiledWriter? writer)
        {
            writer = null;

            if (type == typeof(string))
                return false;
            
            var enumerableGenericType = typeof(IEnumerable<>);
            var interfaces = type.GetInterfaces();
            var enumerableType = interfaces
                .FirstOrDefault(t => t.IsGenericType 
                                     && enumerableGenericType.IsAssignableFrom(t.GetGenericTypeDefinition()));

            if (enumerableType == null)
                return false;

            var elementType = enumerableType.GetGenericArguments()[0];
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(elementType);
            
            // parameters
            var sourceParam = Expression.Parameter(typeof(object));
            var writerParam = Expression.Parameter(typeof(IDestructuringWriter));
            
            // variables
            var typeParam = Expression.Variable(type);
            var enumerator = Expression.Variable(enumeratorType);
            var current = Expression.Variable(elementType);
            var hasNext = Expression.Variable(typeof(bool));
            var loop = Expression.Variable(typeof(bool));
            var label = Expression.Label("_quit");
            
            // Methods
            var moveNext = Expression.Call(enumerator, typeof(IEnumerator).GetMethod("MoveNext")!);
            var getCurrent = (Expression) Expression.Property(enumerator, enumeratorType.GetProperty("Current")!);
            
            // body
            var body = Expression.Block(
                new[] {typeParam, enumerator, current, hasNext, loop},
                Expression.Assign(typeParam, Expression.Convert(sourceParam, type)),
                Expression.Assign(loop, Expression.Constant(true)),
                Expression.Assign(
                    enumerator,
                    Expression.Call(typeParam, enumerableType.GetMethod("GetEnumerator")!)),
                Expression.Assign(
                    hasNext,
                    moveNext),
                Expression.Call(writerParam, WriteStartArrayMethod),
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.And(hasNext, loop),
                        Expression.Block(
                            Array.Empty<ParameterExpression>(),
                            Expression.Assign(current, getCurrent),
                            Expression.Assign(
                                loop,
                                Expression.Call(
                                    writerParam, 
                                    WriteElementMethod, 
                                    Expression.Convert(current, typeof(object)))),
                            Expression.Assign(hasNext, moveNext)),
                        Expression.Break(label))), Expression.Label(label),
                Expression.Call(writerParam, WriteEndArrayMethod));

            var lambda = Expression.Lambda<CompiledWriter>(body, writerParam, sourceParam);

            writer = lambda.Compile();
            
            return true;
        }   
    }
}