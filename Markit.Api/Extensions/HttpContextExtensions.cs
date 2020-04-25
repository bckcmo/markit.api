using System.Runtime.CompilerServices;
using System.Security.Authentication;
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

        public static int GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            int.TryParse(stringId ?? "0", out int userId);

            return userId;
        }
    }
}