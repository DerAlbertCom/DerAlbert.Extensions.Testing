using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class FakeFactoryWithServicesTests
{
    private FakeFactory CreateFactory()
    {
        var serviceCollection = new FakeServiceCollection(FakeMode.Lax);
        serviceCollection.AddTransient<IFakeTest, FakeTest>();
        return new FakeFactory(serviceCollection);
    }

    [Fact]
    public void Create_an_instance_of_Service()
    {
        var fakeFactory = CreateFactory();
        var first = fakeFactory.The<IFakeTest>();

        first.Should().NotBeNull();

        first.Should().BeOfType<FakeTest>();
    }

    [Fact]
    public void Create_same_instance_of_Service()
    {
        var fakeFactory = CreateFactory();
        var first = fakeFactory.The<IFakeTest>();
        var second = fakeFactory.The<IFakeTest>();

        first.Should().NotBeNull();

        first.Should().BeSameAs(second);
        first.Should().BeOfType<FakeTest>();
    }

    [Fact]
    public void An_only_creates_the_mock()
    {
        var fakeFactory = CreateFactory();
        var first = fakeFactory.The<IFakeTest>();
        var second = fakeFactory.An<IFakeTest>();

        first.Should().NotBeNull();
        second.Should().NotBeNull();

        first.Should().NotBeSameAs(second);

        first.Should().BeOfType<FakeTest>();
        second.Should().NotBeOfType<FakeTest>();
    }
}

public class DiInstanceCheck
{
    public IFakeTest Test { get; }

    public DiInstanceCheck(IFakeTest test)
    {
        Test = test;
    }
}

public class FakeTest : IFakeTest
{
}

public interface IFakeTest
{
}