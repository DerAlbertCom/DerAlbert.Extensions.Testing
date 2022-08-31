using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

internal interface IA
{
}

internal record A() : IA;

internal record B() : IA;

public class FakeServiceCollectionLaxTests
{
    [Fact]
    public void Should_able_to_register_services_after_building_a_service_provider()
    {
        var services = new FakeServiceCollection(FakeMode.Lax);

        services.AddTransient<IA, A>();

        var sp = services.ServiceProvider;

        sp.Should().NotBeNull();

        var action = () => services.AddTransient<IA, B>();

        action.Should().NotThrow();
    }
    
    [Fact]
    public void Should_not_throw_when_building_a_service_provider_and_no_services_a_registered()
    {
        var services = new FakeServiceCollection(FakeMode.Lax);

        var action = ()=> services.ServiceProvider;

        action.Should().NotThrow();
    }
}