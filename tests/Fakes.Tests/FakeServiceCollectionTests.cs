using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class TheOne
{
    private readonly IClock _clock;
    private readonly IScoped _scoped;

    public TheOne(IClock clock, IScoped scoped)
    {
        _clock = clock;
        _scoped = scoped;
    }

    public bool IsScopedDisposed()
    {
        return _scoped.Disposed;
    }
}

public abstract class ClockSubject<T> : WithSubject<T> where T : class
{
    public ClockSubject() : base(FakeMode.Strict)
    {
        With<FakeServiceCollectionTests.AddingServiceBehavior>();
    }
}

public class FakeServiceCollectionTests : ClockSubject<TheOne>
{
    public FakeServiceCollectionTests()
    {
    }

    [Fact]
    public void Can_try_add_services()
    {
        var scopeDisposed =Subject.IsScopedDisposed();
        scopeDisposed.Should().BeFalse();
        
    }

    public class AddingServiceBehavior : MockBehaviorBase
    {
        public AddingServiceBehavior(IFakeAccessor accessor) : base(accessor)
        {
        }

        public override void OnEstablish()
        {
            var instance = The<IClock>();
            Services.TryAddSingleton(instance);
            Services.AddScoped<IScoped, Scoped>();
        }
    }
}