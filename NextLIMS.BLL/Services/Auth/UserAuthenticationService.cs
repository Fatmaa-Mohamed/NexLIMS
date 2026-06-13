using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NextLIMS.DAL.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NextLIMS.BLL.Services.Auth
{
    public class UserAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepository;

        public UserAuthenticationService(
            IConfiguration configuration,
            UserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<object> LoginAsync(
            string email,
            string password,
            int tenantId)
        {
            var user = await _userRepository
                .GetByEmailAndTenantAsync(email, tenantId);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");


            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");  // ✅ throw, not return

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("TenantId", user.TenantId.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(
                    Convert.ToInt32(_configuration["Jwt:ExpireHoures"])),
                signingCredentials: creds);

            return new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}