using BlogApi.Models;
using System.Security.Claims;

namespace BlogApi.Extensions
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.Email));

            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug)));

            return claims;
        }
    }
}
