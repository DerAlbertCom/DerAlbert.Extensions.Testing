using System.Reflection;

namespace DerAlbert.Extensions.Fakes.Internal;

internal static class GlobalConfig
{
    private static FakeMode? _fakeMode;
    private static BuildServiceProviderMode? _providerMode;

    public static FakeMode DefaultFakeMode
    {
        get
        {
            if (!_fakeMode.HasValue)
            {
                _fakeMode = GetDefaultFakeMode();
            }

            return _fakeMode.Value;
        }
    }

    public static BuildServiceProviderMode DefaultBuilderServiceProviderMode
    {
        get
        {
            if (!_providerMode.HasValue)
            {
                _providerMode = GetDefaultBuilderServiceProviderMode();
            }

            return _providerMode.Value;
        }
    }

    private static BuildServiceProviderMode GetDefaultBuilderServiceProviderMode()
    {
        var attributes = FindAssemblyAttributes<DefaultBuildServiceProviderAttribute>();

        if (attributes.Length == 0)
        {
            return BuildServiceProviderMode.Forbidden;
        }

        if (attributes.Length > 1)
        {
            throw new FakesSetupException($"Multiple usages of {nameof(DefaultBuildServiceProviderAttribute)} found.");
        }

        return attributes[0].BuildServiceProviderMode;
    }

    private static FakeMode GetDefaultFakeMode()
    {
        var attributes = FindAssemblyAttributes<DefaultFakeModeAttribute>();

        if (attributes.Length == 0)
        {
            return FakeMode.Strict;
        }

        if (attributes.Length > 1)
        {
            throw new FakesSetupException($"Multiple usages of {nameof(DefaultFakeModeAttribute)} found.");
        }

        return attributes[0].FakeMode;
    }

    private static TAttribute[] FindAssemblyAttributes<TAttribute>() where TAttribute : Attribute
    {
        var domain = AppDomain.CurrentDomain;
        var attributes = domain.GetAssemblies()
            .SelectMany(a => a.GetCustomAttributes<TAttribute>());

        return attributes.ToArray();
    }
}