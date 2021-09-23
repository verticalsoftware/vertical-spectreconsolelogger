using VerifyTests;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public static class SharedSettings
    {
        public static readonly VerifySettings Verifier = Factory.New<VerifySettings>(() =>
        {
            var settings = new VerifySettings();
            
            settings.UniqueForRuntime();
            settings.UseDirectory("Verified");
            
            return settings;
        });
    }
} 