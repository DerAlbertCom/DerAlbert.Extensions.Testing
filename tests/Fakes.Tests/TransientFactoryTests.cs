using DerAlbert.Extensions.Fakes.Internal;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class TransientFactoryTests
{
    public class FactoryFakeTest : IFakeFactoryTest
    {
        public int Value { get; }

        public FactoryFakeTest(int value)
        {
            Value = value;
        }
    }

    public class MainFactoryTest1
    {
        public MainFactoryTest1(FactoryFakeTest test)
        {
            Value = test.Value;
        }
        public int Value { get; set; }
    }
    public class MainFactoryTest2
    {
        public MainFactoryTest2(IFakeFactoryTest test)
        {
            Value = test.Value;
        }
        public int Value { get; set; }
    }

    private FakeFactory CreateFakeFactory()
    {
        var serviceCollection = new FakeServiceCollection(FakeMode.Lax);
        serviceCollection.AddTransient<FactoryFakeTest>(sp => new FactoryFakeTest(13));
        serviceCollection.AddTransient<IFakeFactoryTest>(sp => sp.GetRequiredService<FactoryFakeTest>());
        return new FakeFactory(serviceCollection);
    }

    [Fact]
    public void Should_not_resolve_it_with_an()
    {
        var fakeFactory = CreateFakeFactory();
        var action = ()=> fakeFactory.An<FactoryFakeTest>();
        action.Should().Throw<FakesSetupException>();
    }

    [Fact]
    public void Should_resolve_it_with_The()
    {
        var fakeFactory = CreateFakeFactory();
        var r = fakeFactory.The<FactoryFakeTest>();
        r.Value.Should().Be(13);
    }

    [Fact]
    public void Should_resolve_concrete_over_subjectFactory()
    {
        var fakeFactory = CreateFakeFactory();
        var sf = new SubjectFactory(fakeFactory);

        var r= sf.Create<MainFactoryTest1>();
        r.Value.Should().Be(13);
    }

    [Fact]
    public void Should_resolve_interface_over_subjectFactory()
    {
        var fakeFactory = CreateFakeFactory();

        var sf = new SubjectFactory(fakeFactory);

        var r= sf.Create<MainFactoryTest2>();
        r.Value.Should().Be(13);
    }
}

public interface IFakeFactoryTest
{
    int Value { get; }
}
