using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DerAlbert.Extensions.Xunit.Logging;

internal class TestOutputHelperLogger : ILogger
{
    public TestOutputHelperLogger(ITestOutputHelper testOutputHelper, string categoryName)
    {
        _testOutputHelper = testOutputHelper;
        _categoryName = categoryName;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var startFormat = formatter(state, exception);
        _testOutputHelper.WriteLine($"[{logLevel}] {_categoryName} ({eventId}) {startFormat}");
        if (exception != null)
        {
            _testOutputHelper.WriteLine(exception.ToString());
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state)
    {
        if (state == null) throw new ArgumentNullException(nameof(state));
        return new StateScope(state);
    }

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly string _categoryName;

    private class StateScope : IDisposable
    {
        public object State { get; }

        public StateScope(object state)
        {
            State = state;
        }

        public void Dispose()
        {
        }
    }
}
