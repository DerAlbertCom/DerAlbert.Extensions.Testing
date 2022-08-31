using FluentAssertions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class WithBehaviorTests : WithSubject<BehaviourTestClass>
{
    public WithBehaviorTests()
    {
        With<BehaviorFakeTest>();
    }

    [Fact]
    public void FakeTest()
    {
        Subject.FakeTest.Should().BeOfType<FakeTest>();
    }
}  

public class BehaviourTestClass
{
    public IFakeTest FakeTest { get; }

    public BehaviourTestClass(IFakeTest fakeTest)
    {
        FakeTest = fakeTest;
    }
}