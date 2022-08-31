using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class SubjectFactoryLaxTests 
{
    private readonly Factories _factories;

    public SubjectFactoryLaxTests()
    {
        _factories = new Factories();
    }
    
    [Fact]
    public void Is_same_instance_injection_in_services()
    {
        var (subjectFactory, fakeFactory) = _factories.CreateFactories(FakeMode.Lax);

        var i1 = fakeFactory.An<IFakeTest>();
        fakeFactory.Services.AddSingleton(i1);

        var result = subjectFactory.Create<DiInstanceCheck>();

        result.Should().NotBeNull();
        result.Test.Should().BeSameAs(i1);
    }
}