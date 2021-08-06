﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.PseudoTypes;
using Vertical.SpectreLogger.Rendering;

namespace SpectreLoggerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.Clear();
            
            Console.WriteLine();
            Console.WriteLine();
            
            DoExample();
            
            Console.WriteLine();
            Console.WriteLine();
            
            return;

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSpectreConsole(options =>
                {
                    options.MinimumLevel = LogLevel.Trace;
                    options.ConfigureProfiles(profile => profile.AddTypeFormatter<NullValue>(_ => "(null)"));
                });
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            var logger = loggerFactory.CreateLogger("System.Runtime.CompilerServices.Native");

            var colors = typeof(Color)
                .GetProperties()
                .Where(prop => prop.PropertyType == typeof(Color))
                .OrderBy(p => p.Name)
                .Select(prop => (name: prop.Name, color: (Color) prop.GetValue(null)!));

            foreach (var (name, color) in colors)
            {
                AnsiConsole.MarkupLine($"[{color.ToMarkup()}]{name} = #{color.ToHex()} (R={color.R},G={color.G},B={color.B})[/]");
            }

            Exception exception = default;

            try
            {
                try
                {
                    var _ = new Dictionary<string, string>()["nope"];
                }
                catch (KeyNotFoundException ex)
                {
                    throw new InvalidOperationException("Dictionary access failed", ex);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var logLevels = new[] {LogLevel.Trace, LogLevel.Debug, LogLevel.Information, LogLevel.Warning, LogLevel.Error, LogLevel.Critical};

            foreach (var logLevel in logLevels)
            {
                logger.Log(logLevel,
                    exception,
                    // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                    "This is a formatted message for the {level} log level. Note how the following values are rendered:" + Environment.NewLine 
                    + "Numbers:   {x}, {y} {z}" + Environment.NewLine
                    + "Boolean:   {bool_true}, {bool_false}" + Environment.NewLine 
                    + "Date/time: {date}" + Environment.NewLine
                    + "Strings:   {string}" + Environment.NewLine
                    + "Null:      {value}",
                    logLevel.ToString(),
                    10, 20, 30f,
                    true, false,
                    DateTimeOffset.UtcNow,
                    "test-string",
                    null);
            }
        }
        
        private static void DoExample()
        {
            var logger = LoggerFactory.Create(builder => builder.AddSpectreConsole(options =>
                {
                    options.ConfigureProfiles(profile =>
                    {
                        profile.OutputTemplate = "{Message}{Exception:NewLine?}";
                        profile.ConfigureRenderer<ExceptionRenderer.Options>(exOptions =>
                        {
                            exOptions.ExceptionMessageStyle = Color.Grey93.ToMarkup();
                            exOptions.ExceptionNameFormatter = type => type.FullName!;
                            exOptions.ExceptionNameStyle = Color.LightGoldenrod1.ToMarkup();
                            exOptions.SourceLineNumberStyle = Color.Magenta1.ToMarkup(); 
                            exOptions.MaxStackFrames = 5;
                            exOptions.MethodNameStyle = Color.DarkOrange3_1.ToMarkup();
                            exOptions.ParameterNameStyle = Color.LightSeaGreen.ToMarkup();
                            exOptions.ParameterTypeStyle = Color.DodgerBlue1.ToMarkup();
                            exOptions.RenderParameterNames = true;
                            exOptions.RenderSourceLineNumbers = true;
                            exOptions.RenderParameterTypes = true;
                            exOptions.RenderSourcePaths = true;
                            exOptions.SourcePathFormatter = Path.GetFileName;
                            exOptions.SourcePathStyle = Color.Grey50.ToMarkup();
                            exOptions.StackFrameStyle = Color.Grey50.ToMarkup();
                            exOptions.UnwindAggregateExceptions = true;
                            exOptions.UnwindInnerExceptions = true;
                        });
                    });
                }))
                .CreateLogger("Vertical.SpectreLogger.Console.Example");

            try
            {
                new Dictionary<string, string> {["key"] = string.Empty}.Add("key", string.Empty);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An exception was caught in the example");
            }
        }

        private static void DoExample7()
        {
            var logger = LoggerFactory.Create(builder => builder.AddSpectreConsole(options =>
                {
                    options.ConfigureProfile(LogLevel.Information, profile =>
                        profile
                            .ConfigureRenderer<EventIdRenderer.Options>(opt => opt.Style = "green")
                            .OutputTemplate = "[{EventId:Name}]: {Message}");
                }))
                .CreateLogger("Example");

            logger.LogInformation(new EventId(10, "Example"), "Showing the event name only");
        }
        
        private static void DoExample6()
        {
            var logger = LoggerFactory.Create(builder => builder.AddSpectreConsole(options =>
                {
                    options.ConfigureProfile(LogLevel.Information, profile =>
                        profile
                            .ConfigureRenderer<CategoryNameRenderer.Options>(opt => opt.Style = "black on yellow")
                            .OutputTemplate = "[{CategoryName,-25:S2}]: {Message}");
                }))
                .CreateLogger("Vertical.SpectreConsole.Example.CategoryName");

            logger.LogInformation("Look to left for the category");
        }

        private static void DoExample5()
        {
            var logger = LoggerFactory.Create(builder => builder.AddSpectreConsole(options =>
                {
                    options.ConfigureProfiles(profile => profile
                        .AddTypeFormatter<int>(i => i < 1000 
                            ? "less than a second" 
                            : i < 60000 
                                ? "less than a minute" 
                                : "over a minute"));
                }))
                .CreateLogger("Program");

            logger.LogInformation("It took {fast} to do this, but {slow} to do that", 
                500,
                60000);
        }
        
        private static void DoExample4()
        {
            var logger = LoggerFactory.Create(builder => builder.AddSpectreConsole(options =>
                {
                    options.ConfigureProfiles(profile => profile
                        .ClearValueStyles()
                        .AddValueStyle(true, Color.Green.ToMarkup())
                        .AddValueStyle(false, Color.Orange1.ToMarkup()));
                }))
                .CreateLogger("Program");

            logger.LogInformation("This is {true}, but this is {false}", true, false);
        }

        private static void DoExample3()
        {
            var logger = LoggerFactory.Create(builder => builder.AddSpectreConsole(options =>
                {
                    options.ConfigureProfiles(profile => profile
                        .ClearTypeStyles()
                        .AddTypeStyle<int>(Color.Magenta1.ToMarkup())
                        .AddTypeStyle<bool>(Color.Green.ToMarkup())
                        .DefaultTypeStyle = Color.Orange1.ToMarkup());
                }))
                .CreateLogger("Program");

            logger.LogInformation("Here is a bool: {bool}, here is an int: {int}, and here is a string: {string}",
                true,
                int.MaxValue,
                "Hello Spectre Logger");
        }

        private static void DoExample2()
        {
            var logger = LoggerFactory.Create(builder => builder
                .AddSpectreConsole(options => options
                    .ConfigureProfile(LogLevel.Debug, profile => profile.BaseEventStyle = "grey54")
                    .ConfigureProfile(LogLevel.Information, profile => profile.BaseEventStyle = "grey93")
                    .ConfigureProfile(LogLevel.Warning, profile => profile.BaseEventStyle = "yellow")
                    .ConfigureProfile(LogLevel.Error, profile => profile.BaseEventStyle = "red")
                    .ConfigureProfile(LogLevel.Critical, profile => profile.BaseEventStyle = "white on red")
                    .MinimumLevel = LogLevel.Trace
                )
                .SetMinimumLevel(LogLevel.Trace))
                .CreateLogger("Program");

            logger.LogTrace("I am a {level} event", LogLevel.Trace);
            logger.LogDebug("I am a {level} event", LogLevel.Debug);
            logger.LogInformation("I am a {level} event", LogLevel.Information);
            logger.LogWarning("I am a {level} event", LogLevel.Warning);
            logger.LogError("I am a {level} event", LogLevel.Error);
            logger.LogCritical("I am a {level} event", LogLevel.Critical);
        }
    }
}
