using DerAlbert.Extensions.Fakes;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace DerAlbert.Extensions.Xunit.Tests.Logging;

public class LoggerTests : WithFakes
{
    public LoggerTests()
    {
        _outputHelper = An<ITestOutputHelper>();
        Services.AddXunitLogging(_outputHelper);
    }

    [Fact]
    public void Should_log_LogMessage()
    {
        var logger = The<ILogger<LoggerTests>>();

        logger.Log(LogLevel.Information, "OneLog");

        _outputHelper.Received().WriteLine(Arg.Is<string>(s => s.Contains("OneLog")));
    }

    [Fact]
    public void Should_log_LogLevelInfo()
    {
        var logger = The<ILogger<LoggerTests>>();

        logger.Log(LogLevel.Information, "OneLog");

        _outputHelper.Received().WriteLine(Arg.Is<string>(s => s.Contains("[Information]")));
    }

        
    [Fact]
    public void Should_log_LogLevelError()
    {
        var logger = The<ILogger<LoggerTests>>();

        logger.Log(LogLevel.Error, "OneLog");

        _outputHelper.Received().WriteLine(Arg.Is<string>(s => s.Contains("[Error]")));
    }

    [Fact]
    public void Should_log_EventId()
    {
        var logger = The<ILogger<LoggerTests>>();

        logger.Log(LogLevel.Error, new EventId(34,"TheEventId34"), "OneLog");

        _outputHelper.Received().WriteLine(Arg.Is<string>(s => s.Contains("(TheEventId34)")));
    }

    [Fact]
    public void Should_log_Exception()
    {
        var logger = The<ILogger<LoggerTests>>();
        try
        {
            throw new InvalidOperationException("DingMessage");
        }
        catch (InvalidOperationException e)
        {
            logger.Log(LogLevel.Error, e, "Hello {Message}", "The OneLog" );

        }

        _outputHelper.Received().WriteLine(Arg.Is<string>(s => s.Contains("InvalidOperationException")));
    }


    private readonly ITestOutputHelper _outputHelper;
}