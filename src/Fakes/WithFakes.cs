using DerAlbert.Extensions.Fakes.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace DerAlbert.Extensions.Fakes;

public abstract class WithFakes : IFakeAccessor, IDisposable
{
    private readonly FakeFactory _factory;
    private readonly FakeServiceCollection _serviceCollection;

    /// <inheritdoc />
    public IServiceCollection Services => _serviceCollection;

    protected WithFakes()
    {
        _serviceCollection = new FakeServiceCollection(GlobalConfig.DefaultFakeMode, GlobalConfig.DefaultBuilderServiceProviderMode);
        _factory = new FakeFactory(_serviceCollection);
    }

    protected WithFakes(FakeMode fakeMode)
    {
        _serviceCollection = new FakeServiceCollection(fakeMode);
        _factory = new FakeFactory(_serviceCollection);
    }

    /// <summary>
    /// Creates a Subject under Test, normally you Should Use WithSubject&lt;T&gt; this is only for creating
    /// Subject of Internal Classes
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    /// <param name="modifyAction"></param>
    /// <returns></returns>
    protected TSubject CreateSubjectUnderTest<TSubject>(Action<TSubject>? modifyAction = null)
        where TSubject : class
    {
        var subjectFactory = new SubjectFactory(_factory);

        var subject = subjectFactory.Create<TSubject>();
        modifyAction?.Invoke(subject);

        return subject;
    }

    /// <inheritdoc />
    public TMock The<TMock>() where TMock : class => _factory.The<TMock>();

    /// <inheritdoc />
    public TMock An<TMock>() where TMock : class => _factory.An<TMock>();

    /// <inheritdoc />
    public void Inject<TMock>(TMock instance) where TMock : class => _factory.Inject(instance);

    /// <inheritdoc />
    public TBehavior With<TBehavior>(params object[] parameters) where TBehavior : class, IMockBehavior
    {
        return _factory.With<TBehavior>(parameters);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _serviceCollection.Dispose();
        }
    }

    protected TResult FromServiceScope<TService, TResult>(Func<TService, TResult> callback) where TService : notnull
    {
        var scope = _factory.ServiceProvider.CreateScope();
        return callback(scope.ServiceProvider.GetRequiredService<TService>());
    }

    protected async Task<TResult> FromServiceScopeAsync<TService, TResult>(Func<TService, Task<TResult>> callback) where TService : notnull
    {
        var scope = _factory.ServiceProvider.CreateScope();
        return await callback(scope.ServiceProvider.GetRequiredService<TService>());
    }

    protected void InServiceScope<TService>(Action<TService> callback) where TService : notnull
    {
        var scope = _factory.ServiceProvider.CreateScope();
        callback(scope.ServiceProvider.GetRequiredService<TService>());
    }

    protected async Task InServiceScopeAsync<TService>(Func<TService, Task> callback) where TService : notnull
    {
        var scope = _factory.ServiceProvider.CreateScope();
        await callback(scope.ServiceProvider.GetRequiredService<TService>());
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~WithFakes()
    {
        Dispose(false);
    }
}
