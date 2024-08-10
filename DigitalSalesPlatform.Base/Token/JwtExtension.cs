using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DigitalSalesPlatform.Base.Token;

public static class JwtManager
{
    public static Session GetSession(HttpContext context)
    {
        if (context == null || context.User == null || !context.User.Identity.IsAuthenticated)
            return null;

        Session session = new Session();
        var identity = context.User.Identity as ClaimsIdentity;
        var claims = identity?.Claims;

        if (claims != null)
        {
            session.UserId = GetClaimValue(claims, ClaimTypes.NameIdentifier);
            session.Role = GetClaimValue(claims, ClaimTypes.Role);
            session.Email = GetClaimValue(claims, ClaimTypes.Email);
        }

        return session;
    }

    private static string GetClaimValue(IEnumerable<Claim> claims, string name)
    {
        var claim = claims.FirstOrDefault(c => c.Type == name);
        return claim?.Value;
    }
}