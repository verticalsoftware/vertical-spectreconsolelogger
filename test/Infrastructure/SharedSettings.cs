using System.Text.RegularExpressions;
using VerifyTests;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public static class SharedSettings
    {
        public static readonly VerifySettings Verifier = Factory.New(() =>
        {
            var settings = new VerifySettings();
            
            settings.UniqueForRuntime();
            settings.UseDirectory("Verified");
            settings.ScrubLinesWithReplace(src => Regex.Replace(
                src,
                @"b__\d+_\d+\(",
                "b__ANY("));
            
            return settings;
        });
    }
} 