using System.Security.Claims;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DerAlbert.Extensions.Fakes.Tests;

public class FluentBehaviorTest : WithSubject<FluentTestClass>
{
    public FluentBehaviorTest()
    {
        With<FluentTestBehavior>()
            .UseUser(new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("name", "foobar")
            }, "ding", "name", "role")));
    }

    [Fact]
    public void UserHaseName()
    {
        Subject.GetUser().Identity!.Name.Should().Be("foobar");
    }
}

public abstract class HttpContextBase
{
    public abstract ClaimsPrincipal User { get; }
}

public static class FluentTestBehaviorExtensions
{
    public static FluentTestBehavior UseUser(this FluentTestBehavior behavior, ClaimsPrincipal user)
    {
        behavior.Accessor.The<HttpContextBase>().User.Returns(user);
        return behavior;
    }
}

public class FluentTestBehavior : MockBehaviorBase
{
    public FluentTestBehavior(IFakeAccessor accessor) : base(accessor)
    {
    }

    public override void OnEstablish()
    {
    }
}

public class FluentTestClass
{
    private readonly HttpContextBase _httpContext;

    public FluentTestClass(HttpContextBase httpContext)
    {
        _httpContext = httpContext;
    }

    public ClaimsPrincipal GetUser()
    {
        return _httpContext.User;
    }
}