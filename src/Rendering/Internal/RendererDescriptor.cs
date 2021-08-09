using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering.Internal
{
    internal class RendererDescriptor
    {
        private readonly Type _type;
        private readonly TemplateParser _parser;
        private readonly Delegate _factory;

        internal RendererDescriptor(Type type)
        {
            _type = type;
            _parser = GetParser(type);
            _factory = CreateFactoryExpression(type);
        }

        internal bool TryCreateRenderer(string template, out ITemplateRenderer? renderer)
        {
            var templateContext = _parser.Parse(template);

            if (!templateContext.MatchContext.Success)
            {
                renderer = null;
                return false;
            }

            renderer = null;

            switch (_factory)
            {
                case Func<Match, ITemplateRenderer> matchFactory:
                    renderer = matchFactory(templateContext.MatchContext);
                    break;
                
                case Func<TemplateContext, ITemplateRenderer> contextFactory:
                    renderer = contextFactory(templateContext);
                    break;
                
                case Func<ITemplateRenderer> defaultFactory:
                    renderer = defaultFactory();
                    break;
            }

            return renderer != null;
        }
        
        private static Delegate CreateFactoryExpression(Type type)
        {
            var constructors = type.GetConstructors();

            var constructorWithMatchParam = constructors.FirstOrDefault(ctor =>
                ctor.GetParameters().Count(p => p.ParameterType == typeof(Match)) == 1);

            if (constructorWithMatchParam != null)
            {
                var templateParameter = Expression.Parameter(typeof(Match));
                var ctorExpression = Expression.New(constructorWithMatchParam, templateParameter);
                var lambda = Expression.Lambda<Func<Match, ITemplateRenderer>>(ctorExpression,
                    templateParameter);
                return lambda.Compile();
            }

            var constructorWithContextParam = constructors.FirstOrDefault(ctor =>
                ctor.GetParameters().Count(p => p.ParameterType == typeof(TemplateContext)) == 1);

            if (constructorWithContextParam != null)
            {
                var templateParameter = Expression.Parameter(typeof(TemplateContext));
                var ctorExpression = Expression.New(constructorWithContextParam, templateParameter);
                var lambda = Expression.Lambda<Func<TemplateContext, ITemplateRenderer>>(ctorExpression,
                    templateParameter);
                return lambda.Compile();
            }

            var defaultConstructor = constructors.FirstOrDefault(ctor => ctor.GetParameters().Length == 0);

            if (defaultConstructor != null)
            {
                var factory = Expression
                    .Lambda<Func<ITemplateRenderer>>(Expression.New(defaultConstructor))
                    .Compile();

                return new Func<ITemplateRenderer>(() => factory());
            }

            throw new InvalidOperationException(
                $"Cannot create renderer type {type} because it does not have a compatible constructor.");
        }

        private static TemplateParser GetParser(Type type)
        {
            var parserAttribute = type.GetCustomAttribute<TemplateParserAttribute>();

            if (parserAttribute == null)
            {
                return new TemplateParser {CustomPattern = GetTemplate(type)};
            }
            
            var property = type
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(prop => prop.PropertyType == typeof(TemplateParser) && prop.Name == parserAttribute.PropertyOrFieldName);
                
            if (property != null)
            {
                return property.GetValue(null) as TemplateParser ?? throw new InvalidOperationException(
                    $"Static member {type}.{parserAttribute.PropertyOrFieldName} returned a null value.");
            }
                
            var field = type
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(prop => prop.FieldType == typeof(TemplateParser) && prop.Name == parserAttribute.PropertyOrFieldName);
                
            if (field != null)
            {
                return field.GetValue(null) as TemplateParser ?? throw new InvalidOperationException(
                    $"Static member {type}.{parserAttribute.PropertyOrFieldName} returned a null value.");
            }

            throw new InvalidOperationException($"Cannot find static property or field '{type}.{parserAttribute.PropertyOrFieldName}'");
        }
        
        private static string GetTemplate(MemberInfo type)
        {
            var templateAttribute = type.GetCustomAttribute<TemplateAttribute>();

            if (templateAttribute != null)
            {
                return templateAttribute.Template;
            }

            throw new InvalidOperationException(
                $"Cannot use renderer type {type} because it does not have a Template attribute");
        }

        /// <inheritdoc />
        public override string ToString() => $"{_type}=\"{_parser.Pattern}\"";
    }
}