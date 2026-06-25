using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace NexLIMS.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetRequiredIntClaim(
            this ClaimsPrincipal user,
            string claimName)
        {
            var value = user.FindFirst(claimName)?.Value;

            if (!int.TryParse(value, out var result))
                throw new UnauthorizedAccessException(
                    $"Missing or invalid claim: {claimName}");

            return result;
        }

        public static string GetRequiredClaim(
            this ClaimsPrincipal user,
            string claimName)
        {
            var value = user.FindFirst(claimName)?.Value;

            if (string.IsNullOrWhiteSpace(value))
                throw new UnauthorizedAccessException(
                    $"Missing claim: {claimName}");

            return value;
        }
    }
}