using Spectre.Console;
using Vertical.SpectreLogger.Output;

namespace CustomConsoleWriter;

public class NewLineReplacingWriter : ConsoleWriter, IConsoleWriter
{
    public NewLineReplacingWriter(IAnsiConsole console) : base(console)
    {
    }

    /// <inheritdoc />
    public void ResetLine() => ResetLineCore();

    /// <inheritdoc />
    public void Write(string content)
    {
        var replaced = content.Replace(
            Environment.NewLine,
            "\u2028");
        
        WriteToConsole(replaced);
    }
}