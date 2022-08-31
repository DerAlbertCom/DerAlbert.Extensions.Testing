using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public interface IScoped : IDisposable
{
    bool Disposed { get; }
}

internal class Scoped : IScoped
{
    public Scoped()
    {
        Disposed = false;
    }
    public void Dispose()
    {
        Disposed = true;
    }

    public bool Disposed { get; private set; }
}

public class WithFakesScopedServicesTests : WithFakes
{
    public WithFakesScopedServicesTests()
    {
        Services.AddScoped<IScoped, Scoped>();
    }

    [Fact]
    public void Two_scopes_results_in_two_different_instances()
    {
        var first = FromServiceScope((IServiceProvider scoped) => { return scoped.GetRequiredService<IScoped>(); });

        var second = FromServiceScope((IServiceProvider scoped) => { return scoped.GetRequiredService<IScoped>(); });

        first.Should().NotBeSameAs(second);
    }

    [Fact]
    private void One_scope_results_in_the_same_instance()
    {
        var (first, second) = FromServiceScope((IServiceProvider scoped) =>
        {
            return (scoped.GetRequiredService<IScoped>(), scoped.GetRequiredService<IScoped>());
        });

        first.Should().BeSameAs(second);
    }

    [Fact]
    public void Two_scopes_results_in_two_different_instances_async()
    {
        var first = FromServiceScopeAsync(async (IServiceProvider scoped) =>
        {
            await Task.Delay(0);
            return scoped.GetRequiredService<IScoped>();
        });

        var second = FromServiceScope((IServiceProvider scoped) => { return scoped.GetRequiredService<IScoped>(); });

        first.Should().NotBeSameAs(second);
    }

    [Fact]
    public async Task One_scope_results_in_the_same_instance_async()
    {
        var (first, second) = await FromServiceScopeAsync(async (IServiceProvider scoped) =>
        {
            await Task.Delay(0);
            return (scoped.GetRequiredService<IScoped>(), scoped.GetRequiredService<IScoped>());
        });

        first.Should().BeSameAs(second);
    }

    [Fact]
    public async Task InServiceScope_async()
    {
        await InServiceScopeAsync(async (IServiceProvider scoped) =>
        {
            await Task.Delay(0);

            var first = scoped.GetRequiredService<IScoped>();
            var second = scoped.GetRequiredService<IScoped>();
            first.Should().BeSameAs(second);
        });
    }
}