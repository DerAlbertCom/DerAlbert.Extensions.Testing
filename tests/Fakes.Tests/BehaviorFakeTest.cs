using Microsoft.Extensions.DependencyInjection;

namespace DerAlbert.Extensions.Fakes.Tests;

public class BehaviorFakeTest : IMockBehavior
{
    private readonly IFakeAccessor _accessor;

    public BehaviorFakeTest(IFakeAccessor accessor)
    {
        _accessor = accessor;
    }
    public void OnEstablish()
    {
        _accessor.Services.AddTransient<IFakeTest,FakeTest>();
    }
}