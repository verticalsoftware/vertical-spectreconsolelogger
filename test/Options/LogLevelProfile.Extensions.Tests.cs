using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Options;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Options
{
    public class LogLevelProfileExtensionsTests
    {
        private readonly LogLevelProfile _profile = new LogLevelProfile(LogLevel.Information);
        
        [TypeFormatter(typeof(Guid))]
        public class MyFormatter : ICustomFormatter
        {
            /// <inheritdoc />
            public string Format(string? format, object? arg, IFormatProvider? formatProvider) => "";
        }

        public class MyOptions {}

        [Fact]
        public void AddTypeFormattersForAssemblyCreatesInstances()
        {
            _profile.AddTypeFormatters(typeof(LogLevelProfileExtensionsTests).Assembly);
            _profile.TypeFormatters.ContainsKey(typeof(Guid)).ShouldBeTrue();
        }

        [Fact]
        public void AddTypeFormatterRegistersInstance()
        {
            _profile.AddTypeFormatter(typeof(Guid), new MyFormatter());
            _profile.TypeFormatters.ContainsKey(typeof(Guid)).ShouldBeTrue();
        }

        [Fact]
        public void AddTypeFormatterWithTypesRegistersInstance()
        {
            _profile.AddTypeFormatter(new[] {typeof(Guid)}, new MyFormatter());
            _profile.TypeFormatters.ContainsKey(typeof(Guid)).ShouldBeTrue();
        }

        [Fact]
        public void AddTypeFormatterDelegateForTypesRegistersInstance()
        {
            _profile.AddTypeFormatter(new[] {typeof(Guid)}, (_, _, _) => "");
            _profile.TypeFormatters.ContainsKey(typeof(Guid)).ShouldBeTrue();
        }

        [Fact]
        public void AddTypeFormatterWithGenericArgRegistersInstance()
        {
            _profile.AddTypeFormatter<Guid>(Substitute.For<ICustomFormatter>());
            _profile.TypeFormatters.ContainsKey(typeof(Guid)).ShouldBeTrue();
        }

        [Fact]
        public void AddTypeFormatterWithGenericArgRegistersDelegate3()
        {
            _profile.AddTypeFormatter<Guid>((_, _, _) => "");
            _profile.TypeFormatters.ContainsKey(typeof(Guid)).ShouldBeTrue();
        }
        
        [Fact]
        public void AddTypeFormatterWithGenericArgRegistersDelegate2()
        {
            _profile.AddTypeFormatter<Guid>((_, _) => "");
            _profile.TypeFormatters.ContainsKey(typeof(Guid)).ShouldBeTrue();
        }

        [Fact]
        public void AddValueStyleRegistersMarkup()
        {
            _profile.AddValueStyle(true, "[red]");
            _profile.ValueStyles.ContainsKey(true).ShouldBeTrue();
        }

        [Fact]
        public void AddTypeStyleRegistersMarkup()
        {
            _profile.AddTypeStyle(typeof(bool), "[red]");
            _profile.TypeStyles.ContainsKey(typeof(bool)).ShouldBeTrue();
        }

        [Fact]
        public void AddTypeStylesRegistersMarkup()
        {
            _profile.AddTypeStyle(new[] {typeof(bool)}, "[red]");
            _profile.TypeStyles.ContainsKey(typeof(bool)).ShouldBeTrue();
        }

        [Fact]
        public void AddTypeStyleWithGenericTypeRegistersMarkup()
        {
            _profile.AddTypeStyle<bool>("[red]");
            _profile.TypeStyles.ContainsKey(typeof(bool)).ShouldBeTrue();
        }

        [Fact]
        public void ConfigureOptionsCreatesNewInstance()
        {
            _profile.ConfigureOptions<MyOptions>(_ => { });
            _profile.ConfiguredOptions.GetOptions<MyOptions>().ShouldNotBeNull();
        }

        [Fact]
        public void ConfigureOptionsReusesCreatedInstance()
        {
            _profile.ConfigureOptions<MyOptions>(_ => { });
            var instance = _profile.ConfiguredOptions.GetOptions<MyOptions>().ShouldNotBeNull();
            _profile.ConfigureOptions<MyOptions>(opt => opt.ShouldBe(instance));
        }

        [Fact]
        public void ClearTypeFormattersEmptiesDictionary()
        {
            _profile.AddTypeFormatter<bool>((_, _) => "");
            _profile.ClearTypeFormatters();
            _profile.TypeFormatters.ShouldBeEmpty();
        }

        [Fact]
        public void ClearTypeStylesEmptiesDictionary()
        {
            _profile.AddTypeStyle<bool>("[red]");
            _profile.ClearTypeStyles();
            _profile.TypeStyles.ShouldBeEmpty();
        }

        [Fact]
        public void ClearValueStylesEmptiesDictionary()
        {
            _profile.AddValueStyle(true, "[red]");
            _profile.ClearValueStyles();
            _profile.ValueStyles.ShouldBeEmpty();
        }
    }
}