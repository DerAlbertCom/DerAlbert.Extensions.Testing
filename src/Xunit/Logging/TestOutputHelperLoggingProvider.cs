using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DerAlbert.Extensions.Xunit.Logging;

internal class TestOutputHelperLoggingProvider : ILoggerProvider
{
    public TestOutputHelperLoggingProvider(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private readonly ITestOutputHelper _testOutputHelper;

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestOutputHelperLogger(_testOutputHelper, categoryName);
    }
}
