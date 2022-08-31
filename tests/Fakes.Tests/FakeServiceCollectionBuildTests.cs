using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class FakeServiceCollectionBuildTests
{
    [Fact]
    public void Forbidden_Build_allows_central_ServiceProvider_build()
    {
        var services = new FakeServiceCollection();

        services.TryAddScoped<IScoped, Scoped>();

        var serviceProvider = services.ServiceProvider;

        serviceProvider.Should().NotBeNull();
    }

    [Fact]
    public void Forbidden_Build_forbids_local_ServiceProvider_build()
    {
        var services = new FakeServiceCollection();

        services.TryAddScoped<IScoped, Scoped>();

        var action = () => services.BuildServiceProvider();

        action.Should().Throw<FakesSetupException>().WithMessage("*not allowed*ServiceProvider*yourself*");
    }

    [Fact]
    public void Permitted_Build_allows_central_ServiceProvider_build()
    {
        var services = new FakeServiceCollection();

        services.TryAddScoped<IScoped, Scoped>();

        var serviceProvider = services.ServiceProvider;

        serviceProvider.Should().NotBeNull();
    }

}
