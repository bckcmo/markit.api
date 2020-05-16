using System.Security.Claims;
using Markit.Api.Models.Statics;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Markit.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool IsUserAllowed(this IHttpContextAccessor httpContextAccessor, int id)
        {
            return httpContextAccessor.GetCurrentUserId() == id;
        }
        
        public static bool IsSuperUser(this IHttpContextAccessor httpContextAccessor)
        {
            var stringRole = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            int.TryParse(stringRole ?? Roles.User.ToString(), out var role);
            return role == (int) Roles.SuperUser;
        }

        public static int GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            int.TryParse(stringId ?? "0", out int userId);

            return userId;
        }
    }
}