using NSubstitute;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class DummyMockBehavior : MockBehaviorBase
{
    public DummyMockBehavior(IFakeAccessor accessor) : base(accessor)
    {
    }

    public override void OnEstablish()
    {
        throw new System.NotImplementedException();
    }
}

public class MockBehaviourBaseTests : WithSubject<DummyMockBehavior>
{
    [Fact]
    public void The_should_call_fakeAccessor()
    {
        Subject.The<IFakeTest>();

        The<IFakeAccessor>().Received(1).The<IFakeTest>();
    }

    [Fact]
    public void An_should_call_fakeAccessor()
    {
        Subject.An<IFakeTest>();

        The<IFakeAccessor>().Received(1).An<IFakeTest>();
    }

    [Fact]
    public void Inject_should_call_fakeAccessor()
    {
        var instance = An<IFakeTest>();
        Subject.Inject(instance);
        The<IFakeAccessor>().Received(1).Inject<IFakeTest>(instance);
    }

    [Fact]
    public void With_should_call_fakeAccessor()
    {
        Subject.With<FluentTestBehavior>();
        The<IFakeAccessor>().Received(1).With<FluentTestBehavior>();
    }
}