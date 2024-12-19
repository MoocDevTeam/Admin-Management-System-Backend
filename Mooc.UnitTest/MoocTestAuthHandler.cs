using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Mooc.UnitTest;

public class MoocTestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public MoocTestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,"1"),
            new Claim(ClaimTypes.Name,"admin"),
        };

        var identity = new ClaimsIdentity(claims, "testBearer");
        var prncipal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(prncipal, "testScheme");
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}
