using DerAlbert.Extensions.Fakes;

namespace Integration.DefaultStrict.Tests;

public class UnitTest1 : WithFakes
{
    [Fact]
    public void The_resolve_IServiceProvider_to_mocked_IServiceProvider()
    {

        var sp = The<IServiceProvider>();

        sp.Should().NotBeNull();
            
        sp.GetType().FullName.Should().StartWith("Castle.Proxies.");
    }
}