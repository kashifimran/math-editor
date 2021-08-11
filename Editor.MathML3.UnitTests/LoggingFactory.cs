using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace Editor.MathML3.UnitTests
{
    public static class LoggingFactory
    {
        private const string OutputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}]{NewLine}" +
            "in method {MemberName} at {FilePath}:{LineNumber}{NewLine}" +
            "{Message:lj}{NewLine}" +
            "{Exception}{NewLine}";

        public static ILogger CreateLogger(ITestOutputHelper output)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output, LogEventLevel.Verbose, outputTemplate: OutputTemplate)
                .CreateLogger();
        }
    }
}
