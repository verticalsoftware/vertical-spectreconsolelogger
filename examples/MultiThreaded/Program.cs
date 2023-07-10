using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;

var loggerFactory = LoggerFactory.Create(builder => builder.AddSpectreConsole());
var logger = loggerFactory.CreateLogger("Test");
var threads = Enumerable.Range(0, 25).Select(i =>
    Task.Run(() =>
    {
        for (var c = 0; c < 100; c++)
        {
            if (c % 25 == 0)
            {
                try
                {
                    Throw();
                }
                catch (InvalidOperationException exception)
                {
                    logger.LogError(exception, "An InvalidOperationException was thrown");
                }
                continue;
            }

            logger.LogInformation("I'm thread {thread}, and I am on iteration {iteration}",
                i,
                c);
        }
        return Task.CompletedTask;
    }));

await Task.WhenAll(threads);

static void Throw()
{
    throw new InvalidOperationException();
}