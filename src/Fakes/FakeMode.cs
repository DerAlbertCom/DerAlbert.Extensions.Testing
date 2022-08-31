namespace DerAlbert.Extensions.Fakes;

public enum FakeMode
{
    // Can change Services Registration after Building a Service Provider
    Lax = 0,
    // Cannot change Service Registration after building a Service Provider
    Strict = 1
}

public enum BuildServiceProviderMode
{
    Forbidden = 0,
    Permitted = 1
}