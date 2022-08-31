using FluentAssertions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class SubjectFactories
{
    private readonly Factories _factories;

    public SubjectFactories()
    {
        _factories = new Factories();
    }

    [Fact]
    public void Creates_an_instance()
    {
        var subjectFactory = _factories.CreateSubjectFactory();
        var result = subjectFactory.Create<SubjectTestClass>();
        result.Should().NotBeNull();
    }

    [Fact]
    public void Inject_The_instances()
    {
        var subjectFactory = _factories.CreateSubjectFactory();
        var result = subjectFactory.Create<SubjectTestClass>();

        result.One.Should().BeSameAs(result.Two);
    }

    [Fact]
    public void Inject_also_concrete_class()
    {
        var subjectFactory = _factories.CreateSubjectFactory();
        var result = subjectFactory.Create<SubjectTestClass>();

        result.One.Should().BeSameAs(result.Injected.One);
    }

    [Fact]
    public void Inject_injected_instances()
    {
        var (subjectFactory, fakeFactory) = _factories.CreateFactories();
        var o1 = new FooBarOptions();
        var o2 = new BarFooOptions();
        fakeFactory.Inject(o1);
        fakeFactory.Inject(o2);

        var result = subjectFactory.Create<FooBar>();

        result.Options1.Should().BeSameAs(o1);
        result.Options2.Should().BeSameAs(o2);
    }
}

public class FooBar
{
    public FooBarOptions Options1 { get; }
    public BarFooOptions Options2 { get; }


    public FooBar(FooBarOptions options1, BarFooOptions options2)
    {
        Options1 = options1;
        Options2 = options2;
    }
}

public class BarFooOptions
{
}

public class FooBarOptions
{
}
