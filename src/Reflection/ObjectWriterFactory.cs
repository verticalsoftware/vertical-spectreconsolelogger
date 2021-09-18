using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vertical.SpectreLogger.Destructuring;

namespace Vertical.SpectreLogger.Reflection
{
    internal class ObjectWriterFactory : CompiledWriterFactory
    {
        internal static bool TryCreate(Type type, out CompiledWriter? writer)
        {
            writer = null;
            
            // No structs in System
            if (type.Namespace == nameof(System) && type.IsValueType)
                return false;

            if (type == typeof(string))
                return false;

            var properties = type
                .GetProperties()
                .Where(prop => !prop.Name.StartsWith("get_") && prop.GetIndexParameters().Length == 0)
                .ToArray();

            if (properties.Length == 0)
                return false;
            
            // Parameters
            var sourceParam = Expression.Parameter(typeof(object));
            var writerParam = Expression.Parameter(typeof(IDestructuringWriter));
            
            // Variables
            var typeParam = Expression.Variable(type);
            
            // Method body
            var body = new List<Expression>(properties.Length + 5)
            {
                Expression.Assign(typeParam, Expression.Convert(sourceParam, type)),
                Expression.Call(writerParam, WriteStartObjectMethod)
            };

            foreach (var property in properties)
            {
                body.Add(Expression.Call(
                    writerParam,
                    WritePropertyMethod,
                    Expression.Constant(property.Name),
                    Expression.Convert(Expression.Property(typeParam, property), typeof(object))));
            }
            
            body.Add(Expression.Call(writerParam, WriteEndObjectMethod));

            var lambda = Expression.Lambda<CompiledWriter>(Expression.Block(
                new []{typeParam}, body), writerParam, sourceParam);

            writer = lambda.Compile();
            
            return true;
        }
    }
}