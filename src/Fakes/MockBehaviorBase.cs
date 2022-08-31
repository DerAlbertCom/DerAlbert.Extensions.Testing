using Microsoft.Extensions.DependencyInjection;

namespace DerAlbert.Extensions.Fakes;

public abstract class MockBehaviorBase : IMockBehavior, IFakeAccessor
{
    public IFakeAccessor Accessor { get; }

    protected MockBehaviorBase(IFakeAccessor accessor)
    {
        Accessor = accessor;
    }
        
    public abstract void OnEstablish();

    /// <inheritdoc />
    public TMock The<TMock>() where TMock : class
    {
        return Accessor.The<TMock>();
    }

    /// <inheritdoc />
    public TMock An<TMock>() where TMock : class
    {
        return Accessor.An<TMock>();
    }

    /// <inheritdoc />
    public void Inject<TMock>(TMock instance) where TMock : class
    {
        Accessor.Inject(instance);
    }

    /// <inheritdoc />
    public IServiceCollection Services => Accessor.Services;

    /// <inheritdoc />
    public TBehaviour With<TBehaviour>(params object[] parameters) where TBehaviour : class, IMockBehavior
    {
        return Accessor.With<TBehaviour>(parameters);
    }
}