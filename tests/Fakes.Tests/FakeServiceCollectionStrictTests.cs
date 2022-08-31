using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class FakeServiceCollectionStrictTests
{
    [Fact]
    public void Should_throw_when_register_services_after_building_a_service_provider()
    {
        var services = new FakeServiceCollection(BuildServiceProviderMode.Permitted);

        services.AddTransient<IA, A>();

        var sp = services.BuildServiceProvider();

        sp.Should().NotBeNull();

        var action = () => services.AddTransient<IA, B>();

        action.Should().Throw<FakesSetupException>().WithMessage("*Add**ServiceProvider*");
    }

    [Fact]
    public void Should_throw_when_building_a_service_provider_and_no_services_a_registered()
    {
        var services = new FakeServiceCollection(BuildServiceProviderMode.Permitted);

        var action = () => services.ServiceProvider;

        action.Should().Throw<FakesSetupException>().WithMessage("*ServiceProvider*Services*");
    }
}