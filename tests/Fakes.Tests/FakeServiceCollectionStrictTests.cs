using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class FakeServiceCollectionStrictTests
{

    [Fact]
    public void Should_throw_when_building_a_service_provider_and_no_services_a_registered()
    {
        var services = new FakeServiceCollection();

        var action = () => services.ServiceProvider;

        action.Should().Throw<FakesSetupException>().WithMessage("*ServiceProvider*Services*");
    }
}
