using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Rendering.Internal
{
    internal class TemplateDescriptor
    {
        private readonly Type _type;
        private readonly string _template;
        private readonly Func<Match, ITemplateRenderer> _factory;

        internal TemplateDescriptor(Type type)
        {
            _type = type;
            _template = GetTemplate(type);
            _factory = CreateFactoryExpression(type);
        }

        internal bool TryCreateRenderer(string templateContext, out ITemplateRenderer? renderer)
        {
            var match = Regex.Match(templateContext, _template);
            renderer = match.Success ? _factory(match) : null;
            return match.Success;
        }

        internal bool Select(string templateContext)
        {
            return Regex.IsMatch(templateContext, _template);
        }

        internal ITemplateRenderer Create(Match templateContext)
        {
            return _factory(templateContext);
        }
        
        private static Func<Match, ITemplateRenderer> CreateFactoryExpression(Type type)
        {
            var constructors = type.GetConstructors();

            var constructorWithContextParam = constructors.FirstOrDefault(ctor =>
                ctor.GetParameters().Count(p => p.ParameterType == typeof(Match)) == 1);

            if (constructorWithContextParam != null)
            {
                var templateParameter = Expression.Parameter(typeof(Match));
                var ctorExpression = Expression.New(constructorWithContextParam, templateParameter);
                var lambda = Expression.Lambda<Func<Match, ITemplateRenderer>>(ctorExpression,
                    templateParameter);
                return lambda.Compile();
            }

            var defaultConstructor = constructors.FirstOrDefault(ctor => ctor.GetParameters().Length == 0);

            if (defaultConstructor != null)
            {
                var factory = Expression
                    .Lambda<Func<ITemplateRenderer>>(Expression.New(defaultConstructor))
                    .Compile();

                return _ => factory();
            }

            throw new InvalidOperationException(
                $"Cannot create renderer type {type} because it does not have a compatible constructor.");
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
        public override string ToString() => _type.ToString();
    }
}