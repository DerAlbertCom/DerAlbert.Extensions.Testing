using FluentAssertions;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class SubjectFactoryIntegrationTest : WithSubject<SubjectTestClass>
{
    [Fact]
    public void IsCorrectlyInjected()
    {
        Subject.One.Should().BeSameAs(Subject.Two);

        Subject.Info.Should().NotBeNull();
    }
}