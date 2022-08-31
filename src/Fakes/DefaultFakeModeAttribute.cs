namespace DerAlbert.Extensions.Fakes;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public class DefaultFakeModeAttribute : Attribute
{
    public FakeMode FakeMode { get; }

    public DefaultFakeModeAttribute(FakeMode fakeMode)
    {
        FakeMode = fakeMode;
    }
}