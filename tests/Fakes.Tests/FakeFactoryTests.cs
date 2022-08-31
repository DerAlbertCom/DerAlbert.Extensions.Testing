using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class FakeFactoryTests
{
    [Fact]
    public void The_gives_The_Same_Instance()
    {
        var factory = new FakeFactory(new FakeServiceCollection());
        var first = factory.The<IAsyncLifetime>();
        var second = factory.The<IAsyncLifetime>();
        var third = factory.The<IAsyncLifetime>();

        first.Should().BeSameAs(second);
        second.Should().BeSameAs(third);
    }

    [Fact]
    public void An_gives_new_Instances()
    {
        var factory = new FakeFactory(new FakeServiceCollection());
        var first = factory.An<IAsyncLifetime>();
        var second = factory.An<IAsyncLifetime>();
        var third = factory.An<IAsyncLifetime>();

        first.Should().NotBeSameAs(second);
        second.Should().NotBeSameAs(third);
        first.Should().NotBeSameAs(third);
    }

    [Fact]
    public void The_returns_the_injected_instance()
    {
        var factory = new FakeFactory(new FakeServiceCollection());
        var first = factory.An<IAsyncLifetime>();
        factory.Inject(first);
        var second = factory.The<IAsyncLifetime>();
        first.Should().BeSameAs(second);
    }

    [Fact]
    public void An_does_not_returns_the_injected_instance()
    {
        var factory = new FakeFactory(new FakeServiceCollection());
        var first = factory.An<IAsyncLifetime>();
        factory.Inject(first);

        var second = factory.An<IAsyncLifetime>();
        first.Should().NotBeSameAs(second);
    }

    [Fact]
    public void The_should_return_the_injected_instance_of_a_concrete_class()
    {
        var factory = new FakeFactory(new FakeServiceCollection());
        var first = new FooBarOptions();
        factory.Inject(first);

        var second = factory.The<FooBarOptions>();
        first.Should().BeSameAs(second);
    }
}
