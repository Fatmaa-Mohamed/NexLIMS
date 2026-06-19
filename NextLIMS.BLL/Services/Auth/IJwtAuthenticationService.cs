using System.Security.Claims;

namespace NextLIMS.BLL.Services.Auth
{
    public interface IJwtAuthenticationService
    {
        string GenerateToken(
            IEnumerable<Claim> claims);
    }
}