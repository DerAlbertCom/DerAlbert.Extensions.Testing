using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace DerAlbert.Extensions.Fakes.Internal;

internal class FakeFactory : IFakeAccessor
{
    private readonly Dictionary<Type, object> _existingMocks = new();

    public FakeFactory(FakeServiceCollection services)
    {
        _fakeServices = services;
    }

    public object The(Type type)
    {
        if (_existingMocks.TryGetValue(type, out var existing))
        {
            return existing;
        }

        var instance = CreateInstance(type, false);
        if (instance == null)
        {
            throw new FakesSetupException($"Was not able to create an instance of {type.Name}");
        }

        _existingMocks[type] = instance;
        return instance;
    }

    public object An(Type type)
    {
        var instance = CreateInstance(type, true);
        if (instance == null)
        {
            throw new FakesSetupException($"Was not able to create an instance of {type.Name}");
        }

        return instance;
    }


    internal object? CreateInstance(Type type, bool mockOnly)
    {
        object? instance = null;
        if (!mockOnly && _fakeServices.ServicesAvailable)
        {
            if (IsServiceAvailable(type))
            {
                instance = ServiceProvider.GetService(type);
            }
        }

        if (instance == null && type.GetTypeInfo().IsAbstract)
        {
            instance = Substitute.For(new[] { type }, null);
        }

        return instance;
    }

    private bool IsServiceAvailable(Type type)
    {
        if (_fakeServices.Any(d => d.ServiceType == type))
        {
            return true;
        }

        if (type.IsGenericType)
        {
            type = type.GetGenericTypeDefinition();
            return _fakeServices.Any(d => d.ServiceType == type);
        }

        if (type == typeof(IServiceProvider))
        {
            return true;
        }

        return false;
    }

    public TMock The<TMock>() where TMock : class
    {
        return (TMock)The(typeof(TMock));
    }

    public TMock An<TMock>() where TMock : class
    {
        return (TMock)An(typeof(TMock));
    }

    public void Inject<TMock>(TMock instance) where TMock : class
    {
        Inject(typeof(TMock), instance);
    }

    public IServiceCollection Services => _fakeServices;

    public void Inject(Type parameterType, object inst)
    {
        _existingMocks[parameterType] = inst;
    }

    private readonly Dictionary<Type, IMockBehavior> _existingBehaviours = new();
    private readonly FakeServiceCollection _fakeServices;
    private IServiceProvider? _scopedProvider;

    public TBehavior With<TBehavior>(params object[] parameters) where TBehavior : class, IMockBehavior
    {
        if (_existingBehaviours.TryGetValue(typeof(TBehavior), out var behaviour))
        {
            return (TBehavior)behaviour;
        }

        var allParameters = new[] { this }.Concat(parameters).ToArray();

        var parameterTypes = allParameters.Select(c => c.GetType()).ToArray();
        var ctor = typeof(TBehavior).GetConstructor(parameterTypes);
        if (ctor == null)
        {
            throw new InvalidOperationException(
                $"Missing Ctor with types ({string.Join(", ", parameterTypes.Select(t => t.Name))}) on behavior {typeof(TBehavior).Name}");
        }

        var instance = (IMockBehavior)ctor.Invoke(allParameters);
        instance.OnEstablish();
        _existingBehaviours[typeof(TBehavior)] = instance;
        return (TBehavior)instance;
    }

    public object CallbackInServiceScope(Func<FakeFactory, object> callback)
    {
        if (!_fakeServices.ServicesAvailable)
        {
            return callback(this);
        }
        else
        {
            var scope = _fakeServices.ServiceProvider.CreateScope();
            _scopedProvider = scope.ServiceProvider;
            var result = callback(this);
            _scopedProvider = null;
            return result;
        }
    }

    internal bool TryExistingThe(Type parameterType, out object instance)
    {
        return _existingMocks.TryGetValue(parameterType, out instance);
    }

    internal IServiceProvider ServiceProvider => _scopedProvider ?? _fakeServices.ServiceProvider;

    // internal FakeMode FakeMode { get; private set; }
}
