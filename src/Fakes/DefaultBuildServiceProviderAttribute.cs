namespace DerAlbert.Extensions.Fakes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public class DefaultBuildServiceProviderAttribute : Attribute
{
    public BuildServiceProviderMode BuildServiceProviderMode { get; }

    public DefaultBuildServiceProviderAttribute(BuildServiceProviderMode buildServiceProviderMode)
    {
        BuildServiceProviderMode = buildServiceProviderMode;
    }
}