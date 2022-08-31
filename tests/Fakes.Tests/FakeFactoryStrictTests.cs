using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class FakeFactoryStrictTests 
{
        
    [Fact]
    public void The_resolve_IServiceProvider_to_mocked_IServiceProvider()
    {
        var fakeFactory = CreateStrictFactory();

        var sp = fakeFactory.The<IServiceProvider>();

        sp.Should().NotBeNull();
            
        sp.GetType().FullName.Should().StartWith("Castle.Proxies.");
    }

    private FakeFactory CreateStrictFactory()
    {
        return new FakeFactory(new FakeServiceCollection(FakeMode.Strict));
    }
}