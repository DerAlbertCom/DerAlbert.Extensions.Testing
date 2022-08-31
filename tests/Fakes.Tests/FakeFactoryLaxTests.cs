using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class FakeFactoryLaxTests 
{
        
    [Fact]
    public void The_resolve_IServiceProvider_to_concrete_ServiceProvider()
    {
        var fakeFactory = CreateLaxFactory();

        var sp = fakeFactory.The<IServiceProvider>();

        sp.Should().NotBeNull();

        sp.GetType().FullName.Should().StartWith("Microsoft.Extensions.DependencyInjection");

    }

    private FakeFactory CreateLaxFactory()
    {
        return new FakeFactory(new FakeServiceCollection(FakeMode.Lax));
    }
}