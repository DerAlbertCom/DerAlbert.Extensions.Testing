using DerAlbert.Extensions.Fakes;
using DerAlbert.Extensions.Xunit.Logging;
using FluentAssertions;
using Xunit;

namespace DerAlbert.Extensions.Xunit.Tests.Logging;

public class TestOutputHelperLoggingProviderTests : WithFakes
{
    public TestOutputHelperLoggingProviderTests()
    {
        Subject = CreateSubjectUnderTest<TestOutputHelperLoggingProvider>();
    }

    [Fact]
    public void Should_create_a_logger()
    {
        var logger = Subject.CreateLogger("Ding");
        logger.Should().BeOfType<TestOutputHelperLogger>();
    }

    private TestOutputHelperLoggingProvider Subject { get; set; }
}