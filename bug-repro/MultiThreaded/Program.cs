using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;

const int threadCount = 25;
const int logsPerThread = 150;
const int delayMs = 250;

var loggerFactory = LoggerFactory.Create(builder => builder.AddSpectreConsole());
var logger = loggerFactory.CreateLogger("Test");
var threads = Enumerable.Range(0, threadCount).Select(i =>
    Task.Run(async () =>
    {
        await Task.Delay(delayMs);

        for (var c = 0; c < logsPerThread; c++)
        {
            if (c % 5 == 0)
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

Console.Write("Press <enter> to exit");
Console.ReadLine();

static void Throw()
{
    throw new InvalidOperationException();
}