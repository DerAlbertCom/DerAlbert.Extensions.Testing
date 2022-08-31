using DerAlbert.Extensions.Xunit.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DerAlbert.Extensions.Xunit;

public static class XunitExtensions
{
    public static IServiceCollection AddXunitLogging(this IServiceCollection services, ITestOutputHelper testOutputHelper)
    {
        services.AddLogging(builder =>
        {
            builder.AddProvider(new TestOutputHelperLoggingProvider(testOutputHelper));
        });
        return services;
    }
}