using DatingAPI.Models;
using System.Security.Claims;

namespace DatingAPI.Extensions
{
    public static class ClaimPrincipleExtensions
    {
        public static string GetUsername (this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
