using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Vertical.SpectreLogger.Infrastructure
{
    public static class ExpressionFactories
    {
        private static readonly ConstructorInfo KeyValuePairConstructor = typeof(KeyValuePair<object, object>)
            .GetConstructor(new[] {typeof(object), typeof(object)})!;
        
        private static readonly ConstructorInfo PropertyListKeyValuePairConstructor = typeof(KeyValuePair<string, object>)
            .GetConstructor(new[] {typeof(string), typeof(object)})!;

        private static readonly ConstructorInfo PropertyListConstructor = typeof(List<KeyValuePair<string, object>>)
            .GetConstructor(new[] {typeof(int)})!;

        private static readonly MethodInfo AddKeyValuePairMethod = typeof(List<KeyValuePair<string, object>>)
            .GetMethod("Add")!;

        public static Func<object, KeyValuePair<object, object>> CreateKeyValuePairFactory(Type dictionaryType)
        {
            var genericParameters = dictionaryType.GetGenericArguments();
            var keyType = genericParameters[0];
            var valueType = genericParameters[1];
            var keyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType);
            var keyValuePairParameterExpression = Expression.Parameter(typeof(object));
            var castedKeyValuePairExpression = Expression.Convert(keyValuePairParameterExpression, keyValuePairType);
            var getKeyPropertyExpression = Expression.Property(castedKeyValuePairExpression, "Key");
            var getKeyPropertyExpressionAsObject = Expression.Convert(getKeyPropertyExpression, typeof(object));
            var getValuePropertyExpression = Expression.Property(castedKeyValuePairExpression, "Value");
            var getValuePropertyExpressionAsObject = Expression.Convert(getValuePropertyExpression, typeof(object));
            var newKeyValuePairExpression = Expression.New(
                KeyValuePairConstructor,
                getKeyPropertyExpressionAsObject,
                getValuePropertyExpressionAsObject);
            var lambda = Expression.Lambda<Func<object, KeyValuePair<object, object>>>(newKeyValuePairExpression, keyValuePairParameterExpression);

            return lambda.Compile();
        }

        public static Func<object, List<KeyValuePair<string, object>>> CreatePropertyFactory(Type type)
        {
            var properties = type.GetProperties().Where(prop => prop.GetIndexParameters().Length == 0).ToArray();
            var objParameterExpression = Expression.Parameter(typeof(object), "propertyList");
            var typeParameterExpression = Expression.Convert(objParameterExpression, type);
            var listVariableExpression = Expression.Variable(typeof(List<KeyValuePair<string, object>>));
            var newListExpression = Expression.New(PropertyListConstructor, Expression.Constant(properties.Length));
            var assignListExpression = Expression.Assign(listVariableExpression, newListExpression);
            var addEntryExpressions = properties
                .OrderBy(prop => prop.Name)
                .Select(prop =>
                {
                    var keyExpression = Expression.Constant(prop.Name);
                    var valueExpression = Expression.Convert(Expression.Property(typeParameterExpression, prop), typeof(object));
                    var keyValuePairExpression = Expression.New(PropertyListKeyValuePairConstructor, keyExpression, valueExpression);
                    return Expression.Call(listVariableExpression, AddKeyValuePairMethod, keyValuePairExpression);
                })
                .ToList();
            var labelTarget = Expression.Label(typeof(List<KeyValuePair<string, object>>));
            var returnExpression = Expression.Return(labelTarget, listVariableExpression);
            var returnLabel = Expression.Label(labelTarget, listVariableExpression);
            var blockExpressions = new List<Expression>
            {
                listVariableExpression,
                newListExpression,
                assignListExpression
            };
            blockExpressions.AddRange(addEntryExpressions);
            blockExpressions.Add(returnExpression);
            blockExpressions.Add(returnLabel);
            var blockExpression = Expression.Block(new[] {listVariableExpression}, blockExpressions);
            var lambda = Expression.Lambda<Func<object, List<KeyValuePair<string, object>>>>(blockExpression, objParameterExpression);
            return lambda.Compile();
        }
    }
}