using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResourceBookingAPI.Configuration;
using ResourceBookingAPI.DTOs;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResourceBookingAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly ILoginRepos _loginRepo;
        public JwtService(IOptions<JwtConfig> jwtConfig, ILoginRepos loginRepo)
        {
            _jwtConfig = jwtConfig.Value;
            _loginRepo = loginRepo;
        }

        public async Task<LoginResponseDto?> GenerateToken(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var user = await _loginRepo.GetUser(request.Username);
            if (user == null || !await _loginRepo.ValidatePassword(user, request.Password))
                return null;

            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Key)),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseDto
            {
                UserId = user.Id,
                InstitutionId = user.InstitutionId,
                AccessToken = accessToken,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
