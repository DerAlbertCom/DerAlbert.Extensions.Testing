using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace DerAlbert.Extensions.Fakes.Internal;

internal class FakeServiceCollection : IServiceCollection, IDisposable
{
    internal bool Modified { get; private set; } = true;

    public FakeServiceCollection(BuildServiceProviderMode buildServiceProviderMode = BuildServiceProviderMode.Forbidden)
    {
        _buildServiceProviderMode = buildServiceProviderMode;
        _services = new ServiceCollection();
    }

    public IEnumerator<ServiceDescriptor> GetEnumerator()
    {
        return _services.GetEnumerator();
    }

    public void Add(ServiceDescriptor item)
    {
        EnsureModificationIsAllowed();
        Modified = true;
        _services.Add(item);
    }

    public void Clear()
    {
        EnsureModificationIsAllowed();
        Modified = true;
        _services.Clear();
    }

    public bool Contains(ServiceDescriptor item)
    {
        return _services.Contains(item);
    }

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
    {
        EnsureBuildingAServiceProviderIsAllowed();
        // this is the code that will be called after building a service provider
        // should be enough for unit testing scenarios
        _serviceProviderWasBuild = true;
        _services.CopyTo(array, arrayIndex);
    }

    public bool Remove(ServiceDescriptor item)
    {
        EnsureModificationIsAllowed();
        Modified = true;
        return _services.Remove(item);
    }


    public int Count => _services.Count;

    internal bool ServicesAvailable => _services.Count > 0;

    public bool IsReadOnly => _services.IsReadOnly;

    public int IndexOf(ServiceDescriptor item)
    {
        return _services.IndexOf(item);
    }

    public void Insert(int index, ServiceDescriptor item)
    {
        EnsureModificationIsAllowed();
        Modified = true;
        _services.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        EnsureModificationIsAllowed();
        Modified = true;
        _services.RemoveAt(index);
    }

    public ServiceDescriptor this[int index]
    {
        get => _services[index];
        set
        {
            EnsureModificationIsAllowed();
            Modified = true;
            _services[index] = value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


    public IServiceProvider ServiceProvider
    {
        get
        {
            if (Modified)
            {
                if (_services.Count == 0)
                {
                    throw new FakesSetupException(
                        $"In Strict mode you cannot create a ServiceProvider without registered Services. For this you have enabled FakeMode.Lax");
                }

                _internalServiceProviderBuild = true;
                _serviceProvider = _services.BuildServiceProvider(true);
                _internalServiceProviderBuild = false;
                _serviceProviderWasBuild = true;
                _serviceProviders.Push(_serviceProvider);
                Modified = false;
            }

            return _serviceProvider!;
        }
    }

    public void Dispose()
    {
        while (_serviceProviders.Count > 0)
        {
            var serviceProvider = _serviceProviders.Pop();
            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    private void EnsureBuildingAServiceProviderIsAllowed()
    {
        if (_buildServiceProviderMode == BuildServiceProviderMode.Forbidden)
        {
            if (!_internalServiceProviderBuild)
            {
                throw new FakesSetupException(
                    "It not allowed to create ServiceProvider by yourself. For this you must enable BuildServiceProviderMode.Permitted");
            }
        }

        // only an assumption; because BuildServiceProvider does not copy the service collection
        // when no services are registered, so it hase to checked before building the Service Provider
        if (_services.Count == 0)
        {
            throw new FakesSetupException(
                $"In Strict mode you cannot create a ServiceProvider without registered Services. For this you have enabled FakeMode.Lax");
        }
    }

    private void EnsureModificationIsAllowed([CallerMemberName] string? memberName = default)
    {
        if (_serviceProviderWasBuild)
        {
            throw new FakesSetupException(
                $"In Strict mode you cannot use {memberName} to modify the ServiceCollection after building a ServiceProvider");
        }
    }


    private readonly IServiceCollection _services;

    private IServiceProvider? _serviceProvider;
    private readonly Stack<IServiceProvider> _serviceProviders = new();
    private readonly BuildServiceProviderMode _buildServiceProviderMode;
    private bool _serviceProviderWasBuild;
    private bool _internalServiceProviderBuild;
}
