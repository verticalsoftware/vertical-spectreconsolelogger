using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Vertical.SpectreLogger.Types;

namespace Vertical.SpectreLogger.Infrastructure
{
    internal static class ExpressionFactories
    {
        public static Func<object, KeyValuePair<object, object>> CreateKeyValuePairFactory(Type dictionaryType)
        {
            var genericParameters = dictionaryType.GetGenericArguments();
            var keyType = genericParameters[0];
            var valueType = genericParameters[1];
            var keyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType);
            var keyValuePairParameterExpression = Expression.Parameter(typeof(object));
            var castedKeyValuePairExpression = Expression.Convert(keyValuePairParameterExpression, keyValuePairType);
            var newKeyValuePairExpression = Expression.New(
                typeof(KeyValuePair<object,object>).GetConstructor(new[]{typeof(object), typeof(object)})!,
                Expression.Convert(Expression.Property(castedKeyValuePairExpression, "Key"), typeof(object)),
                Expression.Convert(Expression.Property(castedKeyValuePairExpression, "Value"), typeof(object)));
            var lambda = Expression.Lambda<Func<object, KeyValuePair<object, object>>>(newKeyValuePairExpression, keyValuePairParameterExpression);

            return lambda.Compile();
        }

        public static Func<object, object> CreateDestructedObjectFactory(Type type)
        {
            var parameter = Expression.Parameter(typeof(object));
            var destructuredType = typeof(DestructuredValue<>).MakeGenericType(type);
            var newInstance = Expression.New(
                destructuredType.GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance, 
                    null, 
                    CallingConventions.Any, 
                    new[]{type}, 
                    null)!,
                Expression.Convert(parameter, type));

            return Expression
                .Lambda<Func<object, object>>(newInstance, parameter)
                .Compile();
        }

        public static Func<IEnumerable, List<KeyValuePair<object, object>>> CreateDictionaryReader(Type type)
        {
            // Types & method info
            var genericTypeArgs = type.GetGenericArguments();
            var sourceKeyType = genericTypeArgs[0];
            var sourceValueType = genericTypeArgs[1];
            var sourceKeyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(sourceKeyType, sourceValueType);
            var sourceEnumeratorType = typeof(IEnumerator<>).MakeGenericType(sourceKeyValuePairType);
            var outputKeyValuePairType = typeof(KeyValuePair<object, object>);
            var outputKeyValuePairConstructor = outputKeyValuePairType.GetConstructor(new[] {typeof(object), typeof(object)})!;
            var outputListType = typeof(List<KeyValuePair<object, object>>);
            var outputListConstructor = outputListType.GetConstructor(new[] {typeof(int)})!;
            var outputListAddMethod = outputListType.GetMethod("Add", new[]{outputKeyValuePairType})!;
            
            // Input
            var inputParameter = Expression.Parameter(typeof(IEnumerable));
            
            // Variables
            var listVariable = Expression.Variable(outputListType, "returnList");
            var dictionaryVariable = Expression.Variable(type, "sourceDictionary");
            var enumeratorVariable = Expression.Variable(sourceEnumeratorType, "enumerator");
            var hasNextVariable = Expression.Variable(typeof(bool), "hasNext");
            var currentVariable = Expression.Variable(sourceKeyValuePairType, "current");
            var variables = new[]
            {
                listVariable,
                dictionaryVariable,
                enumeratorVariable,
                hasNextVariable,
                currentVariable
            };
            
            // Convert parameter
            var castDictionary = Expression.Assign(
                dictionaryVariable,
                Expression.Convert(inputParameter, type));
            
            // Create the list
            var createList = Expression.Assign(listVariable,
                Expression.New(
                    outputListConstructor,
                    Expression.Property(dictionaryVariable, "Count")));

            // Get the enumerator
            var getEnumerator = Expression.Assign(
                enumeratorVariable,
                Expression.Convert(
                    Expression.Call(dictionaryVariable, type.GetMethod("GetEnumerator")!),
                    sourceEnumeratorType));
            
            // Move next
            var moveNext = Expression.Assign(
                hasNextVariable,
                Expression.Call(enumeratorVariable, typeof(IEnumerator).GetMethod("MoveNext")!));
            
            // Assign current
            var assignCurrent = Expression.Assign(
                currentVariable,
                Expression.Property(enumeratorVariable, "Current"));
            
            // Create output key/value pair
            var newKeyValuePair = Expression.New(
                outputKeyValuePairConstructor,
                Expression.Convert(Expression.Property(currentVariable, "Key"), typeof(object)),
                Expression.Convert(Expression.Property(currentVariable, "Value"), typeof(object)));
            
            // Add to list
            var addKeyValuePair = Expression.Call(
                listVariable,
                outputListAddMethod,
                newKeyValuePair);
            
            // Loop body
            var breakTarget = Expression.Label(outputListType);
            var loop = Expression.Loop(
                Expression.IfThenElse(
                    hasNextVariable,
                    Expression.Block(
                        Array.Empty<ParameterExpression>(),
                        assignCurrent,
                        addKeyValuePair,
                        moveNext),
                        Expression.Break(breakTarget, listVariable)));
            
            // Main body
            var mainBody = Expression.Block(
                variables,
                castDictionary,
                createList,
                getEnumerator,
                moveNext,
                loop,
                Expression.Label(breakTarget, listVariable));

            var lambda = Expression.Lambda<Func<IEnumerable, List<KeyValuePair<object, object>>>>(
                mainBody,
                inputParameter);

            return lambda.Compile();
        }

        public static Func<object, List<KeyValuePair<string, object>>> CreatePropertyReader(Type type)
        {
            var properties = type
                .GetProperties()
                .Where(prop => prop.GetIndexParameters().Length == 0)
                .ToArray();
            
            // Input
            var parameterExpression = Expression.Parameter(typeof(object), "propertyList");
            var typeParameterExpression = Expression.Convert(parameterExpression, type);
            
            // Variables
            var listVariableExpression = Expression.Variable(typeof(List<KeyValuePair<string, object>>));
            
            // Body
            var newListExpression = Expression.New(
                typeof(List<KeyValuePair<string, object>>).GetConstructor(new[]{typeof(int)})!, 
                Expression.Constant(properties.Length));
            var assignListExpression = Expression.Assign(listVariableExpression, newListExpression);
            var keyValuePairConstructor = typeof(KeyValuePair<string, object>).GetConstructor(
                new[] {typeof(string), typeof(object)})!;
            var addListElementMethod = typeof(List<KeyValuePair<string, object>>)
                .GetMethod("Add", new[]{ typeof(KeyValuePair<string, object>) })!;
            var addEntryExpressions = properties
                .OrderBy(prop => prop.Name)
                .Select(prop =>
                {
                    var keyExpression = Expression.Constant(prop.Name);
                    var valueExpression = Expression.Convert(Expression.Property(typeParameterExpression, prop), typeof(object));
                    var keyValuePairExpression = Expression.New(keyValuePairConstructor, keyExpression, valueExpression);
                    return Expression.Call(listVariableExpression, addListElementMethod, keyValuePairExpression);
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

            return Expression
                .Lambda<Func<object, List<KeyValuePair<string, object>>>>(blockExpression, parameterExpression)
                .Compile();
        }
    }
}